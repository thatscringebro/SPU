using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SPU.Domain;
using SPU.Domain.Entites;
using System.Security.Claims;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SpuContext _context;
        private readonly string _loggedUserId;

        public ChatHub(SpuContext context, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            _loggedUserId = claim?.Value;
        }

        public static Dictionary<string, List<string>> ConnectedUsers = new ();

        /// <summary>
        /// Gestion de la connexion de l'utilisateur
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = _loggedUserId;
            lock(ConnectedUsers)
            {
                if (!ConnectedUsers.ContainsKey(userId))
                    ConnectedUsers[userId] = new();
                ConnectedUsers[userId].Add(connectionId);
            }
        }
        /// <summary>
        /// Gestion de la d�connexion de l'utilisateur 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var connectionId = Context.ConnectionId;
            var userId = _loggedUserId;

            lock (ConnectedUsers)
            {
                if (ConnectedUsers.ContainsKey(userId))
                {
                    ConnectedUsers[userId].Remove(connectionId);
                    if (ConnectedUsers[userId].Count == 0)
                        ConnectedUsers.Remove(userId);
                }
            }
        }
        /// <summary>
        /// Envoie les messages
        /// </summary>
        /// <param name="user">le nom de l'utilisateur qui envoie le message</param>
        /// <param name="room">Le chat qui poss�de le message</param>
        /// <param name="message">Le message</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string room, string message)
        {
            Message m = new Message{
                message = message,
                DateHeure = DateTime.UtcNow,
                UtilisateurId = Guid.Parse(user),
                ChatId = Guid.Parse(room), 
            };

            string username = _context.Utilisateurs.Find(Guid.Parse(user)).NomComplet;

            _context.Message.Add(m);
            _context.SaveChanges();

            List<string> coordos = _context.Coordonnateurs.Select(x => x.UtilisateurId.ToString()).ToList();
            var enseignantReal = _context.Chats.FirstOrDefault(x => x.Id.ToString() == room).EnseignantId;
            string enseignant = _context.Enseignants.FirstOrDefault(x => x.Id == enseignantReal).UtilisateurId.ToString();
            string stag = _context.Stagiaires.FirstOrDefault(x => x.ChatId.ToString() == room).UtilisateurId.ToString();
            List<string> mds = _context.MDS.Where(x => x.ChatId.ToString() == room).Select(x => x.UtilisateurId.ToString()).ToList();

            List<string> clientsToSend = ConnectedUsers[user];
            if (ConnectedUsers.ContainsKey(enseignant))
                clientsToSend.AddRange(ConnectedUsers[enseignant]);
            if (ConnectedUsers.ContainsKey(stag))
                clientsToSend.AddRange(ConnectedUsers[stag]);
            foreach (string id in mds)
            {
                if(ConnectedUsers.ContainsKey(id))
                    clientsToSend.AddRange(ConnectedUsers[id]);
            }
            foreach (string id in coordos)
            {
                if(ConnectedUsers.ContainsKey(id))
                  clientsToSend.AddRange(ConnectedUsers[id]);
            }

            await Clients.Clients(clientsToSend).SendAsync("ReceiveMessage", user, room, message, username, DateTime.Now.ToString("h:mmtt, MMM d"));
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using SPU.Domain;
using SPU.Domain.Entites;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SpuContext _context;

        public ChatHub(SpuContext context) 
        {
            _context = context;
        }

        public async Task SendMessage(string user, string room, string message)
        {
            Console.WriteLine("------------------------------");
            Message m = new Message{
                message = message,
                DateHeure = DateTime.UtcNow,
                UtilisateurId = Guid.Parse(user),
                ChatId = Guid.Parse(room), 
            };

            _context.Message.Add(m);
            _context.SaveChanges();

            await Clients.All.SendAsync("ReceiveMessage", user, room, message);
        }
    }
}

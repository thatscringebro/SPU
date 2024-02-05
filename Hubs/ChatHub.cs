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
            Chat chat = _context.Chats.FirstOrDefault(x => x.Id.ToString() == room);
            chat.message.Add(new Message{
                message = message,
                dateHeure = DateTime.Now,
                idUtilisateur = Guid.Parse(user),
            });

            _context.SaveChanges();

            await Clients.All.SendAsync("ReceiveMessage", user, room, message);
        }
    }
}

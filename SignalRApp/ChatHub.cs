using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRApp
{
    public class ChatHub : Hub
    {
        public static List<User> UserList = new List<User>();
        public void HerkeseGonder(string gonderen, string mesaj)
        {
            Clients.All.herkeseGonder(gonderen, mesaj, $"{DateTime.Now:g}");
        }

        public void OzelMesaj(string gonderenId, string aliciId, string mesaj)
        {
            Clients.Client(aliciId).mesajGeldi(gonderenId, mesaj);
        }

        public void Login(string kullaniciAdi, string id)
        {
            var giris = UserList.FirstOrDefault(x => x.Id == id);
            giris.UserName = kullaniciAdi;

            Clients.All.users(UserList);
        }

        public override Task OnConnected()
        {
            UserList.Add(new User()
            {
                Id = Context.ConnectionId
            });
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var silinecek = UserList.FirstOrDefault(x => x.Id == Context.ConnectionId);
            UserList.Remove(silinecek);
            Clients.All.users(UserList);
            return base.OnDisconnected(stopCalled);
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
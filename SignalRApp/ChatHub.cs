using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRApp
{
    public class ChatHub : Hub
    {

        public void HerkeseGonder(string gonderen, string mesaj)
        {
            Clients.All.herkeseGonder(gonderen, mesaj, $"{DateTime.Now:g}");
        }
    }
}
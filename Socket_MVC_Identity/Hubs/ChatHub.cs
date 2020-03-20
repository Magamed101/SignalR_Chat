using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Socket_MVC_Identity.Data;

namespace Socket_MVC_Identity.Hubs
{
    public class ChatHub : Hub
    {
        ApplicationDbContext context;
        public ChatHub()
        {
            context = new ApplicationDbContext(ApplicationDbContext.Opts());
        }
        //[Authorize(Policy = "chat")]
        public async Task SendMessage(string user, string message)
        {
            if (CheckRole(user))
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", user, "nothing");
            }
        }

        public bool CheckRole(string name)
        {
            var token = context.Users.First(x => x.UserName == name).Id;
            if (context.UserRoles.Any(x => x.RoleId == "9bde3682-adb2-4465-a7c6-438fea338b7c" && x.UserId == token))
                return true;
            return false;
        }
    }
}

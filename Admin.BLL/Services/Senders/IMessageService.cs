using Admin.Models.Enums;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Admin.BLL.Services.Senders
{
    public interface IMessageService
    {
        MessageStates MessageState { get; }

        Task SendAsync(IdentityMessage message, params string[] contacts);
        void Send(IdentityMessage message, params string[] contacts);
    }
}

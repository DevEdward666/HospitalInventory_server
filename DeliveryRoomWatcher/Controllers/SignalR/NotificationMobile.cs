using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QueueCore.Hubs;

namespace QueueCore.Controllers.SignalR
{
    [ApiController]
    public class NotificationMobile : ControllerBase
    {
        protected readonly IHubContext<NotifyMobileHub> _notifyhub;
        public NotificationMobile([NotNull] IHubContext<NotifyMobileHub> notifyhub)
        {
            _notifyhub = notifyhub;
        }
        [HttpPost]
        [Route("api/notificationmobile")]
        public async Task<IActionResult> SendMessage(mdlNotifications.NotificationPost notificationPost)
        {
            await _notifyhub.Clients.All.SendAsync("notifyfrommobile", notificationPost);
            return Ok();
        }
    }
}

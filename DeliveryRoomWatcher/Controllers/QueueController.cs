using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Hubs;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QueueCore.Models;
using QueueCore.Repositories;

namespace QueueCore.Controllers
{

    [ApiController]
    public class QueueController : ControllerBase
    {
      
        QueueRepo _queue = new QueueRepo();
        protected readonly IHubContext<NotifyHub> _notifyhub;
        public QueueController([NotNull] IHubContext<NotifyHub> notifyhub)
        {
            _notifyhub = notifyhub;
        }
        [HttpPost]
        [Route("api/queue/waitinglist")]
        public ActionResult waitinglist(Queue.waiting waitings)
        {
            return Ok(_queue.waiting(waitings));
        }
        [HttpPost]
        [Route("api/queue/reception_waitinglist")]
        public ActionResult reception_waitinglist()
        {
            return Ok(_queue.Reception_Waiting());
        }
        [HttpPost]
        [Route("api/queue/reception_lastqueueno")]
        public ActionResult reception_lastqueueno()
        {
            return Ok(_queue.Reception_lastqueueno());
        }
        [HttpPost]
        [Route("api/queue/notification")]
        public async Task<IActionResult> SendMessage(mdlNotifications.NotificationPost notificationPost)
        {
            await _notifyhub.Clients.All.SendAsync("notifytoreact", notificationPost);
            return Ok();
        }
        [HttpPost]
        [Route("api/queue/getcounterlist")]
        public ActionResult getcounterlist(Queue.getlobbynos counterlist)
        {
            return Ok(_queue.getcounterlist(counterlist));
        }
        [HttpPost]
        [Route("api/queue/getmaxnumber")]
        public ActionResult getmaxnumber(Queue.getMaxNUmber maxNUmber)
        {
            return Ok(_queue.getmaxnumber(maxNUmber));
        }
        [HttpPost]
        [Route("api/queue/getqueuemain")]
        public ActionResult getqueuemain()
        {
            return Ok(_queue.getqueuemain());
        }
        [HttpPost]
        [Route("api/queue/getqueuemainsenior")]
        public ActionResult getqueuemainsenior()
        {
            return Ok(_queue.getqueuemainsenior());
        }
        [HttpPost]
        [Route("api/queue/generatequeuenumber")]
        public ActionResult generatequeuenumber(Queue.generatenumber generatenumber)
        {
            return Ok(_queue.generatequeuenumber(generatenumber));
        }
        [HttpPost]
        [Route("api/queue/lastqueueno")]
        public ActionResult lastqueueno(Queue.counterlist counterlist)
        {
            return Ok(_queue.lastqueueno(counterlist));
        }
        [HttpPost]
        [Route("api/queue/generatenumber")]
        public ActionResult generatenumber(Queue.generatecounternumber cntr)
        {
            return Ok(_queue.generatenumber(cntr));
        }      
        [HttpPost]
        [Route("api/queue/generatenumberwithoutpdf")]
        public ActionResult generatenumberwithoutpdf(Queue.generatecounternumber cntr)
        {
            return Ok(_queue.generatenumberwithoutpdf(cntr));
        }
        [HttpPost]
        [Route("api/queue/generatenumberkiosk")]
        public ActionResult generatenumberkiosk(Queue.generatecounternumber cntr)
        {
            return Ok(_queue.generatenumberkiosk(cntr));
        }
        [HttpPost]
        [Route("api/queue/getqueuemaintable")]
        public ActionResult getqueuemaintable(Queue.queues queues)
        {
            return Ok(_queue.getqueuemaintable(queues));
        }
        [HttpPost]
        [Route("api/queue/getqueuno")]
        public ActionResult getqueuno(Queue.getqueues getqueuno)
        {
            return Ok(_queue.getqueuno(getqueuno));
        }
        [HttpPost]
        [Route("api/queue/reception_getqueuno")]
        public ActionResult reception_getqueuno()
        {
            return Ok(_queue.reception_getqueuno());
        }
        [HttpPost]
        [Route("api/queue/getcounterexist")]
        public ActionResult getcounterexist(Queue.getqueues getqueuno)
        {
            return Ok(_queue.getcounterexist(getqueuno));
        }
        [HttpPost]
        [Route("api/queue/getcounterexistandupdate")]
        public ActionResult getcounterexistandupdate(Queue.updatequeues updatequeues)
        {
            return Ok(_queue.getcounterexistandupdate(updatequeues));
        }
        [HttpPost]
        [Route("api/queue/getcounters_table")]
        public ActionResult getcounters_table()
        {
            return Ok(_queue.getcounters_table());
        }
        [HttpPost]
        [Route("api/queue/generatecounterno")]
        public ActionResult generatecounterno(Queue.generatecounterno generateno)
        {
            return Ok(_queue.generatecounterno(generateno));
        }
        [HttpPost]
        [Route("api/queue/getcountertype")]
        public ActionResult getcountertype()
        {
            return Ok(_queue.getcountertype());
        }
        [HttpPost]
        [Route("api/queue/addlobby")]
        public ActionResult addlobby(Queue.addlobby add)
        {
            return Ok(_queue.addlobby(add));
        }
        [HttpPost]
        [Route("api/queue/lobbytable")]
        public ActionResult lobbytable()
        {
            return Ok(_queue.lobbytable());
        }
        [HttpPost]
        [Route("api/queue/getcounternumber")]
        public ActionResult getcounternumber(Queue.getcounterno counterno)
        {
            return Ok(_queue.getcounternumber(counterno));
        }
        [HttpPost]
        [Route("api/queue/addnewcounternumber")]
        public ActionResult addnewcounternumber(Queue.addnewcounternumber counter)
        {
            return Ok(_queue.addnewcounternumber(counter));
        }
        [HttpPost]
        [Route("api/queue/addlobbydtls")]
        public ActionResult addlobbydtls(Queue.addlobbydtls adddtls)
        {
            return Ok(_queue.addlobbydtls(adddtls));
        }
        [HttpPost]
        [Route("api/queue/getcountermaintable")]
        public ActionResult getcountermaintable()
        {
            return Ok(_queue.getcountermaintable());
        }
        [HttpPost]
        [Route("api/queue/nextclienttotext")]
        public ActionResult nextclienttotext(Queue.nextclienttotext next)
        {
            return Ok(_queue.nextclienttotext(next));
        }
        [HttpPost]
        [Route("api/queue/message")]
        public ActionResult message()
        {
            return Ok(_queue.message());
        }
        [HttpPost]
        [Route("api/queue/insertmessage")]
        public ActionResult insertmessage(Queue.insertmessage next)
        {
            return Ok(_queue.insertmessage(next));
        }
        [HttpPost]
        [Route("api/queue/updatequeue")]
        public ActionResult updatequeue(Queue.UpdateQueue updatequeue)
        {
            return Ok(_queue.updatequeue(updatequeue));
        }
        [HttpPost]
        [Route("api/queue/updatequeuephone")]
        public ActionResult updatequeuephone(Queue.updatephonenumber updatephone)
        {
            return Ok(_queue.updatequeuephone(updatephone));
        }
        [HttpPost]
        [Route("api/queue/updateservedqueue")]
        public ActionResult updateservedqueue(Queue.UpdateQueue counters)
        {
            return Ok(_queue.updateservedqueue(counters));
        }
        //[HttpPost]
        //[Route("api/queue/insertnewcounter")]
        //public ActionResult insertnewcounter(Queue.counter counters)
        //{
        //    return Ok(_queue.insertnewcounter(counters));
        //}   
        [HttpPost]
        [Route("api/queue/updatecounter")]
        public ActionResult updatecounter(Queue.UpdateQueue counters)
        {
            return Ok(_queue.updatequeue(counters));
        }
    }
}

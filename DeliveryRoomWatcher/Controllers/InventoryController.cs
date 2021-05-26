using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DeliveryRoomWatcher.Hubs;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        InventoryRepo _inventory = new InventoryRepo();
        protected readonly IHubContext<NotifyHub> _notifyhub;
        public InventoryController([NotNull] IHubContext<NotifyHub> notifyhub)
        {
            _notifyhub = notifyhub;
        }
        [HttpPost]
        [Route("api/inventory/notification")]
        public async Task<IActionResult> SendMessage(mdlNotifications.NotificationPost notificationPost)
        {
            await _notifyhub.Clients.All.SendAsync("notifytoreact", notificationPost);
            return Ok();
        }
        [HttpPost]
        [Route("api/inventory/InsertNewRequest")]
        public ActionResult InsertNewRequest(mdlRequestHeader requests)
        {
            return Ok(_inventory.InsertNewRequest(requests));
        } 
        [HttpPost]
        [Route("api/inventory/getlistofrequest")]
        public ActionResult getlistofrequest(InventoryModel.listofrequest listofrequest)
        {
            return Ok(_inventory.getlistofrequest(listofrequest));
        }   
        [HttpPost]
        [Route("api/inventory/getlistofrequestsearched")]
        public ActionResult getlistofrequestsearched(InventoryModel.listofrequestseareched listofrequest)
        {
            return Ok(_inventory.getlistofrequestsearched(listofrequest));
        }     
        [HttpPost]
        [Route("api/inventory/getlistofrequestbystatus")]
        public ActionResult getlistofrequestbystatus(InventoryModel.listofrequestbydept listofbydept)
        {
            return Ok(_inventory.getlistofrequestbystatus(listofbydept));
        }  
        [HttpPost]
        [Route("api/inventory/getdepartmentList")]
        public ActionResult getdepartmentList()
        {
            return Ok(_inventory.getdepartmentList());
        }

        [HttpPost]
        [Route("api/inventory/getinventoryitem")]
        public ActionResult getinventoryitem()
        {
            return Ok(_inventory.getinventoryitem());
        }


        [HttpPost]
        [Route("api/inventory/getlistofreqyestApproved")]
        public ActionResult getlistofreqyestApproved(InventoryModel.listofrequest listofrequest )
        {
            return Ok(_inventory.getlistofreqyestApproved(listofrequest));
        }

        [HttpPost]
        [Route("api/inventory/getlistofreqyestCancelled")]
        public ActionResult getlistofreqyestCancelled(InventoryModel.listofrequest listofrequest)
        {
            return Ok(_inventory.getlistofreqyestCancelled(listofrequest));
        }

        [HttpPost]
        [Route("api/inventory/getlistofreqyestForApproval")]
        public ActionResult getlistofreqyestForApproval(InventoryModel.listofrequest listofrequest)
        {
            return Ok(_inventory.getlistofreqyestForApproval(listofrequest));
        }


        [HttpPost]
        [Route("api/inventory/getlistofreqyestIssued")]
        public ActionResult getlistofreqyestIssued(InventoryModel.listofrequest listofrequest)
        {
            return Ok(_inventory.getlistofreqyestIssued(listofrequest));
        }

        [HttpPost]
        [Route("api/inventory/getmop")]
        public ActionResult getmop()
        {
            return Ok(_inventory.getmop());
        }

        [HttpPost]
        [Route("api/inventory/getnoninventoryitem")]
        public ActionResult getnoninventoryitem()
        {
            return Ok(_inventory.getnoninventoryitem());
        }  
        [HttpPost]
        [Route("api/inventory/getsinglerequestheader")]
        public ActionResult getsinglerequestheader(mdlSingleRequest singleRequest)
        {
            return Ok(_inventory.getsinglerequestheader(singleRequest));
        }   
        [HttpPost]
        [Route("api/inventory/getsinglerequestdtls")]
        public ActionResult getsinglerequestdtls(mdlSingleRequest singleRequest)
        {
            return Ok(_inventory.getsinglerequestdtls(singleRequest));
        }
        [HttpPost]
        [Route("api/inventory/updaterequestApproved")]
        public ActionResult updaterequestApproved(mdlSingleRequest.SingleRequestApprove singleRequest)
        {
            return Ok(_inventory.updaterequestApproved(singleRequest));
        }
        [HttpPost]
        [Route("api/inventory/updaterequestCancelled")]
        public ActionResult updaterequestCancelled(mdlSingleRequest.SingleRequestCancelled singleRequest)
        {
            return Ok(_inventory.updaterequestCancelled(singleRequest));
        }   
        [HttpPost]
        [Route("api/inventory/dashboardnumbers")]
        public ActionResult dashboardnumbers(InventoryModel.listofrequest listofrequest)
        {
            return Ok(_inventory.dashboardnumbers(listofrequest));
        }


    }
}

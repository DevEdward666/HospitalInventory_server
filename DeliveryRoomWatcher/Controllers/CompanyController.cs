using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        CompanyRepository _company = new CompanyRepository();


        [HttpGet]
        [Route("")]
        public string defaultRoute()
        {
            return "running at 4020";
        }

        [HttpPost]
        [Route("api/company/company-name")]
        public ResponseModel CompanyName()
        {
            return _company.CompanyName();
        }


        [HttpPost]
        [Route("api/company/company-logo")]
        public ResponseModel CompanyLogo()
        {
            return _company.CompanyLogo();
        }

        [HttpPost]
        [Route("api/company/company-tagline")]
        public ResponseModel CompanyTagLine()
        {
            return _company.CompanyTagLine();
        }

    }
}

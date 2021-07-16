using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        CompanyRepository _company = new CompanyRepository();

        [HttpPost]
        [Route("api/company/companyname")]
        public ActionResult companyName()
        {
            return Ok(_company.CompanyName());
        }

        [HttpPost]
        [Route("api/company/companylogo")]
        public ActionResult companyLogo()
        {
            return Ok(_company.CompanyLogo());
        }

        [HttpPost]
        [Route("api/company/companytagline")]
        public ActionResult companyTagline()
        {
            return Ok(_company.CompanyTagLine());
        }

    }
}

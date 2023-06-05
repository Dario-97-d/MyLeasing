using Microsoft.AspNetCore.Mvc;
using MyLeasing.Web.Data.Repository;

namespace MyLeasing.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : Controller
    {
        readonly IOwnerRepository _ownerRepository;

        public OwnersController(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_ownerRepository.GetAllWithUsers());
        }
    }
}

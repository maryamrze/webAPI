using A2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using A2.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace A2.Controllers
{
    [Route("webapi")]
    [ApiController]
    public class A2Controller : Controller
    {
        private readonly IA2Repo _repository;
        public A2Controller(IA2Repo repository)
        {
            _repository = repository;
        }

        //Endpoint 1
        [HttpPost("Register")]
        public ActionResult Register(Users u)
        {
            var addedUser = _repository.Register(u);
            if (addedUser == null)
            {
                return Ok("UserName " + u.UserName + " is not available.");
            }
            else
            {
                return Ok("User successfully registered.");
            }
        }

        //Endpoint 2
        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpGet("PurchaseSign/{id}")]
        public ActionResult<PurchaseOutput> PurchaseSign(string id)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim claim = ci.FindFirst(ClaimTypes.Name);
            string userName = claim.Value.ToString();
            Users u = _repository.GetUserByName(userName);
            var signResult = _repository.PurchaseSign(id);
            if (ci == null)
            {
                return Unauthorized();
            }
            if (claim == null)
            {
                return Unauthorized();
            }
            if (u == null)
            {
                return Forbid();
            }
            if (signResult == null)
            {
                return BadRequest("Sign " + id + " not found");

            }
            PurchaseOutput purchaseOutput = new PurchaseOutput { UserName = userName, signID = id };
            return Ok(purchaseOutput);
        }

        //Endpoint 3
        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "OrganizersOnly")]
        [HttpPost("AddEvent")]
        public ActionResult AddEvent(EventInput EI)
        {
            if (!DateTime.TryParseExact(EI.Start, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime startDate) && !DateTime.TryParseExact(EI.End, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime endDate))
            {
                return BadRequest("The format of Start and End should be yyyyMMddTHHmmssZ.");
            }
            else if (!DateTime.TryParseExact(EI.Start, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out _))
            {
                return BadRequest("The format of Start should be yyyyMMddTHHmmssZ.");
            }
            else if (!DateTime.TryParseExact(EI.End, "yyyyMMddTHHmmssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out _))
            {
                return BadRequest("The format of End should be yyyyMMddTHHmmssZ.");
            }
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim claim = ci.FindFirst(ClaimTypes.Name);
            string userName = claim.Value.ToString();
            Organizers o = _repository.GetOrganizerByName(userName);
            if (ci == null)
            {
                return Unauthorized();
            }
            if (claim == null)
            {
                return Unauthorized();
            }
            if (o != null)
            {
                Events evnt = new Events { Start = EI.Start.ToString(), End = EI.End.ToString(), Summary = EI.Summary, Description = EI.Description, Location = EI.Location };
                _repository.AddEvent(evnt);
                return Ok("Success");
            }
            else
            {
                return Forbid();
            }
        }

        //Endpoint 4
        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "AuthOnly")]
        [HttpGet("EventCount")]
        public ActionResult<int> CountEvent()
        {
            var count = _repository.EventCount();
            return Ok(count);
        }

        //Endpoint 5
        [Authorize(AuthenticationSchemes = "MyAuthentication")]
        [Authorize(Policy = "AuthOnly")]
        [HttpGet("Event/{id}")]
        public ActionResult Event(int id)
        {
            Events e = _repository.GetEventByID(id);
            if (e == null)
            {
                return BadRequest("Event " + id + " does not exist.");
            }
            Response.Headers.Add("Content-Type", "text/calendar");
            Response.Headers.Add("Encoding", "charset=utf-8");

            return Ok(e);

        }
    }
}
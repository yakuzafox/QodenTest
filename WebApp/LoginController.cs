using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }

        [HttpPost("sign-in/{userName}")] 
        public async Task<ActionResult> Login(string userName)
        {
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.ExternalId),
                    new Claim(ClaimTypes.Role, account.Role)
                };
                ClaimsIdentity id = new ClaimsIdentity(claims, "Cookie");
                await HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(id));
     
                return Ok();
                //TODO 1: Solved
                //Generate auth cookie for user 'userName' with external id
            }
            else
            {
                return NotFound();
                //TODO 2: Solved 
                //return 404 if user not found
            }

        }
    }
}
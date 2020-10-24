﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace WebApp
{
    // TODO 4: Solved
    //unauthorized users should receive 401 status code
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize] 
        [HttpGet]
        public ValueTask<Account> Get()
        {
            string id = HttpContext.User?.FindFirst(ClaimTypes.Name).Value;
            return _accountService.LoadOrCreateAsync(id /* TODO 3: Solved */);
            //Get user id from cookie
        }

        //TODO 5: Solved
        //Endpoint should works only for users with "Admin" Role
        [Authorize(Policy = "Admin")]
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
        {
            return _accountService.GetFromCache(id);
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            //Update account in cache, don't bother saving to DB, this is not an objective of this task.
            var account = await Get();
            account.Counter++;
        }
    }
}
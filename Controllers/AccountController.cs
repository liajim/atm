using Microsoft.AspNetCore.Mvc;
using AtmCoroBain.Models;
using AtmCoroBain.Services;

namespace AtmCoroBain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _AccountService;

        public AccountController(IAccountService AccountService)
        {
            _AccountService = AccountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var Accounts = await _AccountService.GetAccountsAsync();

            if (Accounts == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Accounts in database");
            }

            return StatusCode(StatusCodes.Status200OK, Accounts);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAccount(int id)
        {
            Account? Account = await _AccountService.GetAccountAsync(id);

            if (Account == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Account found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, Account);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> AddAccount(Account Account)
        {
            var dbAccount = await _AccountService.AddAccountAsync(Account);

            if (dbAccount == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{Account.Id} could not be added.");
            }

            return CreatedAtAction("GetAccount", new { id = Account.Id }, Account);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateAccount(int id, Account Account)
        {
            if (id != Account.Id)
            {
                return BadRequest();
            }

            Account? dbAccount = await _AccountService.UpdateAccountAsync(Account);

            if (dbAccount == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{Account.Id} could not be updated");
            }

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var Account = await _AccountService.GetAccountAsync(id);
            if (Account == null)
            {
                return BadRequest("Transaction does not exists");
            }
            (bool status, string message) = await _AccountService.DeleteAccountAsync(Account);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, Account);
        }
    }
}
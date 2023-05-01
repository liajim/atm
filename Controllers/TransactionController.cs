using Microsoft.AspNetCore.Mvc;
using AtmCoroBain.Models;
using AtmCoroBain.Services;

namespace AtmCoroBain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _TransactionService;
        private readonly IAccountService _AccountService;

        public TransactionController(ITransactionService TransactionService, IAccountService AccountService)
        {
            _TransactionService = TransactionService;
            _AccountService = AccountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var Transactions = await _TransactionService.GetTransactionsAsync();

            if (Transactions == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Transactions in database");
            }

            return StatusCode(StatusCodes.Status200OK, Transactions);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            Transaction? Transaction = await _TransactionService.GetTransactionAsync(id);

            if (Transaction == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Transaction found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, Transaction);
        }

        [HttpPost("AddDeposit")]
        public async Task<ActionResult<Transaction>> AddDeposit(int AccountId, decimal amount)
        {
            var dbTransaction = await _TransactionService.AddDepositAsync(AccountId, amount);

            if (dbTransaction == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Deposit to account {AccountId} could not be added.");
            }

            return CreatedAtAction("GetTransaction", new { id = dbTransaction.Id }, dbTransaction);
        }

        [HttpPost("AddWithdraw")]
        public async Task<ActionResult<Transaction>> AddWithdraw(int AccountId, decimal amount)
        {
            var account = await _AccountService.GetAccountAsync(AccountId);
            if (account == null)
            {
                return BadRequest("Account does not exists");
            }

            if (account.Balance < amount)
            {
                return BadRequest("Account does not have enough money to do this operation");
            }

            var dbTransaction = await _TransactionService.AddWithdrawAsync(AccountId, amount);

            if (dbTransaction == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Withdraw to account {AccountId} could not be added.");
            }

            return CreatedAtAction("GetTransaction", new { id = dbTransaction.Id }, dbTransaction);
        }

        [HttpPost("AddTransfer")]
        public async Task<ActionResult<Transaction>> AddTransfer(int AccountId, int ToAccountId, decimal amount)
        {
            var fromAccount = await _AccountService.GetAccountAsync(AccountId);
            if (fromAccount == null)
            {
                return BadRequest("Account does not exists");
            }

            if (fromAccount.Balance < amount)
            {
                return BadRequest("Account does not have enough money to do this operation");
            }

            var dbTransaction = await _TransactionService.AddTransferAsync(AccountId, ToAccountId, amount);

            if (dbTransaction == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Transfer from account {AccountId} to {ToAccountId} could not be added.");
            }

            return CreatedAtAction("GetTransaction", new { id = dbTransaction.Id }, dbTransaction);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateTransaction(int id, decimal amount)
        {
            if (amount<=0)
                return BadRequest("Amount should be greater than 0");
            Transaction? dbTransaction = await _TransactionService.UpdateTransactionAsync(id, amount);

            if (dbTransaction == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Transaction {id} could not be updated");
            }

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var Transaction = await _TransactionService.GetTransactionAsync(id);
            if (Transaction == null)
            {
                return BadRequest("Transaction does not exists");
            }

            (bool status, string message) = await _TransactionService.DeleteTransactionAsync(Transaction);

            if (status == false)
            {
                return BadRequest(message);
            }

            return StatusCode(StatusCodes.Status200OK, Transaction);
        }
    }
}
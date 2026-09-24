using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Loans;
using LibraryManagement.API.Services;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDto dto)
        {
            try
            {
                var result = await _loanService.CreateLoanAsync(dto);
                return CreatedAtAction(nameof(GetLoanById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnLoan(int id)
        {
            var result = await _loanService.ReturnLoanAsync(id);
            if (result == null) return NotFound(new { message = "الاستعارة غير موجودة." });
            return Ok(result);
        }

        [HttpPut("{id}/renew")]
        public async Task<IActionResult> RenewLoan(int id)
        {
            try
            {
                var result = await _loanService.RenewLoanAsync(id);
                if (result == null) return NotFound(new { message = "الاستعارة غير موجودة." });
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoans()
        {
            var result = await _loanService.GetAllLoansAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanById(int id)
        {
            var result = await _loanService.GetLoanByIdAsync(id);
            if (result == null) return NotFound(new { message = "الاستعارة غير موجودة." });
            return Ok(result);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueLoans()
        {
            var result = await _loanService.GetOverdueLoansAsync();
            return Ok(result);
        }

        [HttpGet("~/api/members/{id}/loans/active")]
        public async Task<IActionResult> GetActiveLoansByMember(int id)
        {
            var result = await _loanService.GetActiveLoansByMemberAsync(id);
            return Ok(result);
        }
    }
}
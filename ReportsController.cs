using LibraryManagement.API.DTOs;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin,Librarian")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var result = await _reportService.GetDashboardSummaryAsync();
        return Ok(result);
    }

    [HttpGet("most-borrowed-books")]
    public async Task<IActionResult> GetMostBorrowedBooks([FromQuery] int top = 10)
    {
        var result = await _reportService.GetMostBorrowedBooksAsync(top);
        return Ok(result);
    }

    [HttpGet("most-active-members")]
    public async Task<IActionResult> GetMostActiveMembers([FromQuery] int top = 10)
    {
        var result = await _reportService.GetMostActiveMembersAsync(top);
        return Ok(result);
    }

    [HttpGet("overdue-loans")]
    public async Task<IActionResult> GetOverdueLoans()
    {
        var result = await _reportService.GetOverdueLoansAsync();
        return Ok(result);
    }

    [HttpGet("inventory-status")]
    public async Task<IActionResult> GetInventoryStatus()
    {
        var result = await _reportService.GetInventoryStatusAsync();
        return Ok(result);
    }

    [HttpGet("fines-summary")]
    public async Task<IActionResult> GetFinesSummary()
    {
        var result = await _reportService.GetFinesSummaryAsync();
        return Ok(result);
    }

    [HttpGet("reservations-summary")]
    public async Task<IActionResult> GetReservationsSummary()
    {
        var result = await _reportService.GetReservationsSummaryAsync();
        return Ok(result);
    }
}

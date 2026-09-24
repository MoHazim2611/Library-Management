using LibraryApi.Common;
using LibraryApi.Dtos;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

// [Authorize(Roles = "Admin,Librarian")]   // فعّلها لما الفريق يحدد الـ Roles
[Route("api/[controller]")]
public class FinesController : ApiControllerBase
{
    private readonly IFineService _service;
    public FinesController(IFineService service) => _service = service;

    /// GET api/fines?memberId=1&status=Unpaid
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? memberId, [FromQuery] string? status)
    {
        var r = await _service.GetAllAsync(memberId, status);
        return r.Success ? Ok(r.Data) : Fail(r);
    }

    /// GET api/fines/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var r = await _service.GetByIdAsync(id);
        return r.Success ? Ok(r.Data) : Fail(r);
    }

    /// GET api/fines/member/3/summary
    [HttpGet("member/{memberId:int}/summary")]
    public async Task<IActionResult> MemberSummary(int memberId)
    {
        var r = await _service.GetMemberSummaryAsync(memberId);
        return r.Success ? Ok(r.Data) : Fail(r);
    }

    /// POST api/fines
    [HttpPost]
    public async Task<IActionResult> Create(CreateFineDto dto)
    {
        var r = await _service.CreateAsync(dto);
        return r.Success
            ? CreatedAtAction(nameof(GetById), new { id = r.Data!.Id }, r.Data)
            : Fail(r);
    }

    /// PUT api/fines/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateFineDto dto)
    {
        var r = await _service.UpdateAsync(id, dto);
        return r.Success ? NoContent() : Fail(r);
    }

    /// DELETE api/fines/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var r = await _service.DeleteAsync(id);
        return r.Success ? NoContent() : Fail(r);
    }
}

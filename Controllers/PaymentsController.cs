using LibraryApi.Common;
using LibraryApi.Dtos;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

// [Authorize(Roles = "Admin,Librarian")]
[Route("api/[controller]")]
public class PaymentsController : ApiControllerBase
{
    private readonly IPaymentService _service;
    public PaymentsController(IPaymentService service) => _service = service;

    /// GET api/payments?fineId=2
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? fineId)
    {
        var r = await _service.GetAllAsync(fineId);
        return r.Success ? Ok(r.Data) : Fail(r);
    }

    /// GET api/payments/3
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var r = await _service.GetByIdAsync(id);
        return r.Success ? Ok(r.Data) : Fail(r);
    }

    /// POST api/payments
    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentDto dto)
    {
        var r = await _service.CreateAsync(dto);
        return r.Success
            ? CreatedAtAction(nameof(GetById), new { id = r.Data!.Id }, r.Data)
            : Fail(r);
    }
}

using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// بيحوّل فشل الـ Service لـ HTTP status مناسب (404 / 409 / 400)
    protected IActionResult Fail<T>(ServiceResult<T> r) => r.ErrorType switch
    {
        ErrorType.NotFound => NotFound(new { message = r.Error }),
        ErrorType.Conflict => Conflict(new { message = r.Error }),
        _ => BadRequest(new { message = r.Error })
    };
}

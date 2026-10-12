using App.Domain.Core.Techno_King.App.Domain.Core;
using App.Domain.Core.Techno_King.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Techno_King_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AccountController(IUserAppService userAppService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDTO model, CancellationToken cancellationToken)
        {
            model.CreatedByAdmin = false;


            var (identityResult, tokenData) = await userAppService.Register(model, cancellationToken);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(error.Code ?? string.Empty, error.Description);
                }
                return ValidationProblem(ModelState);
            }

            return Ok(new { message = "Registration completed successfully.", data = tokenData });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDTO model)
        {

            var tokenData = await userAppService.Login(model.Email, model.Password);

            if (tokenData is null)
            {
                return Problem(
                    title: "Login failed",
                    detail: "Email or password is incorrect.",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            return Ok(tokenData);
        }
    }
}
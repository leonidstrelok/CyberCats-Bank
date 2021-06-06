using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Registration.Dtos;

namespace Registration.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender emailSender;

        public UserController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //TODO: Implementation logic for calling system queue
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var result = await signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // _logger.LogInformation("User logged in.");
                return Ok();
            }

            if (result.IsLockedOut)
            {
                // _logger.LogWarning("User account locked out.");
                return BadRequest("You are temporarily blocked, please contact your administrator");
            }

            return Ok();
        }

        [HttpPost("registrationUser")]
        public async Task<IActionResult> RegistrationUser([FromBody] RegistrationDto registration)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser {UserName = registration.UserName, Email = registration.Email};
                var result = await userManager.CreateAsync(user, registration.Password);
                if (result.Succeeded)
                {
                    // _logger.LogInformation("User created a new account with password.");

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new {area = "Identity", userId = user.Id, code = code},
                        protocol: Request.Scheme);

                    await emailSender.SendEmailAsync(registration.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Ok();
                }

                foreach (var error in result.Errors)
                {
                    return BadRequest("Invalid data entered");
                }
            }

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
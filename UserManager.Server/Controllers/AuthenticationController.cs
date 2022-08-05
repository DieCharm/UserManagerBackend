using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using UserManager.Auth.Authentication;
using UserManager.Auth.Models;

namespace UserManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController :
        ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly MailService _mailService;
        private readonly TokenService _tokenService;
        
        public AuthenticationController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            MailService mailService,
            TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _tokenService = tokenService;
        }
        
        /// <summary>
        /// Is called to create a new user and send confirmation email
        /// </summary>
        /// <param name="request">Register model with user info</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] Register request)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser()
                {
                    Email = request.EMail,
                    UserName = request.UserName
                };
                
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var param = new Dictionary<string, string>
                    {
                        {"token", token },
                        {"email", user.Email }
                    };
                    
                    var confirmationLink = QueryHelpers.AddQueryString(request.ClientAddress, param);
                    var message = $"Confirmation email link\n{confirmationLink}";

                    if (_mailService.Send(request.EMail, request.UserName, message))
                    {
                        return Ok();
                    }
                    
                    await _userManager.DeleteAsync(user);
                }
            }
            
            return BadRequest();
        }

        /// <summary>
        /// Confirms an email
        /// </summary>
        /// <param name="confirmation">Email and confirmation token</param>
        /// <returns>JWT token</returns>
        [HttpPost]
        [Route("confirm-email")]
        public async Task<ActionResult> ConfirmEMail([FromBody] ConfirmEmail confirmation)
        {
            var user = await _userManager.FindByEmailAsync(confirmation.Email);
            if (user == null) return BadRequest();
            
            var result = await _userManager.ConfirmEmailAsync(user, confirmation.Token);
            if (result.Succeeded && user.EmailConfirmed)
            {
                await _signInManager.SignInAsync(user, false);
                string jwt = _tokenService.GetToken(user);
                return new JsonResult(jwt);
            }

            return BadRequest();
        }

        /// <summary>
        /// Is called to sign in registered user
        /// </summary>
        /// <param name="request">Email and password</param>
        /// <returns>JWT token</returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] Login request)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(request.EMail);

                if (user != null)
                {
                    var result = await _signInManager
                        .PasswordSignInAsync(user, request.Password, false, false);

                    if (result.Succeeded)
                    {
                        string token = _tokenService.GetToken(user);
                        return new JsonResult(token);
                    }
                }
            }

            return new UnauthorizedResult();
        }
    }
}
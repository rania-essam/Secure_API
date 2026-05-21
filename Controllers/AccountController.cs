using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI_DAY3.DTOs.AccountDtos;
using WebAPI_DAY3.Models;

namespace WebAPI_DAY3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserManager<IdentityUser> _userManager;

        public SignInManager<IdentityUser> _signInManager;

        

  
        public AccountController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            var loginres = _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false).Result;

            if (loginres.Succeeded)
            {
                var user = _userManager.FindByNameAsync(loginDto.UserName).Result;
                #region Create Claims ( Data about user )
                var userdata = new List<Claim>();
                userdata.Add(new Claim("username", loginDto.UserName));
                userdata.Add(new Claim(ClaimTypes.MobilePhone, "01001266578"));
                var roles = _userManager.GetRolesAsync(user).Result;

                foreach(var item in roles)
                {
                    userdata.Add(new Claim(ClaimTypes.Role,item));
                }
                #endregion

                #region secretkey+signingCredentials

                string key = _configuration["JWT:key"];
                var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
                var signingcredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

                #endregion


                //Generate JWT Token


                /*
                 token 
                1 - header ( token type (Bearer) , Hashing algorithm)
                2 - payload  ( Cliams , expiration date )
                3 - signature (secret key)
                 */

                var token = new JwtSecurityToken(
                    claims: userdata,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signingcredentials

                    );

                // convert token from object to string
                var Token = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(Token);


            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
        }



        [HttpGet("/api/trytoken")]
        [Authorize]
        public IActionResult Getall()
        {
            return Ok("authorized user !");
        }

        [HttpPut]
        // USING USERMANAGER
        public IActionResult Register(AddEmployeeDto empdto)
        {
            if (empdto == null) return BadRequest();
            if (ModelState.IsValid)
            {
                Employee emp = new Employee()
                {
                    UserName = empdto.Username,
                    Email = empdto.Email,
                    PasswordHash = empdto.Password,
                    Age = empdto.age
                };
                var res = _userManager.CreateAsync(emp, empdto.Password).Result;
                if (res.Succeeded)
                {
                    var r2 = _userManager.AddToRoleAsync(emp, "employee").Result;
                    if (res.Succeeded) return Created();
                    else return BadRequest(r2.Errors);
                }
                else return BadRequest(res.Errors);
            }
            return Ok();
        }


        [HttpGet]
        [Authorize(Roles ="employee")]  // only role employee can access
        public IActionResult Getempbyid(int id)
        {
            return Ok("all emps ");
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApp.Models;

namespace WebApp.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration configuration;

    public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        this.userManager = userManager;
        this.configuration = configuration;
    }

    public IActionResult Register()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(NewUserModel model)
    {
        if (!ModelState.IsValid)
        {
            // Вкажіть ім'я файлу представлення "Register"
            return View("Register", model);
        }

        var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Register", model);
        }
    }

    public IActionResult Login()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser(NewUserModel userModel)
    {
        IdentityUser user = await this.userManager.FindByNameAsync(userModel.UserName);
        if (user == null || !await this.userManager.CheckPasswordAsync(user, userModel.Password))
        {
            return this.Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        JwtSecurityToken jwt = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:IssuerSigningKey"])),
            SecurityAlgorithms.HmacSha256));

        string token = new JwtSecurityTokenHandler().WriteToken(jwt);

        this.Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(10),
        });

        return this.RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult Logout()
    {
        this.Response.Cookies.Delete("jwt");

        return this.RedirectToAction("Index", "Home");
    }
}

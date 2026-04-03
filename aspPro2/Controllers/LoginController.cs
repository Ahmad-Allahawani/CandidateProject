
using aspPro2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace aspPro2.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult signUp()
        {
            return View();
        }

        [HttpPost, ActionName("Sign In")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> SignInCONFIRM(string email, String password)
        {
            var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.Email == email);
            if (email != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.password, password);

                if (result == PasswordVerificationResult.Success)
                {
                    Response.Cookies.Append("UserEmail", email, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true

                    });
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return BadRequest(new { message = "Wrong Email or Password." });
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("sign up")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> signUpForm(User person, String Email, string password)
        {

            bool emailNotExists = !_context.Users.Any(u => u.Email == Email);
                    

            if (Email != null && emailNotExists)
            {
                person.password = _passwordHasher.HashPassword(person, password);

                _context.Users.Add(person);
                await _context.SaveChangesAsync();

                Response.Cookies.Append("UserEmail", Email, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true
                    
                });
                return RedirectToAction("Profile", "login", new { id = person.Id, email = Email });
            }
            return BadRequest(new { message = "User with this email already exists." });
        }



        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            
            var email = Request.Cookies["UserEmail"];

            if (email != null )
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == email);
                if (user != null)
                {
                    return View(user);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction(nameof(signUp));
            }
        }







        [HttpPost]
        
        public async Task<IActionResult> UpdateProfile(User users)
        {
            
            var email = Request.Cookies["UserEmail"];

            
            if (email != null)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == email);
                if (user != null)
                {
                    user.Name = users.Name;
                    user.LastName = users.LastName;
                    user.Email = users.Email;
                    if (!string.IsNullOrEmpty(users.password))
                    {
                        user.password = _passwordHasher.HashPassword(user, users.password);
                    }



                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return View(user);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }

        }



        public IActionResult logout()
        {
			Response.Cookies.Delete("UserPass");
			Response.Cookies.Delete("UserEmail");


			return RedirectToAction(nameof(signUp));
        }
    }
}


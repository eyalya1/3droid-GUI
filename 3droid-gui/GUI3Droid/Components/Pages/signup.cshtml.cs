using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using _3droidUser;

namespace GUI3Droid.Components.Pages
{
    public class signup : PageModel
    {
        private readonly ILogger<signup> _logger;

        public signup(ILogger<signup> logger)
        {
            _logger = logger;
        }

        public void OnPost(string username,string email, string password)
        {
            try
            {

                if (userLogin.SignUp(username: username, email: email, password: password))
                {
                    HttpContext.Response.Cookies.Append("user login",userLogin.hash(username+password));
                    Response.Redirect("/Home");
                }
                else
                {
                    Response.Redirect("signup");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Response.Redirect("/signup");
            }
        }
        //
        // public void sumbitLogin(string username,string email, string password)
        // {
        //
        // }
        public void OnGet()
        {
        }
    }
}
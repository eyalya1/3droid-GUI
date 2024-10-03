using Microsoft.AspNetCore.Mvc.RazorPages;
using _3droidUser;
using Microsoft.Net.Http.Headers;

namespace GUI3Droid.Components.Pages;

public class Login : PageModel
{
    public void OnPost(string username, string password)
    {
        if (userLogin.Login(username, password))
        {
            HttpContext.Response.Cookies.Append("user login",userLogin.hash(username+password));
            Response.Redirect("/Home");
        }
        else
        {
            Response.Redirect("/Login");
        }
    }
}
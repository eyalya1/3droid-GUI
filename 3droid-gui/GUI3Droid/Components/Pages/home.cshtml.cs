using System.Reflection.Metadata;
using _3droidUser;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GUI3Droid.Components.Pages;

public class home : PageModel
{
    public static string cookies { get; private set; }
    public static string[] Users { get; private set; }
    
    public void OnGet()
    {
        cookies="user hash= "+ Request.Cookies["user login"];
        // cookies += cookieValue; 
        Users = DBContext.GetUsers();
    }
}
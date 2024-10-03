using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace _3droidUser;

public class userLogin
{
    private SqliteConnection conn = new SqliteConnection("Data Source=test.db");
    
    public static bool userSignUp(string username,string email, string password)
    {
        Console.WriteLine($"got user sign up with args {username}, {email}, {password}");
        if (DBContext.GetUsers().Contains(username))
        {
            throw new Exception("Error: username already in use");
        }
        /*
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        string savedPasswordHash = Convert.ToBase64String(hashBytes);
        Console.WriteLine("sent sign up to dbcontext");
        */
        string savedPasswordHash = hash(password);
        string cookieHash = hash(username+password);
        DBContext.AddUser(username: username, email: email, password: savedPasswordHash,cookieHash: cookieHash);
        return true;
    }
    public static string hash(string password)
    {
        byte[] salt = new byte[16];
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        string savedPasswordHash = Convert.ToBase64String(hashBytes);
        Console.WriteLine("sent sign up to dbcontext");
        // dbcontext.AddUser(username: username, email: email, password: savedPasswordHash);
        // DBContext.AddUser(username: username, email: email, password: savedPasswordHash);
        return savedPasswordHash;
    }
    public static bool Login(string username, string password)
    {
        /* Fetch the stored value */

        var reader = DBContext.GetUser(username);
        if (!reader.Read())
        {
            Console.WriteLine("no such user");
            return false;
        }
            string savedPasswordHash = reader.GetString(1);
        // string savedPasswordHash = DBContext.GetUser(u => u.UserName == user).Password;
// /* Extract the bytes */
        byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
/* Get the salt */
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);
/* Compute the hash on the password the user entered */
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);
/* Compare the results */
        for (int i=0; i < 20; i++)
            if (hashBytes[i + 16] != hash[i])
                return false;
        return true;
    }
    public static bool SignUp(string username, string email, string password)
    {
        Regex emailValidator = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        if (!emailValidator.Match(email).Success)
        {
            // throw new ErrorEventArgs("email must follow email standarts");
            throw new Exception("email must follow email standarts");
        }
        if(!(password.Any(ch => ! char.IsLetterOrDigit(ch))))
        {
            throw new Exception("password must contain a special character");
        }
        if(!password.Any(c => char.IsLower(c)))
        {
            throw new Exception("password must contain a lowercase letter");
        }

        if (!password.Any(c => char.IsUpper(c)))
        {
            throw new Exception("password must contain an uppercase letter");
        }

        if (!password.Any(c => char.IsDigit(c)))
        {
            throw new Exception("password must contain a number");

        }

        return userLogin.userSignUp(username: username, email: email, password: password);
    }
}
    
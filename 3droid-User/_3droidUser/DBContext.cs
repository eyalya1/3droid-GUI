using Microsoft.Data.Sqlite;

namespace _3droidUser;

public class DBContext
{
    private static SqliteConnection conn = new SqliteConnection("Data Source=test.db");

    public static void AddUser(string username, string password,string email,string cookieHash)
    {
        Console.WriteLine($"dbcontext got adduser with args {username}, {email}, {password}");
        conn.Open();
        var sql = "INSERT INTO users(username,email,password,cookieHash) "+ " VALUES(@username,@email,@password,@cookieHash)";
        try
        {
        using var command = new SqliteCommand(sql, conn);
        command.Parameters.AddWithValue("@username",username);
        command.Parameters.AddWithValue("@email",email);
        command.Parameters.AddWithValue("@password",password);
        command.Parameters.AddWithValue("@cookieHash",cookieHash);

        var rowInserted = command.ExecuteNonQuery();
        Console.WriteLine($"the user {username}, {email},{password}, wass created successfully");
        }
        catch (SqliteException EX)
        {
            Console.WriteLine(EX);
        }
        conn.Close();
    }

    public static SqliteDataReader GetUser(string username)
    {
        conn.Open();
        var command = conn.CreateCommand();
        command.CommandText = @"SELECT username, password FROM users WHERE username = $username";
        command.Parameters.AddWithValue("$username", username);
        var reader = command.ExecuteReader();
        return reader;
    }
    public static SqliteDataReader GetUserPassword(string username)
    {
        conn.Open();
        var command = conn.CreateCommand();
        command.CommandText = @"SELECT password FROM users WHERE username = $username";
        command.Parameters.AddWithValue("$username", username);
        var reader = command.ExecuteReader();
        return reader;
    }

    public static string[] GetUsers()
    {
        conn.Open();
        var command = conn.CreateCommand();
        command.CommandText = @"SELECT username FROM users";
        var reader = command.ExecuteReader();
        List<string> result = new List<string>();
        string currUser;
        while (reader.Read())
        {
            result.Add(reader.GetString(0));
        }

        string[] users = result.ToArray();
        return users;
    }

    public static void DeleteUser(string username)
    {
        Console.WriteLine($"dbcontext got deluser with args {username}");
        conn.Open();
        var sql = "DELETE FROM users "+ " WHERE username= @username";
        try
        {
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@username",username);
            var rowInserted = command.ExecuteNonQuery();
            Console.WriteLine($"the user {username},wass deleted successfully");
        }
        catch (SqliteException EX)
        {
            Console.WriteLine(EX);
        }
        conn.Close();
    }

    
    public static void CreatUserTable()
    {
        conn.Open();
        var command = conn.CreateCommand();
        command.CommandText = @"CREATE TABLE users(username varchar(20),email varchar(100),password varchar(36),cookieHash varchar(36))";
        command.ExecuteScalar();
        conn.Close();
    }
    
}
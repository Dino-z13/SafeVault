using Microsoft.Data.Sqlite;

namespace SafeVault.Data;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateUser(string username, string email, string passwordHash, string role)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            @"INSERT INTO Users (Username, Email, PasswordHash, Role)
              VALUES ($username, $email, $passwordHash, $role);";

        cmd.Parameters.AddWithValue("$username", username);
        cmd.Parameters.AddWithValue("$email", email);
        cmd.Parameters.AddWithValue("$passwordHash", passwordHash);
        cmd.Parameters.AddWithValue("$role", role);

        cmd.ExecuteNonQuery();
    }

    public (int id, string username, string email, string passwordHash, string role)? GetByUsername(string username)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText =
            @"SELECT UserID, Username, Email, PasswordHash, Role
              FROM Users
              WHERE Username = $username
              LIMIT 1;";

        cmd.Parameters.AddWithValue("$username", username);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return (
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4)
        );
    }
}

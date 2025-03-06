using Dapper;
using EroSplorerX.Data.DTO;
using Microsoft.Data.Sqlite;

namespace EroSplorerX.Helpers;

public static class DatabaseHelper
{
    private static bool walEnabled = false;
    private const string CONNECTION_STRING = "Data Source=./Data/ESX.db";
    private static SqliteConnection GetConnection()
    {
        if (!walEnabled)
        {
            walEnabled = true;
            using var conn = GetConnection();
            conn.Open();
            conn.Execute("PRAGMA journal_mode=WAL;");
        }

        return new SqliteConnection(CONNECTION_STRING);
    }

    

    public static EsxFile? GetMetadataForPath(string fullPath)
    {
        using var conn = GetConnection();
        conn.Open();

        var sql = "SELECT * FROM Files WHERE Path = @Path";
        return conn.QueryFirstOrDefault<EsxFile>(sql, new { Path = fullPath });
    }

    public static long InsertFile(string name, string fullPath)
    {
        using var conn = GetConnection();
        conn.Open();

        var sql = "INSERT INTO Files (Title, Path) VALUES (@Title, @Path)";
        return conn.ExecuteScalar<long>(sql, new { Title = name, Path = fullPath });
    }

    public static string? GetTimeForId(long id)
    {
        using var conn = GetConnection();
        conn.Open();

        var sql = "SELECT Length FROM Files WHERE Id = @Id";
        return conn.ExecuteScalar<string>(sql, new { Id = id });
    }

    public static void InsertTimeForId(long id, string time)
    {
        using var conn = GetConnection();
        conn.Open();

        var sql = "UPDATE Files SET Length = @Length WHERE Id = @Id";
        conn.Execute(sql, new { Length = time, Id = id });
    }



}

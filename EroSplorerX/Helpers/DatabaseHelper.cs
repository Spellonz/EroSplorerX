using Dapper;
using EroSplorerX.Data.DTO;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    #region Collection methods

    public static IEnumerable<EsxCollection> GetCollections()
    {
        using var conn = GetConnection();
        conn.Open();

        var sql = "SELECT * FROM Collections";
        return conn.Query<EsxCollection>(sql);
    }

    public static void AddCollection(EsxCollection newCollection)
    {
        using var conn = GetConnection();
        conn.Open();
        var sql = "INSERT INTO Collections (Name, Path, ShowChildren) VALUES (@Name, @Path, 0)";
        conn.Execute(sql, new { Name = newCollection.Name, Path = newCollection.Path });
    }

    public static void RemoveCollection(long id)
    {
        using var conn = GetConnection();
        conn.Open();
        var sql = "DELETE FROM Collections WHERE Id = @Id";
        conn.Execute(sql, new { Id = id });
    }

    public static void UpdateCollection(EsxCollection collection)
    {
        using var conn = GetConnection();
        conn.Open();
        var sql = "UPDATE Collections SET Name = @Name, Path = @Path, ShowChildren = @ShowChildren WHERE Id = @Id";
        conn.Execute(sql, new { collection.Name, collection.Path, collection.ShowChildren, collection.Id });
    }

    public static List<EsxCollection> GetCollectionChildren(EsxCollection collection)
    {
        var children = Directory.GetDirectories(collection.Path).Select(m => new EsxCollection()
        {
            Name = Path.GetFileName(m),
            Path = m,
            Tag = $"{collection.Tag}/{Path.GetFileName(m)}",
        }).ToList();

        return children;
    }

    #endregion

    #region Metadata methods

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

    #endregion

}

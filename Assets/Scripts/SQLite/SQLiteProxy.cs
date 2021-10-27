using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

/// <summary>
/// Helper functions working with SQLite database files. 
/// </summary>
public static class SQLiteProxy
{
    public static void CreateDatabase(string fileName)
    {
        var connectionString = $"URI=file:{fileName}";
        SqliteConnection connection = null;
        try
        {
            if (!File.Exists(fileName))
            {
                // Keep file on disk after connection is closed.
                SqliteConnection.CreateFile(fileName); // luodaan tyhjä tietokanta tiedosto
            }
            // Testataan että yhteys tietokantaan toimii
            connection = new SqliteConnection(connectionString); // luo yhteys objekti
            connection.Open(); // avaa yhteys oikeasti ja näytä tulos alla
            Debug.Log($"SqliteConnection ver {connection.ServerVersion} state {connection.State} name {Path.GetFileNameWithoutExtension(fileName)}");
        }
        finally
        {
            connection?.Close();
        }
    }

    public static void DeleteDatabase(string fileName)
    {
        if (File.Exists(fileName)) // jos tietokanta tiedosto on olemassa
        {
            File.Delete(fileName); // niin se voidaan poistaa
        }
    }

    public static DataTable GetSchema(string fileName) // schema tarkoittaa tietokannan tietojen tai taulujen kuvausta
    {
        var connectionString = $"URI=file:{fileName}";
        SqliteConnection connection = null;
        try
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            Debug.Log($"SqliteConnection ver {connection.ServerVersion} state {connection.State} name {Path.GetFileNameWithoutExtension(fileName)}");
            return connection.GetSchema("Tables"); // haetaan tiedot kaikista tietokannan tauluista
        }
        finally
        {
            connection?.Close();
        }
    }

    /// <summary>
    /// Execute any callback action using SQLLite connection.
    /// </summary>
    /// <param name="fileName">the sqlite database filename</param>
    /// <param name="action">the callback action to execute</param>
    public static void ExecuteAction(string fileName, Action<SqliteConnection> action) //Action c#:ssa tarkoittaa funkitiota, joka ei palauta mtn. eli void.
    {
        var connectionString = $"URI=file:{fileName}";
        SqliteConnection connection = null;
        try
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            // action?.Invoke(connection); // lyhyt tapa
            if (action != null)
            {
                action(connection); // suorita callback metodi annetulla parametrillä
            }
        }
        catch (Exception x)
        {
            Debug.LogWarning($"{x.GetType().Name} code: {x.HResult} message: {x.Message}");
            throw;
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

/// <summary>
/// SQLite singelton helper for common database related commands.
/// </summary>
/// <remarks>
/// Kaikki SQLite k‰skyt l‰htet‰‰n SQLiteProxy objektille suoritettavaksi.
/// </remarks>
public class SQLiteManager : MonoBehaviour
{
    private const string DATABASE_NAME = "leaderboard.sqlite";

    public static SQLiteManager Instance; // (UNITY) standardi singleton pattern

    public bool databaseExists;
    public string databaseName;

    private bool isDatabaseCreated => File.Exists(databaseName); // n‰ytt‰‰ muuttujalta mutta on oikeasti funktio! kysyy onko kyseinen tiedosto olemassa vai ei.

    private void Awake()
    {
        if (Instance == null) // jos singleton puuttuu
        {
            Instance = this; // me olemme singleton
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) // jos me olemme singleton
        {
            Instance = null; // poistamme itsemme
        } 
    }

    private void dumpSchema() // n‰yt‰ tietokannan kaikki taulut
    {
        if (databaseExists)
        {
            var schema = SQLiteProxy.GetSchema(databaseName); // hae schema
            Debug.Log(schema.TableName + " count " + schema.Rows.Count); // n‰yt‰ montako rivi‰ schema (taulujen luettelo) sis‰lt‰‰
            foreach (DataRow row in schema.Rows) // hae scheman rivit yksitellen
            {
                Debug.Log(string.Join(", ", row.ItemArray)); // n‰yt‰ rivin kaikki tiedot pilkulla eroteltuna listana
            }
        }
    }

    private void Start()
    {
        // aseta tietokannan nimi, katso lˆytyykˆ se ja dumppaa se
        databaseName = Application.dataPath + Path.AltDirectorySeparatorChar + DATABASE_NAME;
        databaseExists = isDatabaseCreated;
        if (!databaseExists) //jos tietokantaa ei ole niin t‰m‰ luo uuden tietokannan ja leaderboard taulun.
        {
            CreateDatabase();
            CreateTable();
        }
        dumpSchema();
    }

    public void CreateDatabase()
    {
        // luo tietokanta jos se ei ole olemassa
        Debug.Log("CreateDatabase " + databaseName);
        if (!isDatabaseCreated)
        {
            SQLiteProxy.CreateDatabase(databaseName); // luo se
            databaseExists = isDatabaseCreated; // tarkista onko olemassa
        }
    }

    public void DeleteDatabase()
    {
        // poista tietokanta jos se on olemassa
        Debug.Log("DeleteDatabase " + databaseName);
        if (isDatabaseCreated)
        {
            SQLiteProxy.DeleteDatabase(databaseName); // poista se
            databaseExists = isDatabaseCreated; // tarkista onko olemassa
        }
    }

    public void CreateTable()
    {
        Debug.Log("CreateTable " + databaseName);
        if (isDatabaseCreated)
        {
            // luo uusi pˆyt‰ (tietokantatulu)
            SQLiteProxy.ExecuteAction(databaseName, (connection) =>
            {
                var command = connection.CreateCommand();
                var sql = "CREATE TABLE IF NOT EXISTS [leaderboard] (" + // taulun nimi
                    "[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," + // p‰‰avain (sql termi) eli yksik‰sitteinen rivin numero, tehd‰‰n automaattisesti
                    "[name] VARCHAR(255) NOT NULL," + // pelaajan nimi
                    "[timeSeconds] INTEGER NOT NULL" + // peliaika sekunneisssa koska se on helpointa tallettaa n‰in, usko minua
                    ")";
                command.CommandText = sql;
                Debug.Log("CreateTable sql=" + sql);
                var result = command.ExecuteScalar();
                Debug.Log("CreateTable result=" + result);
                dumpSchema();
            });
        }
    }

    public int InsertRow(string playerName, int timeSeconds)
    {
        Debug.Log("InsertRow " + databaseName);
        if (isDatabaseCreated)
        {
            return ExecuteInsert($"INSERT INTO [leaderboard] (name,timeSeconds) VALUES ('{playerName}',{timeSeconds})"); // list‰‰n uusi rivi leaderboard tauluun
        }
        return -1;
    }

    public void DeleteRow(int rowId)
    {
        Debug.Log("DeleteRow " + databaseName);
        if (isDatabaseCreated)
        {
            ExecuteNonQuery($"DELETE FROM [leaderboard] where id={rowId}"); // poista rivi jonka id an annettu
        }
    }

    public void DeleteAllRows()
    {
        Debug.Log("DeleteRow " + databaseName);
        if (isDatabaseCreated)
        {
            ExecuteNonQuery($"DELETE FROM [leaderboard]"); // poista kaiki rivit
        }
    }

    public int GetMaxId()
    {
        // Esimerkki
        // https://www.w3schools.com/sql/sql_min_max.asp
        var result = executeSelectSingle("SELECT MAX(id) FROM [leaderboard]"); // K‰ytet‰‰n SQL:n MAX() funtiota arvon hakemiseen id sarakkeessa.
        return int.Parse(result.ToString());
    }

    public LeaderboardRow QueryRow(int rowId)
    {
        Debug.Log($"QueryRow {rowId} : " + databaseName);
        if (isDatabaseCreated)
        {
            var sql = $"SELECT * FROM [leaderboard] WHERE id={rowId}";
            var sqlRow = executeSelectObject(sql);
            Debug.Log($"result: {string.Join(", ", sqlRow)}");
            if (sqlRow.Length > 0)
            {
                var id = Convert.ToInt32(sqlRow[0]);
                var name = sqlRow[1].ToString();
                var seconds = Convert.ToInt32(sqlRow[2]);
                var row = new LeaderboardRow(id, name, seconds);
                return row;
            }
        }
        return null;
    }

    public List<LeaderboardRow> QueryTable(int numberOfRows) // query = kysely
    {
        Debug.Log("QueryTable " + databaseName);
        var result = new List<LeaderboardRow>();
        if (isDatabaseCreated)
        {
            // lajittelu ASC (ascending) oletus ja DESC (descending) pit‰‰ sanoa jos haluaa laskevaan j‰rjestykseen
            // haetaan kaikki rivit ja lajitellaan ne peliajan ja nimen mukaan nousevaan j‰rjestykseen
            var sqlResult = executeSelect($"SELECT * FROM [leaderboard] ORDER BY 3,2 LIMIT {numberOfRows}");
            foreach (var sqlRow in sqlResult) // n‰yt‰ kaikki saadut rivit lokilla
            {
                //Debug.Log($"row: {string.Join(", ", sqlRow)}"); // n‰yt‰ rivin tiedot (COLUMN = sarake) debuggausta varten
                //Debug.Log(sqlRow[0].GetType().FullName);
                var id = Convert.ToInt32(sqlRow[0]);
                var name = sqlRow[1].ToString();
                var seconds = Convert.ToInt32(sqlRow[2]);
                var row = new LeaderboardRow(id, name, seconds);
                result.Add(row);
            }
        }
        return result;
    }

    public void DropTable()
    {
        Debug.Log("DropTable " + databaseName);
        if (isDatabaseCreated)
        {
            ExecuteNonQuery("DROP TABLE [leaderboard]"); // poista tualu tietokannasa
            dumpSchema(); // n‰yt‰ j‰ljell‰ olevat taulut
        }
    }

    // --- private implentation for convenience --

    private List<object[]> executeSelect(string sqlQuery)
    {
        var result = new List<object[]>(); // Palautetaan taulun rivit listana (List) taulukoita (object[])
        // Katso lis‰‰ anonymous function ja lambda expression: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
        SQLiteProxy.ExecuteAction(databaseName, (connection) =>
        {
            // T‰m‰ on normaali SQLite funktio SQL lauseen suorttamiseen.
            var command = connection.CreateCommand(); // Tarvitaan komnento-objekti
            command.CommandText = sqlQuery; // Annetaan sille SQL lause suoritettavaksi
            var reader = command.ExecuteReader(); // koska t‰m‰ on kysely, pyydet‰‰n lukija
            var columnCount = reader.FieldCount; // kysyt‰‰n montako saraketta vastauksesta lˆytyy
            while (reader.Read()) // niin kauan kuin on rivej‰ luettavana
            {
                var values = new object[columnCount]; // luo vastaus taulukko
                reader.GetValues(values); // hae rivin arvot vastaustaulukkoon
                result.Add(values); // lis‰‰ taulukko tuloslistaaa
            }
        });
        Debug.Log($"SQL: {sqlQuery} returns {result.Count} row(s).");
        return result; // palautetaan vastaus kun anonyymifuntio on suoritettu ja tuloslista on t‰ytetty
    }

    private object[] executeSelectObject(string sqlQuery)
    {
        object[] result = new object[0];
        SQLiteProxy.ExecuteAction(databaseName, (connection) =>
        {
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            var reader = command.ExecuteReader();
            var columnCount = reader.FieldCount;
            while (reader.Read())
            {
                result = new object[columnCount];
                reader.GetValues(result);
                break;
            }
        });
        Debug.Log($"SQL: {sqlQuery} returns {result.Length} columns(s).");
        return result;
    }

    private object executeSelectSingle(string sqlQuery)
    {
        object result = null; // Palautetaan vastaus objektina (tai se on NULL).
        SQLiteProxy.ExecuteAction(databaseName, (connection) =>
        {
            // T‰m‰ on normaali SQLite funktio SQL lauseen suorttamiseen joka palauttaa tasan yhden arvon.
            var command = connection.CreateCommand(); // Tarvitaan komnento-objekti
            command.CommandText = sqlQuery; // Annetaan sille SQL lause suoritettavaksi
            result = command.ExecuteScalar(); // koska vastaus on yksi asia niin se voidaan pyyt‰‰ suoraan
            Debug.Log($"SQL: {sqlQuery} RESULT {result}");
        });
        return result; // palauta vastauksen, mik‰ tahansa objetki
    }

    private int ExecuteNonQuery(string sqlCommand)
    {
        // SQL insert ja delete lauseet
        int result = -1; // alustetaan dummy arvoksi jotta tiedet‰‰n jos komentoa ei suoritettu lainkaan
        SQLiteProxy.ExecuteAction(databaseName, (connection) =>
        {
            // ExecuteNonQuery suorittaa SQL komennon ja palauttaa takaisin montako rivi‰ komento k‰sitteli
            // Insert kertoo montako rivi‰ lis‰ttiin 
            // Delete kertoo montako rivi‰ poistettiin
            var command = connection.CreateCommand(); // Tarvitaan komnento-objekti
            command.CommandText = sqlCommand; // Annetaan sille SQL lause (joka ei ole kysely) suoritettavaksi
            result = command.ExecuteNonQuery();
            Debug.Log($"SQL: {sqlCommand} RecordsAffected {result}");
        });
        return result; // vastaus voi olla nolla, jos komento ei lˆyt‰nyt yht‰‰n sopivaa rivi‰
    }

    private int ExecuteInsert(string sqlCommand)
    {
        int result = -1;
        SQLiteProxy.ExecuteAction(databaseName, (connection) =>
        {
            var command = connection.CreateCommand();
            command.CommandText = sqlCommand;
            var result1 = command.ExecuteNonQuery();
            var command2 = connection.CreateCommand();
            command2.CommandText = "SELECT last_insert_rowid()";
            var result2 = command2.ExecuteScalar();
            Debug.Log($"SQL: {sqlCommand} RecordsAffected {result1} lastId {result2}");
            if (result2 is long lastId)
            {
                result = (int)lastId;
            }
        });
        return result;
    }

}
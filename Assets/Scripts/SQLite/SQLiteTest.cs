using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SQLiteTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(tester());
    }

    private IEnumerator tester()
    {
        yield return null;
        var manager = SQLiteManager.Instance;

        var maxId = manager.GetMaxId();

        var create = false;
        if (create)
        {
            manager.CreateDatabase(); // luo tietokanta jos sitä ei ole olemassa
            manager.CreateTable(); // luo leaderboard taulu jos sitä ei ole olemassa
            yield return null;
        }

        var insert = false;
        if (insert)
        {
            manager.InsertRow("Jarkko", 247);
            manager.InsertRow("Veissi", 240);
            manager.InsertRow("Eetu", 260);
            manager.InsertRow("Heidi", 251);
            manager.InsertRow("Henri", 270);
            manager.InsertRow("Alex", 280);
            manager.InsertRow("Emmi", 244);
            yield return null;
        }

        var delete = false;
        if (delete)
        {
            manager.DeleteRow(1);
            yield return null;
        }
        var deleteAll = true;
        if (deleteAll)
        {
            manager.DeleteAllRows();
            yield return null;
        }

        var result = manager.QueryTable(10);
        int position = 0;
        foreach (var leaderboardRow in result) // näytä kaikki saadut LeaderboardRow rivit lokilla
        {
            Debug.Log($"pos {++position} {leaderboardRow} id={leaderboardRow.id}");
        }
    }
}

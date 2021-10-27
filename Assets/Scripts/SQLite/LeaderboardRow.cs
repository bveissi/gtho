using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardRow
{
    public readonly int id;
    public readonly string name;
    public readonly int seconds;
    public int rank { set; get; }

    public string displayResult
    {
        get
        {
            var minuteValue = seconds / 60; // ota minuutit jakolaskulla
            var secondValue = seconds % 60; // ota sekunnit modulo laskulla, modulo palauttaa jakolaskun jakojäännöksen
            return $"{minuteValue}:{secondValue:D2}";
        }
    }

    public LeaderboardRow(int id, string name, int seconds)
    {
        this.id = id;
        this.name = name;
        this.seconds = seconds;
    }

    public override string ToString()
    {
        return $"#{rank} {name} {displayResult}";
    }
}

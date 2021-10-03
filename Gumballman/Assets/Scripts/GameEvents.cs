using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event System.Action GameStartEvent;
    public static event System.Action<int> ScoreChangeEvent;
    public static event System.Action<int> HPChangeEvent;

    public static event System.Action GameLose;

    public static void CallScoreChangeEvent(int score)
    {
       ScoreChangeEvent?.Invoke(score);
    }
    public static void CallHpChangeEvent(int hp)
    {
        HPChangeEvent?.Invoke(hp);
    }
    public static void CallPlayerLose()
    {
        GameLose?.Invoke();
    }
}

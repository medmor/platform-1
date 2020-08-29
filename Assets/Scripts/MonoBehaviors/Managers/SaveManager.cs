﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Manager<SaveManager>
{

    public readonly string Level = "level";
    public readonly string BestScore = "bestScore";

    public bool IsBestScoreSaved()
        => PlayerPrefs.HasKey(BestScore);

    public void SetLevel(string value)
        => PlayerPrefs.SetString(Level, value);

    public string GetLevel()
        => PlayerPrefs.GetString(Level);

    public void SetBestScore(int value)
        => PlayerPrefs.SetInt(BestScore, value);

    public int GetBestScore()
        => PlayerPrefs.GetInt(BestScore);

    public void ResetSaving()
    {
        SetBestScore(0);
        SetLevel("Level1");
    }
}

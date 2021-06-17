using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public static float GameDifficulty = 1.0f;


    public int totalShotsFired;
    public int totalReloads;
    public int totalAmmoPickedUp;
    public int totalHealthLost;
    public int totalHealthPickedUp;
    public int totalKills;
    public float playTime;

    public static float MouseSensitivity = 90f;

    public string GetPlayerStats()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(playTime);
        string playTimeStr = string.Format("{0:D2} minutes, {1:D2} seconds", timeSpan.Minutes, timeSpan.Seconds);
        string playerStatsStr = string.Format("Total shots fired:{0}\n" +
                                       "Total reloads: {1}\n" +
                                       "Total ammo picked up: {2}\n" +
                                       "Total health lost: {3}\n" +
                                       "Total health picked up: {4}\n" +
                                       "Total kills: {5}\n" +
                                       "Play time: {6}", totalShotsFired, totalReloads, totalAmmoPickedUp, totalHealthLost, totalHealthPickedUp, totalKills, playTimeStr);
        return playerStatsStr;
    }

    public PlayerStats()
    {
        totalShotsFired = 0;
        totalReloads = 0;
        totalAmmoPickedUp = 0;
        totalHealthLost = 0;
        totalHealthPickedUp = 0;
        totalKills = 0;
        playTime = 0;
    }


    public static PlayerStats PlayerStatsSingleton
    {
        get
        {
            if (_playerStatsSingleton == null)
            {
                _playerStatsSingleton = new PlayerStats();
            }

            return _playerStatsSingleton;
        }
        set
        {
            _playerStatsSingleton = value;
        }
    }

    private static PlayerStats _playerStatsSingleton;
}

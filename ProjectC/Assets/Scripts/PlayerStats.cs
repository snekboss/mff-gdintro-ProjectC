using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public int totalShotsFired;
    public int totalReloads;
    public int totalAmmoPickedUp;
    public int totalHealthLost;
    public int totalHealthPickedUp;
    public int totalKills;
    public float playTime;

    public static float MouseSensitivity;

    public PlayerStats()
    {
        totalShotsFired = 0;
        totalReloads = 0;
        totalAmmoPickedUp = 0; // TODO
        totalHealthLost = 0;
        totalHealthPickedUp = 0; // TODO
        totalKills = 0; // TODO
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
    }

    private static PlayerStats _playerStatsSingleton;
}

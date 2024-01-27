using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "GGJ_Datas/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public enum EHumourType
    {
        DarkHumour,
        Imitations,
        Jokes,
    }

    public int NumberOfSlotsStart => _numberOfSlotsStart;
    [SerializeField]
    private int _numberOfSlotsStart = 3;

    public int MoneyStart => _moneyStart;
    [SerializeField]
    private int _moneyStart = 15;

    public int TotalRoundCount => _totalRoundCount;
    [SerializeField]
    private int _totalRoundCount = 7;


    public int MoneyUpgradeMin => _moneyUpgradeMin;
    public int MoneyUpgradeMax => _moneyUpgradeMax;
    [Header("Upgrades")]
    [SerializeField]
    private int _moneyUpgradeMin = 5;
    [SerializeField]
    private int _moneyUpgradeMax = 10;
}

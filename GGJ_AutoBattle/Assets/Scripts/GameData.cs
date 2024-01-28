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
    public int MoneyStart => _moneyStart;
    [Header("--- Initial Values ---")]
    [SerializeField]
    private int _numberOfSlotsStart = 3;
    [SerializeField]
    private int _moneyStart = 15;



    public int TotalRoundCount => _totalRoundCount;
    public int AddedMoneyPerTurn => _addedMoneyPerTurn;
    public List<string> ListCharactersNames => _listCharactersNames;
    [Header("--- Core ---")]
    [SerializeField]
    private int _totalRoundCount = 7;
    [SerializeField]
    private int _addedMoneyPerTurn = 15;
    [SerializeField]
    private List<string> _listCharactersNames;

    public float MultiplierPriceFromStats => _multiplierPriceFromStats;
    [Header("-- Shop")]
    [SerializeField]
    private float _multiplierPriceFromStats = 1;


    public int RerollPrice => _rerollPrice;
    public float StartFailChancePercentage => _startFailChancePercentage;
    public float FailChanceIncrement => _failChanceIncrement;
    [Header("- Reroll")]
    [SerializeField]
    private int _rerollPrice = 1;
    [SerializeField]
    private float _startFailChancePercentage = 40;
    [SerializeField]
    private float _failChanceIncrement = 20;



    public int MoneyUpgradeMin => _moneyUpgradeMin;
    public int MoneyUpgradeMax => _moneyUpgradeMax;

    [Header("--- Upgrades ---")]
    [SerializeField]
    private int _moneyUpgradeMin = 5;
    [SerializeField]
    private int _moneyUpgradeMax = 10;

}

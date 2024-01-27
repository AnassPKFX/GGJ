using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using static GameData;
using static GameManager;

public class GameManager : MonoBehaviour
{
    #region Data

    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public int NumberOfSlots => _numberOfSlots;
    private int _numberOfSlots;

    public int Money => _money;
    private int _money;
    public int RoundCount => _roundCount;
    private int _roundCount;

    public EGamePhase GamePhase => _gamePhase;
    private EGamePhase _gamePhase;

    public GameData GameData => _gameData;

    public Dictionary<EHumourType, HumourTypeData> HumourTypeDict { get => _humourTypeDict; set => _humourTypeDict = value; }
    private Dictionary<EHumourType, HumourTypeData> _humourTypeDict;

    [Header("Game Stats")]
    [SerializeField]
    GameData _gameData;

    private enum EUpgradeType
    {
        ADD_SLOTS,
        ADD_MONEY,
        ADD_STATS,
    }
    public enum EGamePhase
    {
        START,
        SHOP,
        FIGHT,
        UPGRADE,
        END,
    }
    #endregion

    void Awake()
    {
        _instance = this;
        InitGameStats();
        Debug.Log("<color=magenta> Round starting : " + ((int)_roundCount + 1) + "</color>");

    }

    private void InitGameStats()
    {
        _numberOfSlots = _gameData.NumberOfSlotsStart;
        _money = _gameData.MoneyStart;
        _roundCount = 0;
        _gamePhase = EGamePhase.START;
        List<HumourTypeData> humourTypeDatas = Resources.LoadAll<HumourTypeData>("Data/HumourTypeData").ToList();
        foreach(var htd in humourTypeDatas)
        {
            _humourTypeDict.Add(htd.HumourType, htd);
        }
    }

    public void UpgradeStatsRandom()
    {
        EUpgradeType upgradeType = (EUpgradeType)UnityEngine.Random.Range(0, 2);
        switch (upgradeType)
        {
            case EUpgradeType.ADD_SLOTS:
                Debug.Log("<color=yellow> Upgrade : ADD_SLOTS</color>");
                _numberOfSlots++;
                return;
            case EUpgradeType.ADD_MONEY:
                Debug.Log("<color=yellow> Upgrade : ADD_MONEY</color>");
                _money += UnityEngine.Random.Range(_gameData.MoneyUpgradeMin, _gameData.MoneyUpgradeMax);
                return;
            case EUpgradeType.ADD_STATS:
                Debug.Log("<color=yellow> Upgrade : ADD_STATS</color>");
                // TO DO
                return;
        }
    }

    public void NextPhase(bool lose)
    {
        if(lose)
            _gamePhase = EGamePhase.END;
        else
        {
            if (_gamePhase == EGamePhase.UPGRADE && _roundCount < _gameData.TotalRoundCount-1)
            {
                _gamePhase = EGamePhase.SHOP;
                _roundCount++;
                Debug.Log("<color=magenta> Round starting : " + ((int)_roundCount+1) + "</color>");
            }
            else
            {
                _gamePhase = (EGamePhase)(((int)_gamePhase+1) % 5);
            }
        }
        LaunchNextPhase(_gamePhase);
    }

    private void LaunchNextPhase(EGamePhase gamePhase)
    {
        Debug.Log("Phase Starting : " + _gamePhase.ToString());

        switch (gamePhase)
        {
            case EGamePhase.START:
                //Not supposed to happen
                return;
            case EGamePhase.SHOP:
                // TO DO
                return;
            case EGamePhase.FIGHT:
                // TO DO
                return;  
            case EGamePhase.UPGRADE:
                UpgradeStatsRandom();
                return;
            case EGamePhase.END:
                _roundCount = 0;
                Debug.Log("<color=cyan> GAME END </color>");

                NextPhase(false);
                return;
        }
    }

}

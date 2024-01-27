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

    public int Money { get => _money; set => _money = value; }
    private int _money;
    public int RoundCount => _roundCount;
    private int _roundCount;

    public EGamePhase GamePhase => _gamePhase;
    private EGamePhase _gamePhase;

    public CharacterTierData.ETier CurrentTier => _currentTier;
    private CharacterTierData.ETier _currentTier;
    public EnemyData.ETurn CurrentTurn => _currentTurn;
    private EnemyData.ETurn _currentTurn;

    public List<Joker> Team {get => _team; set=> _team = value; }
    private List<Joker> _team;

    public Movable CurrentDraggedMovable{get => _currentDraggedEntity; set=> _currentDraggedEntity = value; }
    private Movable _currentDraggedEntity;

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
        _currentTier = CharacterTierData.ETier.TIER_1;
        _currentTurn = EnemyData.ETurn.TURN_1;

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

                _currentTier = CharacterTierData.ETier.TIER_1;


            }
            else
            {
                _gamePhase = (EGamePhase)(((int)_gamePhase+1) % 5);
            }
            Debug.Log("<color=magenta> Round starting : " + ((int)_roundCount+1) + "</color>");
            
            _currentTurn = (EnemyData.ETurn)(((int)_currentTurn + 1) % 7);
            _roundCount++;
            switch (_currentTurn)
            {
                case EnemyData.ETurn.TURN_1:
                case EnemyData.ETurn.TURN_2:
                    _currentTier = CharacterTierData.ETier.TIER_1;
                    break;
                case EnemyData.ETurn.TURN_3:
                case EnemyData.ETurn.TURN_4:
                    _currentTier = CharacterTierData.ETier.TIER_2;
                    break;
                case EnemyData.ETurn.TURN_5:
                case EnemyData.ETurn.TURN_6:
                case EnemyData.ETurn.TURN_7:
                    _currentTier = CharacterTierData.ETier.TIER_3;
                    break;
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
                ShopManager.Instance.StartShopPhase();
                return;  
            case EGamePhase.UPGRADE:
                UpgradeStatsRandom();
                return;
            case EGamePhase.END:
                _roundCount = 0;
                _currentTurn = EnemyData.ETurn.TURN_1;

                Debug.Log("<color=cyan> GAME END </color>");

                NextPhase(false);
                return;
        }
    }

}

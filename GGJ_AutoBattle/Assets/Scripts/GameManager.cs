using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static GameData;
using static GameManager;
using static JokerStats;

public class GameManager : MonoBehaviour
{
    #region Data

    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public int NumberOfSlots => _numberOfSlots;
    private int _numberOfSlots;

    public int RoundCount => _roundCount;
    private int _roundCount;

    public EGamePhase GamePhase => _gamePhase;
    private EGamePhase _gamePhase;

    public CharacterTierData.ETier CurrentTier => _currentTier;
    private CharacterTierData.ETier _currentTier;
    public EnemyData.ETurn CurrentTurn => _currentTurn;
    private EnemyData.ETurn _currentTurn;

    public List<Joker> Team {get => _team; set=> _team = value; }
    private List<Joker> _team = new();

    public List<Enemy> EnemyTeam { get => _enemyTeam; set => _enemyTeam = value; }
    private List<Enemy> _enemyTeam = new();

    public Movable CurrentDraggedMovable{get => _currentDraggedEntity; set=> _currentDraggedEntity = value; }
    private Movable _currentDraggedEntity;

    public GameData GameData => _gameData;

    public Slot Trash => _trash;
    [SerializeField]
    private Slot _trash;
    [SerializeField]
    private GameObject _loseScreen;
    [SerializeField]

    private ShopManager sm;

    public List<HumourTypeData> HumourTypeDatas => _humourTypeDatas;
    List<HumourTypeData> _humourTypeDatas = new();

    public bool CanMoveEntities = true; 
    public Dictionary<EHumourType, HumourTypeData> HumourTypeDict { get => _humourTypeDict; set => _humourTypeDict = value; }
    private Dictionary<EHumourType, HumourTypeData> _humourTypeDict = new();

    [Header("Game Stats")]
    [SerializeField]
    GameData _gameData;


    public enum EUpgradeType
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
        _loseScreen.SetActive(false);


    }
    private void Start()
    {
        NextPhase(false);
        
    }

    private void InitGameStats()
    {
        _numberOfSlots = _gameData.NumberOfSlotsStart;
        _roundCount = 0;
        _gamePhase = EGamePhase.START;
        _currentTier = CharacterTierData.ETier.TIER_1;
        _currentTurn = EnemyData.ETurn.TURN_1;

        _humourTypeDatas = Resources.LoadAll<HumourTypeData>("Data/HumourTypeDatas").ToList();
        foreach(var htd in _humourTypeDatas)
        {
            Debug.Log(htd.DisplayName.ToString());
            _humourTypeDict.Add(htd.HumourType, htd);
        }
    }
    public void UpgradeSlots() => Upgrade(EUpgradeType.ADD_SLOTS);
    public void UpgradeMoney() => Upgrade(EUpgradeType.ADD_MONEY);
    public void UpgradeStats() => Upgrade(EUpgradeType.ADD_STATS);
    public void Upgrade(EUpgradeType upgradeType)
    {
        //EUpgradeType upgradeType = (EUpgradeType)UnityEngine.Random.Range(0, 2);
        StartCoroutine(UpgradeManager.Instance.StopUpgradePhase(upgradeType));
        switch (upgradeType)
        {
            case EUpgradeType.ADD_SLOTS:
                Debug.Log("<color=yellow> Upgrade : ADD_SLOTS</color>");
                _numberOfSlots++;
                return;
            case EUpgradeType.ADD_MONEY:
                Debug.Log("<color=yellow> Upgrade : ADD_MONEY</color>");
                sm.Money += UnityEngine.Random.Range(_gameData.MoneyUpgradeMin, _gameData.MoneyUpgradeMax);
                return;
            case EUpgradeType.ADD_STATS:
                Debug.Log("<color=yellow> Upgrade : ADD_STATS</color>");
                foreach(var j in Team)
                {
                    var newStats = new JokerStats(j.name);

                    for (int i = 0; i < newStats.HumourStats.Count; i++)
                    {
                        int newStat = j.JokerStats.HumourStats[i].Value;
                        newStats.HumourStats[i] = new(newStats.HumourStats[i].Key, newStat++);
                    }
                    j.InitStats(newStats);
                }
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
                CanMoveEntities = false;
                return;
            case EGamePhase.SHOP:
                CanMoveEntities = true;
                sm.StartShopPhase();
                return;
            case EGamePhase.FIGHT:
                CanMoveEntities = false;
                FightingManager.Instance.StartFight();
                return;  
            case EGamePhase.UPGRADE:
                CanMoveEntities = false;

                UpgradeManager.Instance.StartUpgradePhase();
                return;
            case EGamePhase.END:
                CanMoveEntities = false;

                _roundCount = 0;
                _currentTurn = EnemyData.ETurn.TURN_1;

                Debug.Log("<color=cyan> GAME END </color>");
                _loseScreen.SetActive(true);
                return;
        }
    }

    public void Retry()
    {

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}

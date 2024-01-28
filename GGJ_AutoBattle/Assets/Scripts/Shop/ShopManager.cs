using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject ShopScene;
    [SerializeField] Joker jokerPrefab;
    [SerializeField] List<Slot> _slots;
    [Header("UI")]
    [SerializeField]
    TMP_Text _moneyText;
    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            UpdateMoneyUI();
        }
    }
    [SerializeField]
    private int _money;

    public List<Joker> JokersOnSale
    {
        get => _jokersOnSale;
        set
        {
            _jokersOnSale = value;
        }
    }
    [SerializeField]
    private List<Joker> _jokersOnSale = new();

    public static ShopManager Instance => _instance;
    private static ShopManager _instance;

    private List<CharacterTierData> _tierStatsData;
    float _rerollFailChance;

    void Start()
    {
        _instance = this;
        ShopScene.SetActive(false);
        _tierStatsData = Resources.LoadAll<CharacterTierData>("Data/Characters/PlayerCharacters").ToList();
        Money = GameManager.Instance.GameData.MoneyStart;

    }

    public void StartShopPhase()
    {
        ShopScene.SetActive(true);
        _rerollFailChance = GameManager.Instance.GameData.StartFailChancePercentage;
        InitSlots(false);
    }
    public void StopShopPhase()
    {
        _jokersOnSale.ForEach(x => x.transform.GetComponent<Collider>().enabled = false);
        _jokersOnSale.ForEach(x => StartCoroutine(x.MoveToSlot(GameManager.Instance.Trash)));
        _jokersOnSale.Clear();
        ShopScene.SetActive(false);
        GameManager.Instance.NextPhase(false);
    }
    public void Reroll()
    {
        int randChance = Random.Range(0, 100);
        if(randChance > _rerollFailChance)
        {
            InitSlots(false);
        }
        else
        {
            InitSlots(true);
        }
        _rerollFailChance += GameManager.Instance.GameData.FailChanceIncrement;
    }
    private void UpdateMoneyUI() => _moneyText.text = _money.ToString();
    private void InitSlots(bool hasRerollFailed)
    {
        _jokersOnSale.ForEach(x => x.transform.GetComponent<Collider>().enabled = false);
        _jokersOnSale.ForEach(x => StartCoroutine(x.MoveToSlot(GameManager.Instance.Trash)));
        _jokersOnSale.Clear();
        CharacterTierData data = _tierStatsData.First(d => d.Tier == GameManager.Instance.CurrentTier);
        foreach(Slot slot in _slots)
        {
            var newJoker = Instantiate(jokerPrefab, slot.TpPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
            slot.IsOccupied = true;
            slot.SlotType = Slot.ESlotType.SHOP_SLOT;
            var newStats = new JokerStats(GameManager.Instance.GameData.ListCharactersNames[UnityEngine.Random.Range(0, GameManager.Instance.GameData.ListCharactersNames.Count())]);

            if (!hasRerollFailed)
            {
                int countPoint = 0;
                for(int i = 0; i< newStats.HumourStats.Count; i++)
                {
                    int randStats = Random.Range(0, data.MaxStatPerCategory);
                    if (countPoint + randStats > data.MaxStatPerCharacter)
                        randStats = Random.Range(0, data.MaxStatPerCategory- countPoint);

                    newStats.HumourStats[i] = new(newStats.HumourStats[i].Key, randStats);
                    countPoint += randStats;
                    //Debug.Log(randStats);
                }
            }
            newJoker.SlotBase = slot;
            newJoker.InitStats(newStats);
            _jokersOnSale.Add(newJoker);
        }
    }


}

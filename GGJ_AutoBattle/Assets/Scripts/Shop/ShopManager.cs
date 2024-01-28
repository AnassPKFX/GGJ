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

    public static ShopManager Instance => _instance;
    private static ShopManager _instance;

    private List<CharacterTierData> _tierStatsData;

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
        InitSlots();
    }
    public void StopShopPhase()
    {
        ShopScene.SetActive(false);
        GameManager.Instance.NextPhase(false);
    }
    private void UpdateMoneyUI() => _moneyText.text = _money.ToString();
    private void InitSlots()
    {
        CharacterTierData data = _tierStatsData.First(d => d.Tier == GameManager.Instance.CurrentTier);
        foreach(Slot slot in _slots)
        {
            var newJoker = Instantiate(jokerPrefab, slot.TpPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
            slot.IsOccupied = true;
            slot.SlotType = Slot.ESlotType.SHOP_SLOT;
            var newStats = new JokerStats(GameManager.Instance.GameData.ListCharactersNames[UnityEngine.Random.Range(0, GameManager.Instance.GameData.ListCharactersNames.Count())]);

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
            newJoker.SlotBase = slot;
            newJoker.InitStats(newStats);
        }
    }


}

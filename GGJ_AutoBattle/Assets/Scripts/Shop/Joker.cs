using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joker : Movable
{
    public enum EStatus
    {
        UNBOUGHT,
        TEAMMATE,
    }
    public EStatus Status => _status;
    private EStatus _status;

    public int Price => _price;
    private int _price;
    public TMP_Text PriceText => _priceText;
    [SerializeField]
    private TMP_Text _priceText;
    [SerializeField]
    private TMP_Text _darkHumour;
    [SerializeField]
    private TMP_Text _imitation;
    [SerializeField]
    private TMP_Text _jokes;


    private void Start()
    {
        
        _status = EStatus.UNBOUGHT;
    }
    public override void InitStats(JokerStats newStats)
    {
        base.InitStats(newStats);

        _price = (int)(_jokerStats.HumourStats.Select(d => d.Value).Sum() * GameManager.Instance.GameData.MultiplierPriceFromStats);
        _priceText.text = _price.ToString() + "$";
        //Debug.Log(newStats.HumourStats[0].Key);
        //GameManager.Instance.HumourTypeDatas.ForEach(k => Debug.Log(k.HumourType));
        _darkHumour.text = GameManager.Instance.HumourTypeDatas.First(k => k.HumourType == newStats.HumourStats[0].Key).DisplayName + " : " + newStats.HumourStats[0].Value.ToString();
        _imitation.text = GameManager.Instance.HumourTypeDatas.First(k => k.HumourType == newStats.HumourStats[1].Key).DisplayName + " : " + newStats.HumourStats[1].Value.ToString();
        _jokes.text = GameManager.Instance.HumourTypeDatas.First(k => k.HumourType == newStats.HumourStats[2].Key).DisplayName + " : " + newStats.HumourStats[2].Value.ToString();

    }

    protected override bool AnalyseNewSlot(Slot newSlot)
    {
        //base.AnalyseNewSlot(newSlot);
        if (newSlot.IsOccupied)
        {
            Debug.Log("newSlot.IsOccupied && _canBeMoved", newSlot.gameObject);
            _c = StartCoroutine(MoveToSlot(_slotBase));
            return false;
        }
        switch (newSlot.SlotType)
        {
            default:
            case Slot.ESlotType.SHOP_SLOT:
                if (_status == EStatus.UNBOUGHT)
                    return true;
                else
                    return false;
            case Slot.ESlotType.TRASH_SLOT:
                if(_status == EStatus.UNBOUGHT)
                {
                    ShopManager.Instance.JokersOnSale.Remove(this);
                }
                else
                {
                    GameManager.Instance.Team.Remove(this);
                }
                StartCoroutine(Die());
                return true;
            case Slot.ESlotType.TEAM_SLOT:
                if (_status == EStatus.TEAMMATE)
                    return true;
                if (ShopManager.Instance.Money >= _price)
                {
                    _status = EStatus.TEAMMATE;
                    ShopManager.Instance.Money -= _price;
                    ShopManager.Instance.JokersOnSale.Remove(this);

                    GameManager.Instance.Team.Add(this);
                    _priceText.gameObject.SetActive(false);
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }


}

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


    public void InitStats(JokerStats newStats)
    {
        _status = EStatus.UNBOUGHT;
        _jokerStats = newStats;
  
        _price = (int)(_jokerStats.HumourStats.Select(d => d.Value).Sum() * GameManager.Instance.GameData.MultiplierPriceFromStats);
        _priceText.text = _price.ToString();
    }

    protected override bool AnalyseNewSlot(Slot newSlot)
    {
        base.AnalyseNewSlot(newSlot);
        if (newSlot.IsOccupied && _canBeMoved)
        {
            Debug.Log("newSlot.IsOccupied && _canBeMoved", newSlot.gameObject);
            _c = StartCoroutine(MoveToSlot(_slotBase));
            return false;
        }
        switch (newSlot.SlotType)
        {
            default:
            case Slot.ESlotType.SHOP_SLOT:
                return false;
            case Slot.ESlotType.TRASH_SLOT:
                GameManager.Instance.Team.Remove(this);
                Destroy(gameObject);
                return true;
            case Slot.ESlotType.TEAM_SLOT:
                if (_status == EStatus.TEAMMATE)
                    return true;
                if (ShopManager.Instance.Money >= _price)
                {
                    _status = EStatus.TEAMMATE;
                    ShopManager.Instance.Money -= _price;
                    Debug.Log(this);
                    Debug.Log(GameManager.Instance);
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private GameManager _gm;

    private void Awake()
    {
        _status = EStatus.UNBOUGHT;
        _gm = GameManager.Instance;
    }
    public void InitStats(JokerStats newStats)
    {
        _jokerStats = newStats;
        _price = (int)(_jokerStats.HumourStats.Select(d => d.Value).Sum() * _gm.GameData.MultiplierPriceFromStats);
    }

    protected override bool AnalyseNewSlot(Slot newSlot)
    {
        base.AnalyseNewSlot(newSlot);
        switch (newSlot.SlotType)
        {
            default:
            case Slot.ESlotType.SHOP_SLOT:
                StartCoroutine(MoveToSlot(_slotBase));
                return false;
            case Slot.ESlotType.TRASH_SLOT:
                _gm.Team.Remove(this);
                Destroy(gameObject);
                return true;
            case Slot.ESlotType.TEAM_SLOT:
                if(_gm.Money >= _price)
                {
                    _status = EStatus.TEAMMATE;
                    _gm.Money-= _price;
                    _gm.Team.Add(this);
                    return true;
                }
                else
                {
                    StartCoroutine(MoveToSlot(_slotBase));
                    return false;
                }
        }
    }


}

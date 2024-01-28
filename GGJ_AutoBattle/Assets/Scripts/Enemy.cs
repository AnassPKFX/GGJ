using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Joker;

public class Enemy : Joker
{

    public override void InitStats(JokerStats newStats)
    {
        _canBeMovedManually = false;
        base.InitStats(newStats);
    }
    protected override bool AnalyseNewSlot(Slot newSlot)
    {
        //base.AnalyseNewSlot(newSlot);
        if (newSlot.IsOccupied && _canBeMoved)
        {
            _c = StartCoroutine(MoveToSlot(_slotBase));
            return false;
        }
        switch (newSlot.SlotType)
        {
            default:
            case Slot.ESlotType.SHOP_SLOT:
            case Slot.ESlotType.TEAM_SLOT:
                return false;
            case Slot.ESlotType.TRASH_SLOT:
                GameManager.Instance.EnemyTeam.Remove(this);
                StartCoroutine(Die());
                return true;
        }
    }
}

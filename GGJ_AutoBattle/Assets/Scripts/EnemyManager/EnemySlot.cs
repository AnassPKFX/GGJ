using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlot : MonoBehaviour
{
    public enum ESlotType
    {
        SLOT_ONE,
        SLOT_TWO,
        SLOT_THREE,
        SLOT_FOUR,
        SLOT_FIVE
    }
    public ESlotType SlotType { get => _slotType; set => _slotType = value; }
    [SerializeField]
    private ESlotType _slotType;
    public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }
    [SerializeField]
    bool _isOccupied;
    private void Awake()
    {
        //if (_slotType == ESlotType.TEAM_SLOT)
            //_isOccupied = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Transform TpPoint => _tpPoint;

    [SerializeField]
    private Transform _tpPoint;

    public enum ESlotType
    {
        SHOP_SLOT,
        TEAM_SLOT,
        TRASH_SLOT
    }
    public ESlotType SlotType => _slotType;
    [SerializeField]
    private ESlotType _slotType;
    public bool IsOccupied => _isOccupied;
    bool _isOccupied;

    private void OnMouseEnter()
    {
        if(GameManager.Instance.CurrentDraggedMovable != null)
        {
            GameManager.Instance.CurrentDraggedMovable.TargetSlot = this;
        }
    }
    private void OnMouseExit()
    {
        if (GameManager.Instance.CurrentDraggedMovable != null)
        {
            GameManager.Instance.CurrentDraggedMovable.TargetSlot = GameManager.Instance.CurrentDraggedMovable.SlotBase;
        }
    }

}

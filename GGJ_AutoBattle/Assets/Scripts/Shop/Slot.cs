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
    public ESlotType SlotType { get => _slotType; set => _slotType = value; }
    [SerializeField]
    private ESlotType _slotType;
    public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }
    [SerializeField]
    bool _isOccupied;
    private void Awake()
    {
        if (_slotType == ESlotType.TEAM_SLOT)
            _isOccupied = false;
    }

    private void OnMouseEnter()
    {
        if(GameManager.Instance.CurrentDraggedMovable != null)
        {
            GameManager.Instance.CurrentDraggedMovable.TargetSlot = this;
        }
    }
    private void OnMouseUp()
    {
        if (GameManager.Instance.CurrentDraggedMovable != null)
        {
            GameManager.Instance.CurrentDraggedMovable.TargetSlot = GameManager.Instance.CurrentDraggedMovable.SlotBase;
        }
    }

}

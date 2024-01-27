using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable : MonoBehaviour
{

    [SerializeField] LayerMask Mask;
    [SerializeField] float lerpMovementTime = .5f;
    [SerializeField] AnimationCurve _animationCurve;
    bool _canBeMoved = true;

    public Slot SlotBase => _slotBase;
    [SerializeField]
    protected Slot _slotBase;
    public Slot TargetSlot { get => _targetSlot; set => _targetSlot = value; }
    Slot _targetSlot;

    public JokerStats JokerStats { get => _jokerStats; set => _jokerStats = value; }
    protected JokerStats _jokerStats;

    protected void Start()
    {
        _canBeMoved = true;

    }

    protected void OnMouseDown()
    {
        if (!_canBeMoved)
            return;        
        GetComponent<Rigidbody>().isKinematic = true;
        GameManager.Instance.CurrentDraggedMovable = this;
    }

    protected void OnMouseDrag()
    {
        if (!_canBeMoved)
            return;
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000.0f, Mask))
        {
            transform.position = hit.point + new Vector3(0,1);
        }
    }

    protected void OnMouseUp()
    {
        if (!_canBeMoved)
            return;
        GameManager.Instance.CurrentDraggedMovable = null;

        if (_targetSlot != _slotBase)
        {
            StartCoroutine(MoveToSlot(_targetSlot));
        }
        else
        {
            StartCoroutine(MoveToSlot(_slotBase));
        }

    }

    protected IEnumerator MoveToSlot(Slot targetSlot)
    {
        Vector3 targetPos = targetSlot.TpPoint.position + new Vector3(0, 1);
        _canBeMoved = false;
        float cursor = 0;
        Vector3 startPos = transform.position;
        while (cursor < lerpMovementTime) 
        { 
            transform.position = Vector3.Lerp(startPos, targetPos, _animationCurve.Evaluate(cursor/lerpMovementTime));
            cursor += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime/lerpMovementTime);
            Debug.Log(cursor);
        }
        GetComponent<Rigidbody>().isKinematic = false;
        if (AnalyseNewSlot(targetSlot))
        {
            _slotBase = targetSlot;

        }
        _canBeMoved = true;
    }

    protected virtual bool AnalyseNewSlot(Slot newSlot)
    {
        return true;
    }
}
public class JokerStats
{
    public string Name => _name;
    string _name;
    public Dictionary<GameData.EHumourType, int> HumourStats => _humourStats;
    private Dictionary<GameData.EHumourType, int> _humourStats;
}

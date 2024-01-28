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
    protected bool _canBeMoved = true;

    public Slot SlotBase { get => _slotBase; set => _slotBase = value; }
    [SerializeField]
    protected Slot _slotBase;
    public Slot TargetSlot { get => _targetSlot; set => _targetSlot = value; }
    Slot _targetSlot;

    public JokerStats JokerStats { get => _jokerStats; set => _jokerStats = value; }
    protected JokerStats _jokerStats;

    protected Coroutine _c;
    protected void Awake()
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
            _c = StartCoroutine(MoveToSlot(_targetSlot));
        }
        else
        {
            _c = StartCoroutine(MoveToSlot(_slotBase));
        }

    }

    protected IEnumerator MoveToSlot(Slot targetSlot)
    {
        _slotBase.IsOccupied = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Vector3 targetPos = targetSlot.TpPoint.position + new Vector3(0, 1);
        _canBeMoved = false;
        float cursor = 0;
        Vector3 startPos = transform.position;
        while (cursor < lerpMovementTime) 
        { 
            transform.position = Vector3.Lerp(startPos, targetPos, _animationCurve.Evaluate(cursor/lerpMovementTime));
            cursor += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime/lerpMovementTime);
            //Debug.Log(cursor);
        }
        GetComponent<Rigidbody>().isKinematic = false;
        if (AnalyseNewSlot(targetSlot))
        {

            targetSlot.IsOccupied = true;
            _slotBase = targetSlot;
        }
        else
        {
            StopCoroutine(_c);
            _canBeMoved = true;
            _c = StartCoroutine(MoveToSlot(_slotBase));
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
    public JokerStats(string name) 
    { 
        _name = name;
        _humourStats = new();
        _humourStats.Add(new JokerStatItem(GameData.EHumourType.DarkHumour, 0));
        _humourStats.Add(new JokerStatItem(GameData.EHumourType.Imitations, 0));
        _humourStats.Add(new JokerStatItem(GameData.EHumourType.Jokes, 0));
    }

    public string Name => _name;
    string _name;

    public struct JokerStatItem
    {
        public JokerStatItem(GameData.EHumourType K, int V)
        {
            Key = K;
            Value = V;
        }
        public GameData.EHumourType Key;
        public int Value;
    }
    public List<JokerStatItem> HumourStats => _humourStats;

    private List<JokerStatItem> _humourStats;
}

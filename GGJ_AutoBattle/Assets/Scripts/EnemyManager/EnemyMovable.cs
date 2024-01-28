using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyMovable : MonoBehaviour
{
    ///public Slot SlotBase { get => _slotBase; set => _slotBase = value; }
    ///[SerializeField]
    ///protected Slot _slotBase;
    ///public Slot TargetSlot { get => _targetSlot; set => _targetSlot = value; }
    ///Slot _targetSlot;

    public EnemyStats EnemyStats { get => _enemyStats; set => _enemyStats = value; }
    protected EnemyStats _enemyStats;
}
public class EnemyStats
{
    public EnemyStats(string name) 
    { 
        _name = name;
        _humourStats = new();
        _humourStats.Add(new EnemyStatItem(GameData.EHumourType.DarkHumour, 0));
        _humourStats.Add(new EnemyStatItem(GameData.EHumourType.Imitations, 0));
        _humourStats.Add(new EnemyStatItem(GameData.EHumourType.Jokes, 0));
    }

    public string Name => _name;
    string _name;

    public struct EnemyStatItem
    {
        public EnemyStatItem(GameData.EHumourType K, int V)
        {
            Key = K;
            Value = V;
        }
        public GameData.EHumourType Key;
        public int Value;
    }
    public List<EnemyStatItem> HumourStats => _humourStats;

    private List<EnemyStatItem> _humourStats;
}

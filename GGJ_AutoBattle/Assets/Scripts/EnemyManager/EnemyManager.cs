using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] List<EnemySlot> _enemySlots;

    public static EnemyManager Instance => _instance;
    private static EnemyManager _instance;

    public int nombreEnnemis = 3;

    private List<EnemyData> _turnStatsData;

    public void Start()
    {
        _turnStatsData = Resources.LoadAll<EnemyData>("Data/Characters/Enemies").ToList();
        InitSlots();
    }
 
    private void InitSlots()
    {
        EnemyData data = _turnStatsData.First(d => d.Turn == GameManager.Instance.CurrentTurn);
        //foreach(EnemySlot slot in _enemyslots)
        for (int i = 0; i < nombreEnnemis; i++)
        {
            var newEnemy = Instantiate(enemyPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            //slot.IsOccupied = true;
            var newStats = new EnemyStats(GameManager.Instance.GameData.ListCharactersNames[UnityEngine.Random.Range(0, GameManager.Instance.GameData.ListCharactersNames.Count())]);

            int countPoint = 0;
            for(int j = 0; j< newStats.HumourStats.Count; j++)
            {
                //Mettre les valeurs comme ci-dessous
                int randStats = Random.Range(0, data.MaxStatPerCategory);
                if (countPoint + randStats > data.MaxStatPerCharacter)
                randStats = Random.Range(0, data.MaxStatPerCategory- countPoint);

                newStats.HumourStats[i] = new(newStats.HumourStats[i].Key, randStats);
                countPoint += randStats;
                //Debug.Log(randStats);
            }
            //newEnemy.SlotBase = slot;
            newEnemy.InitStats(newStats);
        }
    }


}

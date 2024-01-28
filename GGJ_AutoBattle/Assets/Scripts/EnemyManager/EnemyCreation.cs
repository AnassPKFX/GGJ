using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreation : MonoBehaviour
{
    //Même méthode d'initialisation (faudra juste remplacer par les prénoms personalisés si c'est pas chiant
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

    //Initialisation de l'équipe ennemie un par un
    private void InitEnemies()
    {
        foreach (Enemy enemy in _enemies)
        {
            var newEnemy = Instantiate(EnemyPrefab, slot.TpPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
          
            var newStats = new EnemyStats(GameManager.Instance.GameData.ListCharactersNames[UnityEngine.Random.Range(0, GameManager.Instance.GameData.ListCharactersNames.Count())]);

            int countPoint = 0;
            for (int i = 0; i < newStats.HumourStats.Count; i++)
            {
                int randStats = Random.Range(0, data.MaxStatPerCategory);
                if (countPoint + randStats > data.MaxStatPerCharacter)
                    randStats = Random.Range(0, data.MaxStatPerCategory - countPoint);

                newStats.HumourStats[i] = new(newStats.HumourStats[i].Key, randStats);
                countPoint += randStats;
                //Debug.Log(randStats);
            }
            newEnemy.SlotBase = slot;
            newEnemy.InitStats(newStats);
        }
    }
}


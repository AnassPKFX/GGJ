using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

public class FightingManager : MonoBehaviour
{

    private Movable charaSend;
    private Movable enemySend;

    private HumourTypeData themeChosen;
    public static FightingManager Instance => _instance;
    private static FightingManager _instance;

    [SerializeField] Enemy enemyPrefab;
    [SerializeField] GameObject fightScene;

    [SerializeField] List<Slot> _slots;

    private List<EnemyData> _turnStatsData;

    public void Awake()
    {
        _turnStatsData = Resources.LoadAll<EnemyData>("Data/Characters/Enemies").ToList();
        _instance = this;
        fightScene.SetActive(false);

    }

    public void StartFight()
    {
        InitSlots();
        fightScene.SetActive(true);
        StartCoroutine(AutoBattle());


    }

    IEnumerator AutoBattle()
    {

        while (GameManager.Instance.Team.Any() && GameManager.Instance.EnemyTeam.Any())
        {
            //CHANGE DUEL -----------

            if (GameManager.Instance.Team.Any())
            {
                charaSend = GameManager.Instance.Team[Random.Range(0, GameManager.Instance.Team.Count)];
            }

            if (GameManager.Instance.EnemyTeam.Any())
            {
                enemySend = GameManager.Instance.EnemyTeam[0];
            }

            var randomTheme = GameManager.Instance.HumourTypeDict.Keys.ElementAt((int)Random.Range(0, GameManager.Instance.HumourTypeDict.Keys.Count - 1));
            themeChosen = GameManager.Instance.HumourTypeDict[randomTheme];
            Debug.Log("The King chooses the theme : " + themeChosen.DisplayName);


            bool equality = true;

            while (equality)
            {
                //LANCEER BLAGUE -------------

                (charaSend).PlayFX_Joke(themeChosen.Id);
                (enemySend).PlayFX_Joke(themeChosen.Id);
                yield return new WaitForSeconds(2);

                bool testJoueur = false;
                bool testEnnemi = false;

                int limitWinJoke = 5; 
                int y = Random.Range(0, 10);

                if (y >= limitWinJoke)
                {
                    Debug.Log("La blague a fonctionnée");
                    testJoueur = true;
                    (charaSend).PlayFX_Success();
                }
                else
                {
                    Debug.Log("La blague n'a pas fonctionnée");
                    (charaSend).PlayFX_Fail();

                }

                //La blague a-t-elle fonctionné ? -- Ennemi 
                //Récupération de la caractéristique Ennemi
                limitWinJoke = 5; //Arbitraire A CHANGER
                y = Random.Range(0, 10);

                if (y >= limitWinJoke)
                {
                    (enemySend).PlayFX_Success();
                    Debug.Log("La blague ennemie a fonctionnée");
                    testEnnemi = true;
                }
                else
                {
                    (enemySend).PlayFX_Fail();

                    Debug.Log("La blague ennemie n'a pas fonctionnée");
                }
                yield return new WaitForSeconds(1);

                //Résultats 
                if (testJoueur && !testEnnemi)
                {
                    GameManager.Instance.EnemyTeam.Remove((Enemy)enemySend);
                    yield return new WaitForSeconds(.5f);

                    LoseTurn(enemySend);
                    yield return new WaitForSeconds(.5f);

                    equality = false;

                    //Le personnage adverse est éliminé.
                }
                else if (!testJoueur && testEnnemi)
                {
                    yield return new WaitForSeconds(.5f);

                    GameManager.Instance.Team.Remove((Joker)charaSend);
                    LoseTurn(charaSend);
                    yield return new WaitForSeconds(.5f);

                    //Notre personnage est éliminé.
                    equality = false;
                }
                else
                {
                    Debug.Log("Egalité");
                }
                yield return new WaitForSeconds(.5f);
            }

        }

        fightScene.SetActive(false);

        if (GameManager.Instance.Team.Any())
        {
            GameManager.Instance.EnemyTeam.ForEach(x => x.transform.GetComponent<Collider>().enabled = false);
            GameManager.Instance.EnemyTeam.ForEach(x => StartCoroutine(x.MoveToSlot(GameManager.Instance.Trash)));
            GameManager.Instance.EnemyTeam.Clear();
            Debug.Log("Victoire");
            GameManager.Instance.NextPhase(false);
        }
        if (GameManager.Instance.EnemyTeam.Any())
        {
            Debug.Log("Défaite");
            GameManager.Instance.NextPhase(true);
        }

    }
    void LoseTurn(Movable joker)
    {
        StartCoroutine(joker.MoveToSlot(GameManager.Instance.Trash));
    }

    private void InitSlots()
    {
        EnemyData data = _turnStatsData.First(d => d.Turn == GameManager.Instance.CurrentTurn);
        //foreach(EnemySlot slot in _enemyslots)
        foreach (Slot slot in _slots)
        {
            var newEnemy = Instantiate(enemyPrefab, slot.TpPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
            //slot.IsOccupied = true;
            var newStats = new JokerStats(GameManager.Instance.GameData.ListCharactersNames[UnityEngine.Random.Range(0, GameManager.Instance.GameData.ListCharactersNames.Count())]);
            slot.IsOccupied = true;
            slot.SlotType = Slot.ESlotType.ENEMY_SLOT;

            int countPoint = 0;
            for (int j = 0; j < newStats.HumourStats.Count; j++)
            {
                //Mettre les valeurs comme ci-dessous
                int randStats = Random.Range(0, data.MaxStatPerCategory);
                if (countPoint + randStats > data.MaxStatPerCharacter)
                    randStats = Random.Range(0, data.MaxStatPerCategory - countPoint);

                newStats.HumourStats[j] = new(newStats.HumourStats[j].Key, randStats);
                countPoint += randStats;
                //Debug.Log(randStats);
            }
            newEnemy.SlotBase = slot;
            newEnemy.InitStats(newStats);
            GameManager.Instance.EnemyTeam.Add(newEnemy);
        }
    }
}

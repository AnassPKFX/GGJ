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
    private void Start()
    {
        _instance = this;
    }

    public void StartFight()
    {
        if (Fight())
        {
            Debug.Log("Victoire");
        }
        else
        {
            Debug.Log("D�faite");
        }
    }

    bool Fight()
    {
        //Le player choisit son personnage
        if (GameManager.Instance.Team.Any())
        {
            charaSend = GameManager.Instance.Team[Random.Range(0, GameManager.Instance.Team.Count)];
            Debug.Log("Joueur choisit : " + charaSend);
        }
        else
        {
            GameManager.Instance.EnemyTeam.Clear();
            return true;
        }

        //L'ennemi est choisi de gauche � droite et condition de victoire:
        if (GameManager.Instance.EnemyTeam.Any())
        {
            enemySend = GameManager.Instance.EnemyTeam[0];
            Debug.Log("Ennemi : " + enemySend);
        }

        else
        {
            return false;
        }

        StartCoroutine(AutoBattle());

        //Les personnages s'avancent
        //Le th�me se choisit
        var randomKey = GameManager.Instance.HumourTypeDict.Keys.ElementAt((int)Random.Range(0, GameManager.Instance.HumourTypeDict.Keys.Count - 1));
        themeChosen = GameManager.Instance.HumourTypeDict[randomKey];
        Debug.Log("The King chooses the theme : " + themeChosen.DisplayName);

        if (GameManager.Instance.Team.Any())
        {
            GameManager.Instance.EnemyTeam.Clear();
            return true;
        }
        if (GameManager.Instance.EnemyTeam.Any())
        {
            return false;
        }
        return false;
    }

    IEnumerator AutoBattle()
    {

        //Le "combat" commence
        bool fight = true;
        while (fight == true)
        {
            yield return new WaitForSeconds(1);
            bool testJoueur = false;
            bool testEnnemi = false;

            //La blague a-t-elle fonctionn� ? -- Joueur 
            //R�cup�ration de la caract�ristique Joueur
            int caractJoueur = 5; //Arbitraire A CHANGER
            int y = Random.Range(0, 10);

            if (caractJoueur >= y)
            {
                Debug.Log("La blague a fonctionn�e");
                testJoueur = true;
            }
            else
            {
                Debug.Log("La blague n'a pas fonctionn�e");
            }

            //La blague a-t-elle fonctionn� ? -- Ennemi 
            //R�cup�ration de la caract�ristique Ennemi
            int caractEnnemi = 5; //Arbitraire A CHANGER
            y = Random.Range(0, 10);

            if (caractEnnemi >= y)
            {
                Debug.Log("La blague ennemie a fonctionn�e");
                testEnnemi = true;
            }
            else
            {
                Debug.Log("La blague ennemie n'a pas fonctionn�e");
            }

            //R�sultats 
            if (testJoueur && !testEnnemi)
            {
                LoseTurn(enemySend);
                fight = false;
                //Le personnage adverse est �limin�.
            }
            else if (!testJoueur && testEnnemi)
            {
                LoseTurn(charaSend);
                fight = false;
                //Notre personnage est �limin�.
            }
            else
            {
                Debug.Log("Egalit�");
            }
            if (!GameManager.Instance.Team.Any() || !GameManager.Instance.EnemyTeam.Any())
            {
                yield break;
            }
        }
    }
    void LoseTurn(Movable joker)
    {
        StartCoroutine(joker.MoveToSlot(GameManager.Instance.Trash));
    }
}

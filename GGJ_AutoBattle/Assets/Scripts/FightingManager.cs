using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightingManager : MonoBehaviour
{

    // Setup des variables :
    // R�cup�re la Team Joueur
    List<string> playerTeam = new List<string>();

    // R�cup�re la Team Ennemi
    List<string> enemyTeam = new List<string>();

    private string charaSend;
    private string enemySend;

    private int themeChosen;

    private int y;

    private bool gameOver;

    void Start()
    {
        playerTeam.Add("Perso1");
        playerTeam.Add("Perso2");
        playerTeam.Add("Perso3");

        enemyTeam.Add("Enemy1");
        enemyTeam.Add("Enemy2");
        enemyTeam.Add("Enemy3");

            //Le player choisit son personnage
            if (playerTeam.Any())
            {
                charaSend = playerTeam[Random.Range(0, playerTeam.Count)];
                Debug.Log("Joueur choisit : " + charaSend);
            }
            else
            {
                Debug.Log("Victoire");
                UnityEditor.EditorApplication.isPlaying = false;
            }

            //L'ennemi est choisi de gauche � droite et condition de victoire:
            if (enemyTeam.Any())
            {
                enemySend = enemyTeam[0];
                Debug.Log("Ennemi : " + enemySend);
            }

            else
            {
                Debug.Log("D�faite");
                UnityEditor.EditorApplication.isPlaying = false;
            }

            //Les personnages s'avancent
            //Le th�me se choisit
            themeChosen = Random.Range(0, 2);
            Debug.Log("Le th�me choisit est : " + themeChosen);

            //Le "combat" commence
            bool fight = true;
            while (fight == true)
            {
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
                if(testJoueur && !testEnnemi)
                {
                    Debug.Log("Victoire");
                    fight = false;
                    //Le personnage adverse est �limin�.
                }                       
                else if(!testJoueur && testEnnemi)
                {
                    Debug.Log("D�faite");
                    fight = false;
                    //Notre personnage est �limin�.
                }
                else
                {
                    Debug.Log("Egalit�");
                }
            }  
    }

    void Update()
    {
        
    }
}

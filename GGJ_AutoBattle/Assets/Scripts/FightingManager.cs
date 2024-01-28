using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightingManager : MonoBehaviour
{

    // Setup des variables :
    // Récupère la Team Joueur
    List<string> playerTeam = new List<string>();

    // Récupère la Team Ennemi
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

            //L'ennemi est choisi de gauche à droite et condition de victoire:
            if (enemyTeam.Any())
            {
                enemySend = enemyTeam[0];
                Debug.Log("Ennemi : " + enemySend);
            }

            else
            {
                Debug.Log("Défaite");
                UnityEditor.EditorApplication.isPlaying = false;
            }

            //Les personnages s'avancent
            //Le thème se choisit
            themeChosen = Random.Range(0, 2);
            Debug.Log("Le thème choisit est : " + themeChosen);

            //Le "combat" commence
            bool fight = true;
            while (fight == true)
            {
                bool testJoueur = false;
                bool testEnnemi = false;

                //La blague a-t-elle fonctionné ? -- Joueur 
                //Récupération de la caractéristique Joueur
                int caractJoueur = 5; //Arbitraire A CHANGER
                int y = Random.Range(0, 10);
                
                if (caractJoueur >= y)
                {
                    Debug.Log("La blague a fonctionnée");
                    testJoueur = true;
                }
                else
                {
                    Debug.Log("La blague n'a pas fonctionnée");
                }

                //La blague a-t-elle fonctionné ? -- Ennemi 
                //Récupération de la caractéristique Ennemi
                int caractEnnemi = 5; //Arbitraire A CHANGER
                y = Random.Range(0, 10);

                if (caractEnnemi >= y)
                {
                    Debug.Log("La blague ennemie a fonctionnée");
                    testEnnemi = true;
                }
                else
                {
                    Debug.Log("La blague ennemie n'a pas fonctionnée");
                }

                //Résultats 
                if(testJoueur && !testEnnemi)
                {
                    Debug.Log("Victoire");
                    fight = false;
                    //Le personnage adverse est éliminé.
                }                       
                else if(!testJoueur && testEnnemi)
                {
                    Debug.Log("Défaite");
                    fight = false;
                    //Notre personnage est éliminé.
                }
                else
                {
                    Debug.Log("Egalité");
                }
            }  
    }

    void Update()
    {
        
    }
}

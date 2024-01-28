using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyGen : EnemyMovable
{

    public void InitStats(EnemyStats newStats)
    {
        _enemyStats = newStats;
    }

    public void InitSlots(EnemySlot slot)
    {
        //_enemySlots 
        //Faire en sorte que l'ennemi se place au bon endroit
    }

}

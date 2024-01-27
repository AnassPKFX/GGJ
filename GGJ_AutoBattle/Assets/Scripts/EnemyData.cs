using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterTierData;


[CreateAssetMenu(fileName = "EnemyData", menuName = "GGJ_Datas/EnemyData", order = 4)]
public class EnemyData : EntityData
{
    public enum Turn
    {
        TURN_1,
        TURN_2,
        TURN_3,
        TURN_4,
        TURN_5,
        TURN_6,
        TURN_7,
    }
    public ETier Tier => _tier;
    [SerializeField]
    private ETier _tier;

}

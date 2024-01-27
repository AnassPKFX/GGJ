using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterTierData", menuName = "GGJ_Datas/CharacterTierData", order = 3)]
public class CharacterTierData : EntityData
{
    public enum ETier
    {
        TIER_1,
        TIER_2,
        TIER_3,
    }
    public ETier Tier => _tier;
    [SerializeField]
    private ETier _tier;
}

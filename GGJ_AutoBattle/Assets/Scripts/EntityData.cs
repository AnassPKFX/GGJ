using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntityData : ScriptableObject
{
    public int MaxStatPerCategory => _maxStatPerCategory;
    [SerializeField]
    protected int _maxStatPerCategory;
    public int MaxStatPerCharacter => _maxStatPerCharacter;
    [SerializeField]
    protected int _maxStatPerCharacter;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "HumourTypeData", menuName = "GGJ_Datas/HumourTypeData", order = 2)]
public class HumourTypeData : ScriptableObject
{
    public EHumourType HumourType { get => _humourType; set => _humourType = value; }
    [SerializeField]
    private EHumourType _humourType;

    public string DisplayName { get => _displayName; set => _displayName = value; }
    [SerializeField]
    private string _displayName;

    public int Id { get => _id; set => _id = value; }
    [SerializeField]
    private int _id;

}

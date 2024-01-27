using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject ShopScene;
    [SerializeField] List<Slot> _slots;

    public static ShopManager Instance => _instance;
    private static ShopManager _instance;

    void Start()
    {
        _instance = this;
        ShopScene.SetActive(false);
    }

    public void StartShopPhase()
    {
        ShopScene.SetActive(true);
    }

    private void InitSlots()
    {
        //int totalStatPoints = GameManager.Instance.GameData.
    }


}

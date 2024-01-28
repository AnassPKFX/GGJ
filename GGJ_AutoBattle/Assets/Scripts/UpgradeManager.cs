using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;
using UnityEngine.Playables;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance => _instance;
    private static UpgradeManager _instance;

    [SerializeField] GameObject UpgradeScene;


    void Start()
    {
        _instance = this;
        UpgradeScene.SetActive(false);
    }

    public void StartUpgradePhase()
    {
        UpgradeScene.SetActive(true);
    }
    public IEnumerator StopUpgradePhase(GameManager.EUpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case EUpgradeType.ADD_SLOTS:
                yield return new WaitForSeconds(1);
                break;
            case EUpgradeType.ADD_MONEY:
                yield return new WaitForSeconds(1);
                break;
            case EUpgradeType.ADD_STATS:
                yield return new WaitForSeconds(1);
                break; 
        }
        UpgradeScene.SetActive(false);
        GameManager.Instance.NextPhase(false);
    }
}

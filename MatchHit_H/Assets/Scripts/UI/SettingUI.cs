using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameManager gameManager;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void ExitSetting()
    {
        this.gameObject.SetActive(false);
    }

    public void ToHomePage()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.INDIE);
    }

}

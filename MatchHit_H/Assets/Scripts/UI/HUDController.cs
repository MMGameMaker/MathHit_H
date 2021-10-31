using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField]
    private Button settingBtn;

    [SerializeField]
    private GameObject settingUI;

    [SerializeField]
    private GameObject homeBtn;

    public void OnSettingBtnClick()
    {
        settingUI.SetActive(true);
        if(gameManager._gameState == GameManager.eGameSates.INDIE)
        {
            homeBtn.SetActive(false);
        }
    }
}

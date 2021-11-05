using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private Button settingBtn;

    [SerializeField]
    private GameObject settingUI;

    [SerializeField]
    private GameObject homeBtn;


    private void Start()
    {
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();

        //        gameManager.OnGameStateChange.AddListener(DisableOnSceneLoad);

        gameManager.OnGameStateChange += DisableOnSceneLoad;
    }

    public void OnSettingBtnClick()
    {
        settingUI.SetActive(true);
        if(gameManager.CurrentState == GameManager.eGameSates.INDIE)
        {
            homeBtn.SetActive(false);
        }
    }

    public void DisableOnSceneLoad(GameManager.eGameSates gameSates, GameManager.eGameSates lastSate)
    {
        if(gameSates == GameManager.eGameSates.LOADING)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }


}

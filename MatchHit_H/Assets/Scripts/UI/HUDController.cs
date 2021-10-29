using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private Button settingBtn;

    public void OnSettingBtnClick()
    {
        UIManager.Instance.OnGameStateChange(GameManager.eGameSates.SETTING);
    }


}

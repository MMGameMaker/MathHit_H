using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UIPanel
{
    [SerializeField]
    private UIPanel settingPanel;

    public void OpenSettingPanel()
    {
        settingPanel.Show();
    }
}

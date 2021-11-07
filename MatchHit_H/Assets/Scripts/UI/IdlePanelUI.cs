using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePanelUI : UIPanel
{
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        SetupPanelPosition();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void TapToPlay()
    {
        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_STARTED;
    }

}

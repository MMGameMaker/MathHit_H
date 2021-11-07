using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartedPanelUI : UIPanel
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
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

    public void ToGameOver()
    {
        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_OVER;
    }
}

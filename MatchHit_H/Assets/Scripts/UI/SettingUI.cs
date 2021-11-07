using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIPanel
{
    private GameManager gameManager;

    [SerializeField]
    private UIPanel idlePanel;

    [SerializeField]
    private Button homeBtn;

    [SerializeField]
    private GameObject board;

    [SerializeField]
    private GameObject settingBtn;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetupPanelPosition();
    }

    public void ToHomePage()
    {
        GameManager.Instance.CurrentState = GameManager.eGameSates.IDLE;
        this.gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        settingBtn.SetActive(false);
        homeBtn.gameObject.SetActive(true);
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.eGameSates.IDLE:
                idlePanel.Hide();
                homeBtn.gameObject.SetActive(false);
                break;
            case GameManager.eGameSates.GAME_STARTED:
                board.SetActive(false);
                break;
        }
    }

    public override void Hide()
    {
        base.Hide();
        settingBtn.SetActive(true);
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.eGameSates.IDLE:
                idlePanel.Show();
                break;
            case GameManager.eGameSates.GAME_STARTED:
                board.SetActive(true);
                break;
        }
    }

}

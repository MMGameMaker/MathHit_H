using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get;
        private set;
    }

    public UIPanel idlePanelUI;

    public UIPanel gameStartedPanelUI;

    public UIPanel gameOverPanelUI;

    public UIPanel settingPanelUI;


    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;

        GameManager.Instance.OnGameStateChange += OnGameStateChangeUI;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGameStateChangeUI (GameManager.eGameSates currentSate, GameManager.eGameSates lastState)
    {
        switch (currentSate)
        {
            case GameManager.eGameSates.IDLE:
                this.idlePanelUI.Show();
                break;

            case GameManager.eGameSates.GAME_STARTED:
                this.gameStartedPanelUI.Show();
                break;

            case GameManager.eGameSates.GAME_OVER:
                this.gameOverPanelUI.Show();
                break;
        }

        switch (lastState)
        {
            case GameManager.eGameSates.IDLE:
                this.idlePanelUI.Hide();
                break;

            case GameManager.eGameSates.GAME_STARTED:
                this.gameStartedPanelUI.Hide();
                break;

            case GameManager.eGameSates.GAME_OVER:
                this.gameOverPanelUI.Hide();
                break;
        }
    }
}
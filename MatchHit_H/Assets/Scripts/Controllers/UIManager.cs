using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{

    private GameManager gameManager;

    private static UIManager uiManagerInstance;

    public static UIManager Instance
    {
        get;
        private set;
    }

    [SerializeField]
    private GameObject INDIE_UI;

    [SerializeField]
    private GameObject LOADING_UI;

    [SerializeField]
    private GameObject BATTLE_STARTED_UI;

    [SerializeField]
    private GameObject GAME_WIN_UI;

    [SerializeField]
    private GameObject GAME_LOSE_UI;

    private void Awake()
    {
        if(Instance!=null & Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;

        OnDisableAllStateUI();
    }

    private void Start()
    {
        

        gameManager = GameManager.Instance.gameObject.GetComponent<GameManager>();

        GameManager.Instance.OnGameStateChange += GameSateUIChangeHandler;
    }

    public void GameSateUIChangeHandler(GameManager.eGameSates currentState, GameManager.eGameSates lastState)
    {
        switch (currentState)
        {
            case GameManager.eGameSates.INDIE:
                INDIE_UI.SetActive(true);
                break;

            case GameManager.eGameSates.LOADING:
                LOADING_UI.SetActive(true);
                LOADING_UI.GetComponent<GameLoadingUI>().StartCoroutine("GameLoad");
                break;

            case GameManager.eGameSates.BATTLE_STARTED:
                BATTLE_STARTED_UI.SetActive(true);
                break;

            case GameManager.eGameSates.GAME_WIN:
                GAME_WIN_UI.SetActive(true);
                break;

            case GameManager.eGameSates.GAME_LOSE:
                GAME_LOSE_UI.SetActive(true);
                break;
        }

        switch (lastState)
        {
            case GameManager.eGameSates.INDIE:
                INDIE_UI.SetActive(false);
                break;

            case GameManager.eGameSates.LOADING:
                LOADING_UI.SetActive(false);
                break;

            case GameManager.eGameSates.BATTLE_STARTED:
                BATTLE_STARTED_UI.SetActive(false);
                break;

            case GameManager.eGameSates.GAME_WIN:
                GAME_WIN_UI.SetActive(false);
                break;

            case GameManager.eGameSates.GAME_LOSE:
                GAME_LOSE_UI.SetActive(false);
                break;
        }
    }

    private void OnDisableAllStateUI()
    {
        INDIE_UI.SetActive(false);
        LOADING_UI.SetActive(false);
        BATTLE_STARTED_UI.SetActive(false);
        GAME_WIN_UI.SetActive(false);
        GAME_LOSE_UI.SetActive(false);

        Debug.Log("disable all ui");
    }

}

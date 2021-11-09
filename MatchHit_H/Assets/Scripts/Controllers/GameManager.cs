using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [Serializable]
    public enum eGameSates
    {
        IDLE,
        GAME_STARTED,
        GAME_OVER,
        PAUSE,
    }

    public enum eGameEnd
    {
        None,
        Win,
        Lose,
    }

    private eGameEnd gameEnd;
    public eGameEnd GameEnd
    {
        get => gameEnd;
        set { gameEnd = value; }
    }

    private eGameSates currentGameState;

    private eGameSates lastGameState;

    public eGameSates CurrentState 
    { 
        get => currentGameState;
        set
        {
            if(value != Instance.currentGameState)
            {
                lastGameState = currentGameState;
                currentGameState = value;
                OnGameStateChange.Invoke(currentGameState, lastGameState);
                Debug.Log("GameSate: " + currentGameState);
            }
        }
    }

    public delegate void OnGameStateChangeEvent(eGameSates currentSate, eGameSates lastState);

    public OnGameStateChangeEvent OnGameStateChange;

    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        gameEnd = eGameEnd.None;
        OnGameStateChange.Invoke(eGameSates.IDLE, eGameSates.GAME_OVER);
    }
}

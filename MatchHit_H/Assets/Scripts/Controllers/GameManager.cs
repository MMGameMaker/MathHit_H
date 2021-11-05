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
        INDIE,
        LOADING,
        BATTLE_STARTED,
        GAME_WIN,
        GAME_LOSE,
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

    //    public UnityEvent<eGameSates> OnGameStateChange;

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
        OnGameStateChange.Invoke(eGameSates.INDIE, eGameSates.GAME_WIN);
    }
}

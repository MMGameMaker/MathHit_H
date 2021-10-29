using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public enum eGameSates
    {
        INDIE,
        LOAD,
        READY,
        GAME_STARTED,
        SETTING,
        GAME_WIN,
        GAME_LOSE,
    }

    private static GameManager GM_Instance;
    public static GameManager Instance
    {
        get
        {
            if(GM_Instance == null)
            {
                GameObject singletonObject = new GameObject();
                GM_Instance = singletonObject.AddComponent<GameManager>();
                singletonObject.name = "Singleton - GameManager";
                Debug.Log("Create singleton - GameManager");
            }
            return GM_Instance;
        }
        private set { }
    }

    public eGameSates _gameState;


    private void Awake()
    {
        if (GM_Instance != null && GM_Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // set instance
            GM_Instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }

    Dictionary<eGameSates, Action<object>> _gameSateListeners = new Dictionary<eGameSates, Action<object>>();

    // Register to listen for GameSate
    public void RegisGameStateListener(eGameSates gameSate, Action<object> callback)
    {
        //check if listener exist in dictionary
        if (_gameSateListeners.ContainsKey(gameSate))
        {
            _gameSateListeners[gameSate] += callback;
        }
        else
        {
            _gameSateListeners.Add(gameSate, null);
            _gameSateListeners[gameSate] += callback;
            Debug.Log("Register Listener for GameState: " + gameSate);
        }
    }

    public void PostGameSateChange(eGameSates gameSate, object param = null)
    {
        if (!_gameSateListeners.ContainsKey(gameSate))
        {
            Debug.Log("Listeners not contain this state");
            return;
        }

        _gameState = gameSate;
        var callBack = _gameSateListeners[gameSate];
        if (callBack != null)
        {
            callBack(param);
        }
        else
        {
            _gameSateListeners.Remove(gameSate);
        }
    }

}

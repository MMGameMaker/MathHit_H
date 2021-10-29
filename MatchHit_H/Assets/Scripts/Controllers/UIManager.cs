using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager UI_instance;
    public static UIManager Instance
    {
        get
        {
            if (UI_instance == null)
            {
                GameObject singletonObject = new GameObject();
                UI_instance = singletonObject.AddComponent<UIManager > ();
                singletonObject.name = "Singleton - GameManager";
                Debug.Log("Create singleton - GameManager");
            }
            return UI_instance;
        }
        private set { }
    }


    [System.Serializable]
    public struct GameUIPrefab
    {
        public GameManager.eGameSates gameState;
        public GameObject prefab;
    }

    public GameUIPrefab[] gameUIPrefabs;

    private Dictionary<GameManager.eGameSates, GameObject> GameStateUIDict = new Dictionary<GameManager.eGameSates, GameObject>();

    private void Awake()
    {
        if (UI_instance != null && UI_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // set instance
            UI_instance = this;
        }


        for (int i =0; i<gameUIPrefabs.Length; i++)
        {
            if (!GameStateUIDict.ContainsKey(gameUIPrefabs[i].gameState))
            {
                GameStateUIDict.Add(gameUIPrefabs[i].gameState, gameUIPrefabs[i].prefab);
            }
        }

        GameObject.DontDestroyOnLoad(gameObject);

        OnGameStateChange(GameManager.eGameSates.INDIE);

    }

    private void Start()
    {
        for (int i = 0; i < gameUIPrefabs.Length; i++)
        {
            GameManager.Instance.RegisGameStateListener(gameUIPrefabs[i].gameState, (param) => OnGameStateChange(gameUIPrefabs[i].gameState));
 //           Debug.Log("Register Listener for GameState: " + gameUIPrefabs[i].gameState);
        }
    }


    public void OnGameStateChange(GameManager.eGameSates gameSate)
    {

    }


    public void Test()
    {

    }
}

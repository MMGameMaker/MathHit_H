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

    [System.Serializable]
    public struct GameUIPrefab
    {
        public GameManager.eGameSates gameState;
        public GameObject prefab;
    }

    public GameUIPrefab[] gameUIPrefabs;

    private void Awake()
    {
        if(Instance!=null & Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance.gameObject.GetComponent<GameManager>();
        gameManager.OnGameStateChange.AddListener(GameSateUIChange);
    }

    public void GameSateUIChange(GameManager.eGameSates _gameSates)
    {
        for (int i =0; i<gameUIPrefabs.Length; i++)
        {
            if(gameUIPrefabs[i].gameState == _gameSates)
            {
                gameUIPrefabs[i].prefab.SetActive(true);
            }
            else
            {
                gameUIPrefabs[i].prefab.SetActive(false);
            }
        }
    }


   
}

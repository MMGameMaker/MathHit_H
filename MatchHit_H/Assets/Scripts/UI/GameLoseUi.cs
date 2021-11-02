using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoseUi : MonoBehaviour
{
   
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();
    }

    public void Revival()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.BATTLE_STARTED);
    }

    public void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

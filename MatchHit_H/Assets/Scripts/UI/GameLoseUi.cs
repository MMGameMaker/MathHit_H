using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoseUi : MonoBehaviour
{
    public void Revival()
    {
        GameManager.Instance.CurrentState = GameManager.eGameSates.BATTLE_STARTED;
    }

    public void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

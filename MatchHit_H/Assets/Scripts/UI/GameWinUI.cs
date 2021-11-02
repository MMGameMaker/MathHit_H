using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinUI : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();
    }

    public void ToNextLvlIndie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

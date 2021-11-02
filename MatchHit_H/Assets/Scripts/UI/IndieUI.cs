using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndieUI : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();
    }

    public void OnTapToPlay()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.LOADING);
        Debug.Log("Tap Tap!");
    }
}

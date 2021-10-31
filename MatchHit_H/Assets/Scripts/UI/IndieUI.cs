using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndieUI : MonoBehaviour
{
    public GameManager gameManager;
    public void OnTapToPlay()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.LOADING);
        Debug.Log("Tap Tap!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardEvent : MonoBehaviour
{
    public static BoardEvent Instance { get; set; }

    public enum eBoardState
    {
        INIT,
        WAITING_BATTLE_HIT,
        NORMAL,
    }

    private eBoardState currentBoardState;

    public eBoardState CurrentBoardSate 
    {
        get { return Instance.currentBoardState; }
        set
        {
                currentBoardState = value;

                BoardStateChangeHandle.Invoke(currentBoardState);

                Debug.Log("Change boardState: " + currentBoardState) ;
        }
    }

    public delegate void OnBoardStateChange(eBoardState currentState);

    public OnBoardStateChange BoardStateChangeHandle;

    

    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
}

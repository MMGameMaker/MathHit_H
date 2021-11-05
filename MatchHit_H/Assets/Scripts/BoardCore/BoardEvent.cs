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
        LOADING,
        STARTED,
        MATCHING_A_TYPE,
        MATCHFINISHED,
        END,
    }

    private eBoardState currentBoardState;
    private eBoardState lastBoardState;

    public eBoardState CurrentBoardSate 
    {
        get { return Instance.currentBoardState; }
        set
        {
                currentBoardState = value;

                BoardStateChangeHandler.Invoke(currentBoardState);

                Debug.Log("Change boardState: " + currentBoardState) ;
        }
    }

    public delegate void OnBoardStateChange(eBoardState currentState);

    public static OnBoardStateChange BoardStateChangeHandler;

    

    private void Awake()
    {
        Instance = this;
    }
}

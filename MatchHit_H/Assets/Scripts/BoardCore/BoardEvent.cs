using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardEvent : MonoBehaviour
{
    public enum eBoardState
    {
        INIT,
        LOADING,
        STARTED,
        MATCHING_A_TYPE,
        MATCHFINISHED,
        END,
    }

    //    public UnityEvent<eBoardState> OnBoardStateChange;

    private eBoardState currentBoardState;
    private eBoardState lastBoardState;

    public eBoardState CurrentBoardSate 
    {
        get { return Instance.currentBoardState; }
        set
        {
            if (value != currentBoardState)
            {
                lastBoardState = currentBoardState;

                currentBoardState = value;

                BoardStateChangeHandler.Invoke(currentBoardState);

                Debug.Log("Change boardState!");
            }
        }
    }

    public delegate void OnBoardStateChange(eBoardState currentState);

    public static OnBoardStateChange BoardStateChangeHandler;

    public static BoardEvent Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        CurrentBoardSate = eBoardState.INIT;
    }






}

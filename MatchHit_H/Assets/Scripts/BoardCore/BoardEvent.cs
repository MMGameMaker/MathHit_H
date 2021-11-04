using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardEvent : MonoBehaviour
{
    public enum eBoardState
    {
        NORMAL,
        ISMATCHINGCAKE,
    }

    public UnityEvent<eBoardState> OnBoardStateChange;

    public static BoardEvent Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }






}

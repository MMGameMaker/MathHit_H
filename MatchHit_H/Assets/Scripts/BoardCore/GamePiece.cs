using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    private int boardIndex;
    public int BoardIndex
    {
        get { return boardIndex; }
        set { boardIndex = value; }
    }

    public int X
    {
        get { return boardIndex % board.XDim; }
    }

    public int Y
    {
        get { return boardIndex / board.XDim; }
        
    }

    private BoardEvent boardEvent;

    private Collider2D colliderComponent;

    public Collider2D ColliderComponent
    {
        get { return colliderComponent; }
    }

    private ClearablePiece clearableComponent;
    public ClearablePiece ClearableComponent
    {
        get { return clearableComponent; }
    }

    private CakePiece cakeComponent;
    public CakePiece CakeComponent
    {
        get { return cakeComponent; }
    }

    private MoveablePiece moveableComponent;
    public MoveablePiece MoveableComponent
    {
        get { return moveableComponent; }
    }

    private SpecialPiece specialComponent;
    public SpecialPiece SpecialComponent 
    { 
        get { return specialComponent; }
    }

    private RockPiece rockComponent;
    public RockPiece RockComponent
    {
        get { return rockComponent; }
    }

    private BoardManager.ePieceType type;
    public BoardManager.ePieceType Type
    {
        get { return type; }
    }

    private BoardManager board;
    public BoardManager BoardRef
    {
        get { return board; }
    }

    public delegate void OnPieceMatched();

    public OnPieceMatched OnPieceMatchedHandler;

    public delegate void OnclearMatched();

    public OnclearMatched OnClearMatchedHandler;


    private void Awake()
    {
        //Get Component if avaiable
        clearableComponent = GetComponent<ClearablePiece>();
        cakeComponent = GetComponent<CakePiece>();
        moveableComponent = GetComponent<MoveablePiece>();
        specialComponent = GetComponent<SpecialPiece>();
        rockComponent = GetComponent<RockPiece>();
        colliderComponent = GetComponent<Collider2D>();

        boardEvent = BoardEvent.Instance.GetComponent<BoardEvent>();
        

        //Register Battle event
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnReady, (param) => OnBattleReadyHandler());
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnBattleShow, (param) => OnBattleShowHandler());
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnBattleEnd, (param) => OnBattleEndHandler());
    }

    private void Start()
    {
        if (isHasCollider())
            this.colliderComponent.enabled = false;
    }

    private void OnBattleReadyHandler()
    {
        if (isHasCollider())
            this.colliderComponent.enabled = true;
    }

    private void OnBattleShowHandler()
    {
        if (isHasCollider())
            this.colliderComponent.enabled = false;
    }

    private void OnBattleEndHandler()
    {
        if (isHasCollider())
            this.colliderComponent.enabled = true;
    }

    public void Init(int i, BoardManager _board, BoardManager.ePieceType _type)
    {
        this.boardIndex = i;
        this.board = _board;
        this.type = _type;
    }

    private void OnMouseDown()
    {
        board.StartMatching();
        board.UpdateMatchList(this);
    }

    private void OnMouseEnter()
    {
        board.UpdateMatchList(this);
    }

    private void OnMouseUp()
    {
        board.FinishMatch();
    }


    public bool isMoveabe()
    {
        return moveableComponent != null;
    }

    public bool isCake()
    {
        return cakeComponent != null; 
    }

    public bool isClearable()
    {
        return clearableComponent != null;
    }

    public bool isRock()
    {
        return rockComponent != null;
    }

    public bool isHasCollider()
    {
        return colliderComponent != null;
    }
}

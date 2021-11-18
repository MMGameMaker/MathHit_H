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

    private Animator pieceAnimator;

    private int matchedHash = Animator.StringToHash("InMatchList");

    private int clearMatchedHash = Animator.StringToHash("ClearMatch");




    private void Awake()
    {
        //Get Component if avaiable
        clearableComponent = GetComponent<ClearablePiece>();
        cakeComponent = GetComponent<CakePiece>();
        moveableComponent = GetComponent<MoveablePiece>();
        specialComponent = GetComponent<SpecialPiece>();
        rockComponent = GetComponent<RockPiece>();
        colliderComponent = GetComponent<Collider2D>();

        pieceAnimator = GetComponent<Animator>();

        boardEvent = BoardEvent.Instance.GetComponent<BoardEvent>();
        

        //Register Battle event
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnReady, (param) => OnBattleReadyHandler());
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

    public void OnPieceMatchedHandler()
    {
        pieceAnimator.SetBool(matchedHash, true);

        if (isCake())
        {
            CakeComponent.OnCakeMatchedHandler();
        }

        if (isSpecial())
        {
            SpecialComponent.OnSpecialMatchedHandler();
        }
    }

    public void OnPieceOutMatchedHandler()
    {
        pieceAnimator.SetBool(matchedHash, false);

        if (isCake())
        {
            CakeComponent.OnCakeOutMatchedHandler();
        }

        if (isSpecial())
        {
            SpecialComponent.OnSpecialOutMatchedHandler();
        }
    }

    public void OnClearMatchedHandler()
    {
        if (!isClearable())
        {
            return;
        }

        pieceAnimator.SetBool(clearMatchedHash, true);

        ClearableComponent.StartCoroutine("ClearCoroutine");
    }


    public bool isMoveabe()
    {
        return moveableComponent != null;
    }

    public bool isCake()
    {
        return cakeComponent != null; 
    }

    public bool isSpecial()
    {
        return specialComponent != null;
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

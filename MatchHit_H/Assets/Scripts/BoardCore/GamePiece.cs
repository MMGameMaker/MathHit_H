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



    private void Awake()
    {
        clearableComponent = GetComponent<ClearablePiece>();
        cakeComponent = GetComponent<CakePiece>();
        moveableComponent = GetComponent<MoveablePiece>();
        specialComponent = GetComponent<SpecialPiece>();
        boardEvent = BoardEvent.Instance.GetComponent<BoardEvent>();
    }


    // Start is called before the first frame update
    void Start()
    {
        if(cakeComponent)
        boardEvent.OnBoardStateChange.AddListener(cakeComponent.OnBoardStateChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int i, BoardManager _board, BoardManager.ePieceType _type)
    {
        this.boardIndex = i;
        this.board = _board;
        this.type = _type;
    }

    private void OnMouseDown()
    {
        board.IsMatching = true;
        board.UpdateMatchList(this);
    }

    private void OnMouseEnter()
    {
        if (!board.IsMatching)
        {
            return;
        }
        else
        {
            board.UpdateMatchList(this);
        }        
    }

    private void OnMouseUp()
    {
        if (!board.IsMatching)
        {
            return;
        }
        Debug.Log("finish matching!");
        board.ResetMatchList();
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

    

}

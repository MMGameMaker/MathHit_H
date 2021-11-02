using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    private int x;
    private int y;

    public int X
    {
        get { return x; }
        set 
        {
            if (isMoveabe())
                x = value;
        }
    }

    public int Y
    {
        get { return y; }
        set 
        {
            if (isMoveabe())
                y = value;
        }
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
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int _x, int _y, BoardManager _board, BoardManager.ePieceType _type)
    {
        this.x = _x;
        this.y = _y;
        this.board = _board;
        this.type = _type;
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

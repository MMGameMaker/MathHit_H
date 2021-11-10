using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RockPiece : MonoBehaviour
{
    private int sustainPoint = 3;

    public int SustainPoint 
    {
        get => this.sustainPoint;
        set { this.sustainPoint = value; }
    }

    [SerializeField]
    private Sprite firstBreakSprite;

    [SerializeField]
    private Sprite secondBreakSprite;

    private GamePiece piece;

    private SpriteRenderer sprite;

    private BoardManager board;

    Action<object> _OnReceiveEventMatchFinish;

    private void Awake()
    {
        //Get Component
        piece = GetComponent<GamePiece>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _OnReceiveEventMatchFinish = (param) => OnSustainChangeHandler();

        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnMatchFinish, _OnReceiveEventMatchFinish);
    }

    public void OnSustainChangeHandler()
    {
        switch (sustainPoint)
        {
            case 3:
                break;
            case 2:
                sprite.sprite = this.firstBreakSprite;
                break;
            case 1:
                sprite.sprite = this.secondBreakSprite;
                break;

            // for sustainPoin <= 0 => destroy piece and spawn a new EMPTY PIECE instead
            default:
                int newPieceIndex = piece.BoardIndex;

                BattleEventDispatcher.Instance.RemoveListener(EventID.EvenID.OnMatchFinish, _OnReceiveEventMatchFinish);

                piece.ClearableComponent.Clear();
                BoardManager.boardInstance.SpawnNewPiece(newPieceIndex, BoardManager.ePieceType.EMPTY);
                break;
        }
    }

    

}
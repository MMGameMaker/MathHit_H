using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoadPreset", menuName = "GameConfiguration/BoadPreset", order = 1)]
public class BoadPreset : ScriptableObject
{
    BoardManager board = BoardManager.boardInstance;

    // list constain ihdexes of rockPiece in board
    public int[] rockPieceList;

    // list constain indexes of fixed Piece in board
    public int[] fixedBGPieceList;

    /* Map Board Index:
      0,  1,  2,  3,  4,  5,
      6,  7,  8,  9, 10, 11,
     12, 13, 14, 15, 16, 17,
     18, 19, 20, 21, 22, 23,
     24, 25, 26, 27, 28, 29,
     30, 31, 32, 33, 34, 35
    */

    private void Awake()
    {
        CreateRockList();

        CreateFixedBGList();
    }

    // Set values to rockPieceList
    public void CreateRockList()
    {
        rockPieceList = new int[] { 14, 15 };
    }

    // Set values to fixedBGPieceList
    public void CreateFixedBGList()
    {
        fixedBGPieceList = new int[]
        {
        0,                  5,
        6,                  11,
        12,                 17,
        18, 19,         22, 23,
        24, 25,         28, 29,
        30, 31, 32, 33, 34, 35
        };
    }
}

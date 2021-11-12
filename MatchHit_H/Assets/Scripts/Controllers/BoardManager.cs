using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public BoadPreset boardPreset;

    public static BoardManager boardInstance;
    
    public enum ePieceType
    {
        EMPTY,
        NORMALCAKE,
        SPECIAL,
        ROCK,
        FIXED_BG,
    }

    [SerializeField]
    private int xDim;

    public int XDim { get => xDim; }

    [SerializeField]
    private int yDim;

    public int YDim { get => yDim; }

    private int boardSize;

    public float fillTime;

    private bool isFilling = false;


    [System.Serializable]
    public struct PiecePrefab
    {
        public ePieceType type;
        public GameObject prefab;
    }

    public GameObject backgroundPrefab;

    public PiecePrefab[] piecePrefabs;

    private Dictionary<ePieceType, GameObject> piecePrefabDict = new Dictionary<ePieceType, GameObject>();

    private GamePiece[] pieces;

    List<GamePiece> matchList = new List<GamePiece>();

    private bool isMatching = false;

    private GamePiece lastListPiece;

    private bool constainSpecial = false;

    private CakePiece.CakeType listCakeType;

    public CakePiece.CakeType ListCakeType { get => listCakeType; }

    CakePiece.CakeType checkNeighborCaketype;

    public bool IsMatching 
    { 
        get {return isMatching; }
        set { this.isMatching = value; }
    }

    private BoardEvent boardEvent;

    [SerializeField]
    private LineRenderer lineMatch;

    public int MatchPoint { get; private set; }

    private void Awake()
    {
        GameManager.Instance.OnGameStateChange += ChangeBoardActive;

        BoardEvent.Instance.BoardStateChangeHandle += OnBoardSateChange;

        boardInstance = this;
        
        boardSize = xDim * yDim;

        pieces = new GamePiece[boardSize];

        
        // create piecetypeDict include type and gameobjectprefab
        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
                Debug.Log("add prefab!");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BoardEvent.Instance.CurrentBoardSate = BoardEvent.eBoardState.INIT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBoardActive(GameManager.eGameSates currentSate, GameManager.eGameSates lastState)
    {
        switch (currentSate)
        {
            case GameManager.eGameSates.IDLE:
                this.gameObject.SetActive(false);
                break;
            case GameManager.eGameSates.GAME_STARTED:
                this.gameObject.SetActive(true);
                break;
            case GameManager.eGameSates.GAME_OVER:
                this.gameObject.SetActive(false);
                break;
        }
    }

    public void OnBoardSateChange(BoardEvent.eBoardState currentState)
    {
        switch (currentState)
        {
            case BoardEvent.eBoardState.INIT:
                InitNewBoard();
                break;
            case BoardEvent.eBoardState.WAITING_BATTLE_HIT:
                break;         
            case BoardEvent.eBoardState.NORMAL:
                break;
        }
    }


    #region Init and spawn new Board
    private void InitNewBoard()
    {
        //Spawn fixed BG
        if (boardPreset.fixedBGPieceList != null)
        {
            for (int i = 0; i < boardPreset.fixedBGPieceList.Length; i++)
            {
                SpawnNewPiece(boardPreset.fixedBGPieceList[i], ePieceType.FIXED_BG);
            }
        }

        // spawn background cells
        for (int i = 0; i < boardSize; i++)
        {
            if (pieces[i] != null)
                continue;
            GameObject pieceBG = GameObject.Instantiate(backgroundPrefab, GetWorldPosition(i), Quaternion.identity);
            pieceBG.transform.parent = this.transform;
        }

        // spawn CakePieces
        SpawnBoard();
    }

    public void SpawnBoard()
    {
        //Spawn Rock Pieces base boardPreset Config
        if(boardPreset.rockPieceList != null)
        {
            for (int i = 0; i< boardPreset.rockPieceList.Length; i++)
            {
                SpawnNewPiece(boardPreset.rockPieceList[i], ePieceType.ROCK);
            }
        }

        // spawn normal Cake in the rest of cell board
        for (int i = 0; i < boardSize; i++)
        {
            if (pieces[i] != null)
                continue;
            else
            {
                SpawnNewPiece(i, ePieceType.NORMALCAKE);

                pieces[i].CakeComponent.SetType((CakePiece.CakeType)Random.Range(0, pieces[i].CakeComponent.NumCakeType));
            }  
        }
    }
    #endregion


    #region Matching Clearing and reFilling GamePieces
    // Check and update MatchList when mouse focus to a gamepiece
    public void UpdateMatchList(GamePiece newPiece)
    {
        // return if is not matching a list
        if (!this.IsMatching)
            return;

        // return if board is still filling
        if (this.isFilling)
            return;

        //Check for unmatch last piece in list
        if (matchList.Count > 1 && newPiece == matchList[matchList.Count - 2])
        {
            if (matchList[matchList.Count - 1].Type == ePieceType.SPECIAL)
            {
                this.constainSpecial = false;
            }

            ResetEffectScale(matchList[matchList.Count - 1]);

 //           matchList[matchList.Count - 1].OnClearMatchedHandler.Invoke();

            matchList.Remove(matchList[matchList.Count - 1]);

            lineMatch.positionCount--;

            if(matchList.Count == 1 && matchList[0].Type == ePieceType.SPECIAL)
            {
                StopMatchingSuggest();
            }


            Debug.Log("remove piece");
            return;
        }

        // Check if the newpiece is a specical type, add to list if the MatchList isn't contain a special piece before.
        if (newPiece.Type == ePieceType.SPECIAL && !constainSpecial)
        {
            AddToMatchList(newPiece);
            constainSpecial = true;
            return;
        }
        else if(newPiece.Type == ePieceType.SPECIAL && constainSpecial)
        {
            return;
        }

        // Check the the first matchlist component
        if (matchList.Count == 0)
        {
            AddToMatchList(newPiece);
            if (newPiece.Type == ePieceType.NORMALCAKE)
            {
                listCakeType = newPiece.CakeComponent.Type;
                MatchingSuggest();
            }
            return;
        }

        // Check the the seconde matchlist component to set matchlist type
        if (matchList.Count == 1)
        {
            if (matchList[0].Type == ePieceType.NORMALCAKE)
            {
                if (IsNeighbor(newPiece, matchList[matchList.Count - 1]) && IsSameCakeType(newPiece, listCakeType) && !matchList.Contains(newPiece))
                {
                    AddToMatchList(newPiece);
                    return;
                }
            }
            else if (matchList[0].Type == ePieceType.SPECIAL && IsNeighbor(newPiece, matchList[matchList.Count - 1]))
            {
                listCakeType = newPiece.CakeComponent.Type;
                MatchingSuggest();
                AddToMatchList(newPiece);
                return;
            }   
        }

        // Normal Check for the next matchlist from thirth
        if (IsNeighbor(newPiece, matchList[matchList.Count - 1]) && IsSameCakeType(newPiece, listCakeType) && !matchList.Contains(newPiece))
        {
            AddToMatchList(newPiece);
            Debug.Log("add piece");
            return;
        }
        return;
    }

    public void StartMatching()
    {
        this.isMatching = true;
        matchList.Clear();
        MatchPoint = 0;
        Debug.Log("start matching!");
    }

    private void AddToMatchList(GamePiece newPiece)
    {
        matchList.Add(newPiece);

        // add line position while matching
        lineMatch.positionCount++;
        lineMatch.SetPosition(lineMatch.positionCount - 1, newPiece.transform.position);

        //add matchpoint
        if(newPiece.Type == ePieceType.NORMALCAKE)
        {
            MatchPoint++;
        }
        else if(newPiece.Type == ePieceType.SPECIAL)
        {
            MatchPoint += newPiece.SpecialComponent.SpecialValue;
        }

        //effect scale up

//        newPiece.OnPieceMatchedHandler.Invoke();

        if(newPiece.Type == ePieceType.NORMALCAKE)
        {
            newPiece.CakeComponent.lightBGSprite.gameObject.SetActive(true);
            newPiece.transform.localScale = new Vector3(0.75f, 0.75f, 0);
        }
        else if(newPiece.Type == ePieceType.SPECIAL)
        {
            newPiece.transform.localScale = new Vector3(0.4f, 0.4f, 0);
        }
        

        Debug.Log("Add a piece to list!");
    }

    private void ResetEffectScale(GamePiece newPiece)
    {
 //       newPiece.OnClearMatchedHandler.Invoke();

        if (newPiece.Type == ePieceType.NORMALCAKE)
        {
            newPiece.CakeComponent.lightBGSprite.gameObject.SetActive(false);
            newPiece.transform.localScale = new Vector3(0.6f, 0.6f, 0);
        }
        else if (newPiece.Type == ePieceType.SPECIAL)
        {
            newPiece.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        }
        
    }

    public void FinishMatch()
    {
        int specialValue;
        int specialIndex;

        if (matchList.Count < 2)
        {
            ResetEffectScale(matchList[0]);
 //           matchList[0].OnClearMatchedHandler.Invoke();
            matchList.Clear();
        }

        // Clear rest of pieces in match list
        ClearMatchListPieces();

        // clear line match
        lineMatch.positionCount = 0;

        // Check for creating a special Piece
        if (matchList.Count >= 4 && !constainSpecial)
        {
            specialValue = matchList.Count - 1;
            specialIndex = matchList[matchList.Count - 1].BoardIndex;
            Destroy(pieces[specialIndex].gameObject);
            pieces[specialIndex] = SpawnSpecialPiece(specialIndex, specialValue);
            Debug.Log("Spawn Special Piece, value: " + specialValue);
        }

        this.isMatching = false;
        this.constainSpecial = false;
        StopMatchingSuggest();

        //post match finish event
        if (matchList.Count >=2) 
        {
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnMatchFinish, MatchPoint);
        }

        // Filling empty piece 
        StartCoroutine(Fill());

        Debug.Log("finish matching!");

    }

    public void ClearMatchListPieces()
    {
        for (int i = 0; i < matchList.Count; i++)
        {
            int boardIndex = matchList[i].BoardIndex;

            CheckAffectRockPiece(boardIndex);

            matchList[i].ClearableComponent.Clear();

            SpawnNewPiece(boardIndex, ePieceType.EMPTY);
        }
    }

    // Check and affect if there is a rock surroud
    public void CheckAffectRockPiece(int boardIndex)
    {
        // check upper piece
        if(pieces[boardIndex].Y > 0)
        {
            GamePiece pieceUpper = pieces[boardIndex - xDim];
            if (pieceUpper.Type == ePieceType.ROCK)
            {
                pieceUpper.RockComponent.SustainPoint--;
            }
        }

        //check below piece
        if(pieces[boardIndex].Y < yDim - 1)
        {
            GamePiece pieceLower = pieces[boardIndex + xDim];
            if (pieceLower.Type == ePieceType.ROCK)
            {
                pieceLower.RockComponent.SustainPoint--;
            }
        }

        //check left piece
        if(pieces[boardIndex].X > 0)
        {
            GamePiece pieceLeft = pieces[boardIndex - 1];
            if(pieceLeft.Type == ePieceType.ROCK)
            {
                pieceLeft.RockComponent.SustainPoint--;
            }
        }

        //check right piece
        if (pieces[boardIndex].X < xDim -1)
        {
            GamePiece pieceRight = pieces[boardIndex + 1];
            if (pieceRight.Type == ePieceType.ROCK)
            {
                pieceRight.RockComponent.SustainPoint--;
            }
        }
    }
    #endregion

    public void ClearBoardCake()
    {   
        for (int i = 0; i < boardSize; i++)
        {
            if(pieces[i].Type == ePieceType.NORMALCAKE)
            Destroy(pieces[i].gameObject);
        }
    }

    public void MatchingSuggest()
    {
        for(int i =0; i < boardSize; i++)
        {
            if (pieces[i].isCake())
            {
                pieces[i].CakeComponent.ChangeToInactiveSprite();
            }
        }
    }

    public void StopMatchingSuggest()
    {
        for (int i = 0; i < boardSize; i++)
        {
            if (pieces[i].isCake())
            {
                pieces[i].CakeComponent.ChangeToNormalSprite();
            }
        }
    }

    public IEnumerator Fill()
    {
        yield return new WaitForSeconds(0.15f);

        this.isFilling = true;

        while (FillStep())
        {
            yield return new WaitForSeconds(fillTime);
        }

        this.isFilling = false;
    }

    public bool FillStep()
    {
        bool movePiece = false;

        for(int i = boardSize - xDim -1; i>=0; i--)
        {
            GamePiece piece = pieces[i];

            if (piece.isMoveabe())
            {
                GamePiece pieceBelow = pieces[i + xDim];

                if (pieceBelow.Type == ePieceType.EMPTY)
                {
                    Destroy(pieceBelow.gameObject);
                    piece.BoardIndex = i + xDim;
                    piece.MoveableComponent.Move(piece.BoardIndex, fillTime);
                    pieces[i + xDim] = piece;
                    SpawnNewPiece(i, ePieceType.EMPTY);
                    movePiece = true;
                }
                else
                {
                    for (int diag = -1; diag <= 1; diag++)
                    {
                        if (diag == 0)
                            continue;

                        int diagIndex = i + diag;

                        if ((diagIndex % xDim >= 0) && (diagIndex % xDim < xDim))
                        {
                            GamePiece diagPiece = pieces[diagIndex + xDim];

                            if(diagPiece.Type == ePieceType.EMPTY)
                            {
                                bool hasPieceAbove = true;

                                for ( int diagAbove = diagIndex; diagAbove>=0; diagAbove -= xDim)   //
                                {
                                    GamePiece pieceAbove = pieces[diagAbove];

                                    if (pieceAbove.isMoveabe())
                                    {
                                        break;
                                    }
                                    else if(!pieceAbove.isMoveabe() && pieceAbove.Type != ePieceType.EMPTY)
                                    {
                                        hasPieceAbove = false;
                                        break;
                                    }

                                }

                                if (!hasPieceAbove)
                                {
                                    Destroy(diagPiece.gameObject);
                                    piece.MoveableComponent.Move(diagIndex + xDim, fillTime);
                                    pieces[diagIndex + xDim] = piece;
                                    SpawnNewPiece(i, ePieceType.EMPTY);
                                    movePiece = true;
                                    break;
                                }

                            }

                        }

                    }
                }
            }
        }

        for (int i = 0; i < xDim; i++)
        {
            GamePiece pieceBelow = pieces[i];

            if (pieceBelow.Type == ePieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject);

                GameObject newPiece = GameObject.Instantiate(piecePrefabDict[ePieceType.NORMALCAKE], new Vector3(GetWorldPosition(i).x, GetWorldPosition(i).y + 1, 0), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[i] = newPiece.GetComponent<GamePiece>();
                pieces[i].Init(i, this, ePieceType.NORMALCAKE);
                pieces[i].CakeComponent.SetType((CakePiece.CakeType)Random.Range(0, pieces[i].CakeComponent.NumCakeType));
                pieces[i].MoveableComponent.Move(i, fillTime);
                
                movePiece = true;
            }
        }

        return movePiece;
    }

    public GamePiece SpawnSpecialPiece(int boardIndex, int value)
    {
        GamePiece newSpecialPieces = SpawnNewPiece(boardIndex, ePieceType.SPECIAL);

        newSpecialPieces.SpecialComponent.SpecialValue = value;

        return newSpecialPieces;
    }


    //Check if board still has potential Match
    public bool IsHasPotentialMatch() 
    {
        bool hasPotentialMatch = false;

        for (int i = 0; i<boardSize-xDim; i++)
        {
            if (pieces[i].Type == ePieceType.EMPTY)
            {
                continue;
            }

            if (pieces[i].Type == ePieceType.SPECIAL)
            {
                return true;
            }

            if(pieces[i].Type == ePieceType.NORMALCAKE)
            {
                checkNeighborCaketype = pieces[i].CakeComponent.Type;

                if(i%xDim !=(xDim - 1))
                {
                    if (pieces[i + 1].Type == ePieceType.NORMALCAKE && IsSameCakeType(pieces[i + 1], checkNeighborCaketype))
                    {
                        return true;
                    }

                    if (pieces[i + xDim + 1].Type == ePieceType.NORMALCAKE && IsSameCakeType(pieces[i + 1], checkNeighborCaketype))
                    {
                        return true;
                    }
                }

                if(i%xDim != 0)
                {
                    if (pieces[i + xDim - 1].Type == ePieceType.NORMALCAKE && IsSameCakeType(pieces[i + 1], checkNeighborCaketype))
                    {
                        return true;
                    }
                }

                if (pieces[i + xDim].Type == ePieceType.NORMALCAKE && IsSameCakeType(pieces[i + 1], checkNeighborCaketype))
                {
                    return true;
                }

            }
        }
        return hasPotentialMatch;
    }


    public Vector2 GetWorldPosition(int i)
    {
        int y = i / xDim;

        int x = i % xDim;
        
        return new Vector2((transform.position.x - (xDim*0.5f - x - 0.5f)*4/3) , (transform.position.y + (yDim*0.5f  - y - 0.5f)*4/3));
    }

    public GamePiece SpawnNewPiece(int i, ePieceType type)
    {
        GameObject newpiece = GameObject.Instantiate(piecePrefabDict[type], GetWorldPosition(i), Quaternion.identity);
        newpiece.transform.parent = this.transform;

        pieces[i] = newpiece.GetComponent<GamePiece>();
        pieces[i].Init(i, this, type);

        return pieces[i];
    }

    private bool IsNeighbor(GamePiece piece1, GamePiece piece2)
    {
        if (piece1 == piece2)
            return false;
        if (Mathf.Abs(piece1.X - piece2.X) > 1)
            return false;
        if (Mathf.Abs(piece1.Y - piece2.Y) > 1)
            return false;
        return true;
    }

    private bool IsSameCakeType(GamePiece piece1, CakePiece.CakeType cakeType)
    {
        if (piece1.CakeComponent.Type == cakeType)
            return true;
        else
            return false;
    }

}

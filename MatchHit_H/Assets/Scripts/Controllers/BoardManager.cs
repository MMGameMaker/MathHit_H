using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private GameManager gameManager;

    public static BoardManager boardInstance;
    
    public enum ePieceType
    {
        EMPTY,
        NORMALCAKE,
        SPECIAL,
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

<<<<<<< HEAD
    [SerializeField]
    private GameObject lineMatch;

    private LineRenderer lineComponent;
=======
>>>>>>> 237698e1f17bcb17e83829881d65bcf03cfc445b

    private void Awake()
    {
        lineComponent = lineMatch.GetComponent<LineRenderer>();

        boardEvent = BoardEvent.Instance.GetComponent<BoardEvent>();

        boardInstance = this;
        
        boardSize = xDim * yDim;

        pieces = new GamePiece[boardSize];

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
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();

        gameManager.OnGameStateChange.AddListener(BoardActiveControll);

        BoardEvent.BoardStateChangeHandler += this.OnBoardSateChange;

        boardEvent.CurrentBoardSate = BoardEvent.eBoardState.INIT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBoardSateChange(BoardEvent.eBoardState currentState)
    {
        switch (currentState)
        {
            case BoardEvent.eBoardState.INIT:
                InitNewBoard();
                break;

            case BoardEvent.eBoardState.LOADING:
                this.gameObject.SetActive(false);
                break;

            case BoardEvent.eBoardState.STARTED:
                this.gameObject.SetActive(true);
                break;

            case BoardEvent.eBoardState.MATCHING_A_TYPE:

                break;

            case BoardEvent.eBoardState.MATCHFINISHED:
                break;

            case BoardEvent.eBoardState.END:
                break;

        }
    }

    private void InitNewBoard()
    {
        // spawn background cells
        for (int i = 0; i < boardSize; i++)
        {
            GameObject pieceBG = GameObject.Instantiate(backgroundPrefab, GetWorldPosition(i), Quaternion.identity);
            pieceBG.transform.parent = this.transform;
        }

        // spawn CakePieces
        SpawnBoard();
    }

    public void SpawnBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            SpawnNewPiece(i, ePieceType.NORMALCAKE);

            pieces[i].CakeComponent.SetType((CakePiece.CakeType)Random.Range(0, pieces[i].CakeComponent.NumCakeType));
        }
    }

    public void ClearBoardCake()
    {   
        for (int i = 0; i < boardSize; i++)
        {
            Destroy(pieces[i].gameObject);
        }
    }

    public void MatchingSuggest()
    {

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
                    piece.MoveableComponent.Move(pieceBelow.BoardIndex, fillTime);
                    piece.BoardIndex = i + xDim;
                    pieces[i + xDim] = piece;
                    SpawnNewPiece(i, ePieceType.EMPTY);
                    movePiece = true;
                }
            }
        }

        for (int i = 0; i < xDim; i++)
        {
            GamePiece pieceBelow = pieces[i];

            if (pieceBelow.Type == ePieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject);

                GameObject newPiece = GameObject.Instantiate(piecePrefabDict[ePieceType.NORMALCAKE], new Vector3(GetWorldPosition(i).x, GetWorldPosition(i).y + 2, 0), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[i] = newPiece.GetComponent<GamePiece>();
                pieces[i].Init(i, this, ePieceType.NORMALCAKE);
                pieces[i].MoveableComponent.Move(i, fillTime);
                pieces[i].CakeComponent.SetType((CakePiece.CakeType)Random.Range(0, pieces[i].CakeComponent.NumCakeType));
                movePiece = true;
            }
        }

        return movePiece;
    }

    public void UpdateMatchList(GamePiece newPiece)
    {

        if (!isMatching)
        {
            return;
        }

        if(newPiece.Type == ePieceType.SPECIAL && !constainSpecial)
        {
            if (matchList.Count == 0)
            {
                lineMatch.SetActive(true);
            }
            AddToMatchList(newPiece);
            constainSpecial = true;
            return;
        } 

        if (matchList.Count == 0)
        {
            AddToMatchList(newPiece);

            lineMatch.SetActive(true);

            if (newPiece.Type == ePieceType.NORMALCAKE)
            {
                listCakeType = newPiece.CakeComponent.Type;
                boardEvent.CurrentBoardSate = BoardEvent.eBoardState.MATCHING_A_TYPE;
            }

            Debug.Log("add first piece" + newPiece.X + " , " + newPiece.Y) ;
            return;
        }

        if(matchList.Count == 1)
        {
            if(matchList[0].Type == ePieceType.NORMALCAKE)
            {
                if(IsNeighbor(newPiece, matchList[matchList.Count - 1]) && IsSameCakeType(newPiece, listCakeType) && !matchList.Contains(newPiece))
                {
                    AddToMatchList(newPiece);
                    Debug.Log("add piece");
                    return;
                }
            }
            else if (matchList[0].Type == ePieceType.SPECIAL)
            {
                listCakeType = newPiece.CakeComponent.Type;
                boardEvent.CurrentBoardSate = BoardEvent.eBoardState.MATCHING_A_TYPE;
                AddToMatchList(newPiece);
                return;
            }


        }

        if (IsNeighbor(newPiece, matchList[matchList.Count - 1]) && IsSameCakeType(newPiece, listCakeType) && !matchList.Contains(newPiece))
        {
            AddToMatchList(newPiece);
            Debug.Log("add piece");
            return;
        }

        //unmatch newpiece
        if (newPiece == matchList[matchList.Count - 1] && matchList.Count > 1)
        {
            if(newPiece.Type == ePieceType.SPECIAL)
            {
                this.constainSpecial = false;
            }
            matchList.Remove(newPiece);
            RemoveLinePoint();
            if (newPiece.Type != ePieceType.SPECIAL)
            {
                newPiece.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                newPiece.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            }
                
            Debug.Log("remove piece");
            return;
        }
        return;
    }

    private void AddToMatchList(GamePiece newPiece)
    {
        matchList.Add(newPiece);
        AddLinePoint(newPiece.transform.position);
        if (newPiece.Type != ePieceType.SPECIAL)
        {
            newPiece.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        }
        else
        {
            newPiece.transform.localScale = new Vector3(0.75f, 0.75f, 1);
        }
            
    }


    public void ResetMatchList()
    {
        int specialValue;
        int specialIndex;

        ResetLine();
        lineMatch.SetActive(false);

        if (matchList.Count < 2)
        {
            matchList[0].transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            ClearMatchList();
        }
        
        if(matchList.Count >= 4 && !constainSpecial)
        {
            specialValue = matchList.Count - 1;
            specialIndex = matchList[matchList.Count - 1].BoardIndex;
            Destroy(pieces[specialIndex].gameObject);
            pieces[specialIndex] = SpawnSpecialPiece(specialIndex, specialValue);
            Debug.Log("Spawn Special Piece, value: " + specialValue);
        }
        
        this.matchList.Clear();
        this.constainSpecial = false;
        this.isMatching = false;
        Debug.Log("reset matchlist!");

        StartCoroutine(Fill());

        boardEvent.CurrentBoardSate = BoardEvent.eBoardState.MATCHFINISHED;

        while (!IsHasPotentialMatch())
        {
            Debug.Log("There is no matchavaiable!");
            ClearBoardCake();
            SpawnBoard();
        }
    }

    public void ClearMatchList()
    {
        for(int i = 0; i <matchList.Count; i++)
        {
            int boardIndex = matchList[i].BoardIndex;

            matchList[i].ClearableComponent.Clear();

            SpawnNewPiece(boardIndex, ePieceType.EMPTY);
        }
    }

    private void AddLinePoint(Vector2 matchpoint)
    {
        lineComponent.positionCount++;
        lineComponent.SetPosition(lineComponent.positionCount - 1, matchpoint); 
    }

    private void RemoveLinePoint()
    {
        lineComponent.positionCount--;
    }

    private void ResetLine()
    {
        lineComponent.positionCount = 0;
    }


    public GamePiece SpawnSpecialPiece(int boardIndex, int value)
    {
        GamePiece newSpecialPieces = SpawnNewPiece(boardIndex, ePieceType.SPECIAL);

        newSpecialPieces.SpecialComponent.SpecialValue = value;

        return newSpecialPieces;
    }

    public bool IsHasPotentialMatch() 
    {
        bool hasPotentialMatch = false;

        for (int i = 0; i<boardSize-xDim; i++)
        {

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


    public void BoardActiveControll(GameManager.eGameSates gameSates)
    {
        if(gameSates == GameManager.eGameSates.BATTLE_STARTED)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public Vector2 GetWorldPosition(int i)
    {
        int y = i / xDim;

        int x = i % xDim;
        
        return new Vector2(transform.position.x - xDim + x*2 + 1 , transform.position.y + yDim  - y*2 - 1);
    }

    private GamePiece SpawnNewPiece(int i, ePieceType type)
    {
        GameObject newpiece = GameObject.Instantiate(piecePrefabDict[type], GetWorldPosition(i), Quaternion.identity);
        newpiece.transform.parent = this.transform;

//        Debug.Log("init piece!");

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
        if (piece1.Type != ePieceType.NORMALCAKE)
            return false;
        if (piece1.CakeComponent.Type == this.listCakeType)
            return true;
        else
            return false;
    }
}
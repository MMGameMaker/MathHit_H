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

    public bool IsMatching 
    { 
        get {return isMatching; }
        set { this.isMatching = value; }
    }

    private void Awake()
    {
        boardInstance = this;
        
        boardSize = xDim * yDim;

        pieces = new GamePiece[boardSize];

        for (int i=0; i<piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
                Debug.Log("add prefab!");
            }
        }

        // spawn background cells
        for (int i = 0; i < boardSize; i++)
        {
            GameObject pieceBG = GameObject.Instantiate(backgroundPrefab, GetWorldPosition(i), Quaternion.identity);
            pieceBG.transform.parent = this.transform;
        }

        // spawn normal cake piece to board
        for (int i = 0; i < boardSize; i++)
        {
            SpawnNewPiece(i, ePieceType.NORMALCAKE);

            pieces[i].CakeComponent.SetType((CakePiece.CakeType)Random.Range(0, pieces[i].CakeComponent.NumCakeType));           
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();

        gameManager.OnGameStateChange.AddListener(BoardActiveControll);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator Fill()
    {

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

        if (matchList.Count == 0)
        {
            matchList.Add(newPiece);
            newPiece.transform.localScale = new Vector3(1.1f, 1.1f, 1);
            Debug.Log("add first piece" + newPiece.X + " , " + newPiece.Y) ;
            return;
        }

        if (isNeighbor(newPiece, matchList[matchList.Count - 1]) && isSameCakeType(newPiece, matchList[matchList.Count - 1]) && !matchList.Contains(newPiece))
        {
            matchList.Add(newPiece);
            newPiece.transform.localScale = new Vector3(1.1f, 1.1f, 1);
            Debug.Log("add piece");
            return;
        }

        if (newPiece == matchList[matchList.Count - 1] && matchList.Count > 1)
        {
            matchList.Remove(newPiece);
            newPiece.transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("remove piece");
            return;
        }
        return;
    }

    public void ResetMatchList()
    {
        if(matchList.Count >= 2)
        {
            ClearMatchList();
        }
        else
        {
            matchList[0].transform.localScale = new Vector3(1, 1, 1);  
        }
        this.matchList.Clear();
        this.isMatching = false;
        Debug.Log("reset matchlist!");

        StartCoroutine(Fill());
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

    private bool isNeighbor(GamePiece piece1, GamePiece piece2)
    {
        if (piece1 == piece2)
            return false;
        if (Mathf.Abs(piece1.X - piece2.X) > 1)
            return false;
        if (Mathf.Abs(piece1.Y - piece2.Y) > 1)
            return false;
        return true;
    }

    private bool isSameCakeType(GamePiece piece1, GamePiece piece2)
    {
        if (piece1.CakeComponent.Type == piece2.CakeComponent.Type)
            return true;
        else
            return false;
    }
}

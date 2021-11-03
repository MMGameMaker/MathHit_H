using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private GameManager gameManager;
    
    public enum ePieceType
    {
        EMPTY,
        NORMALCAKE,
        SPECIAL,
    }

    [SerializeField]
    private int xDim;

    [SerializeField]
    private int yDim;

    private int boardSize;


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




    private void Awake()
    {
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
        int x = i / xDim;

        int y = i % xDim;
        
        return new Vector2(transform.position.x - xDim + x*2 + 1 , transform.position.y + yDim  - y*2 - 1);
    }

    private GamePiece SpawnNewPiece(int i, ePieceType type)
    {
        GameObject newpiece = GameObject.Instantiate(piecePrefabDict[type], GetWorldPosition(i), Quaternion.identity);
        newpiece.transform.parent = this.transform;

        Debug.Log("init piece!");

        pieces[i] = newpiece.GetComponent<GamePiece>();
        pieces[i].Init(i, this, type);

        return pieces[i];
    }

}

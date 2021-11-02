using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private GameManager gameManager;
    
    public enum ePieceType
    {
        EMPTY,
        NORMAL,
        SPECIAL,
    }

    public GameObject backgroundPrefab;

    [SerializeField]
    private int xDim;

    [SerializeField]
    private int yDim;




    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();

        gameManager.OnGameStateChange.AddListener(BoardActiveControll);

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject pieceBG = GameObject.Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                pieceBG.transform.parent = transform;
            }
        }
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

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim + x*2 + 1 , transform.position.y + yDim  - y*2 - 1);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLoadingUI : MonoBehaviour
{
    public GameManager gameManager;
    public Text countDownText;
    private int countDownNum;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager.OnGameStateChange.AddListener(StartLoading);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLoading(GameManager.eGameSates gameSates)
    {
        if (gameManager._gameState != GameManager.eGameSates.LOADING)
        {
            return;
        }
        StartCoroutine("GameLoad");
    }


    IEnumerator GameLoad()
    {
        this.countDownNum = 3;
        while (countDownNum >= 1)
        {
            countDownText.text = countDownNum.ToString();
            yield return new WaitForSeconds(1f);
            countDownNum--;
        }

        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.BATTLE_STARTED);
    }


}

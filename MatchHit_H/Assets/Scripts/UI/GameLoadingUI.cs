using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLoadingUI : MonoBehaviour
{
    public Text countDownText;
    private int countDownNum;

    IEnumerator GameLoad()
    {
        this.countDownNum = 3;
        while (countDownNum >= 1)
        {
            countDownText.text = countDownNum.ToString();
            yield return new WaitForSeconds(1f);
            countDownNum--;
        }

        GameManager.Instance.CurrentState = GameManager.eGameSates.BATTLE_STARTED;
    }
}

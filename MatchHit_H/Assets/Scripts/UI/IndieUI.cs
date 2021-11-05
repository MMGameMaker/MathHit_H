using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndieUI : MonoBehaviour
{

    public void OnTapToPlay()
    {
        GameManager.Instance.CurrentState = GameManager.eGameSates.LOADING;
    }
}

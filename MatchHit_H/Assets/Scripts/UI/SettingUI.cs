using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private Button SoundPlay;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        gameManager = GameManager.Instance.transform.GetComponent<GameManager>();
    }

    public void ExitSetting()
    {
        this.gameObject.SetActive(false);
    }

    public void ToHomePage()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.INDIE);
        ExitSetting();
    }



}

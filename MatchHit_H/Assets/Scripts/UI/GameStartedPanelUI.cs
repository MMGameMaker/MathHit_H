using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartedPanelUI : UIPanel
{
    [SerializeField]
    private Image playerHealthFill;

    [SerializeField]
    private Image enemyHealthFill;

    [SerializeField]
    private Character player;

    [SerializeField]
    private Character enemy; 


    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPanelPosition();
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnEnemyTakingDamage, (param) => OnEnemyTakeDamage());
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnPlayerTakingDamage, (param) => OnPlayerTakeDamage());
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    void OnEnemyTakeDamage()
    {
        enemyHealthFill.fillAmount = (float)enemy.CurHealth / (float)enemy.MaxHealth;
    }

    void OnPlayerTakeDamage()
    {
        playerHealthFill.fillAmount = (float)player.CurHealth / (float)player.MaxHealth;
    }


}

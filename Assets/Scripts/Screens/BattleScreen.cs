using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScreen : MonoBehaviour
{
    public List<Hero> heroTeam;
    public Enemy enemy;
    [SerializeField] Transform heroTeamLayout;
    [SerializeField] Hero heroPrefab;

    void Update()
    {
   
        if((GameManager.Instance.GetGameState == GameManager.GameState.IdleIntoEnemyTurn)&&!GameManager.Instance.isOnIdle)
            GameManager.Instance.EnemyIdleStart();
        else if (GameManager.Instance.GetGameState == GameManager.GameState.BattleEnemyTurn)
            GameManager.Instance.EnemysTurn();
    }

    #region Screen Setup
    public void GridFirstSetup()
    {
        HeroGridSetup();
        EnemySetup();
    }
    public void GridRestart()
    {
        UpdateHeroes();
        EnemySetup();
    }

    void HeroGridSetup()
    {
        for (int i=0;i<3;i++)
        {
            heroTeam.Add(Instantiate(heroPrefab, heroTeamLayout));
            heroTeam[i].SetHeroByID(GameManager.Instance.HeroTeam[i].HeroID, true);
            heroTeam[i].HeroOnBattleScreen();
            heroTeam[i].RestartHealth();
           // Debug.Log($"hero number {i+1} ready");
            heroTeam[i].enemyTarget = enemy;
        }
    }
    public void UpdateHeroes()
    {

        for (int i = 0; i < 3; i++)
        {
            heroTeam[i].SetHeroByID(GameManager.Instance.HeroTeam[i].HeroID, true);
            heroTeam[i].HeroOnBattleScreen();
            heroTeam[i].HeroSelected(false);
            heroTeam[i].RestartHealth();
            heroTeam[i].enemyTarget = enemy;
        }
    }
    void EnemySetup()
    {
        enemy.RestartHealth();
    }
    #endregion


}

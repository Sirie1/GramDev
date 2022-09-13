using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScreen : MonoBehaviour
{
    public List<Hero> heroTeam;
    public Enemy enemy;
    [SerializeField] Transform heroTeamLayout;
    [SerializeField] Hero heroPrefab;
    Hero newHero;

    Hero battleHeroSelected;

    int level;

    // Start is called before the first frame update
    void Start()
    {
        //GridSetup();

    }
    void Update()
    {
        if (!GameManager.Instance.isInBattlePlayerTurn())
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
            heroTeam[i].SetHeroByID(GameManager.Instance.heroTeam[i].HeroID, true);
            heroTeam[i].HeroOnBattleScreen();
            heroTeam[i].RestartHealth();
            Debug.Log($"hero number {i+1} ready");
            heroTeam[i].enemyTarget = enemy;
        }
    }
    public void UpdateHeroes()
    {

        for (int i = 0; i < 3; i++)
        {
            heroTeam[i].SetHeroByID(i + 1, true);
            heroTeam[i].HeroOnBattleScreen();
            heroTeam[i].HeroSelected(false);
            heroTeam[i].RestartHealth();
            heroTeam[i].enemyTarget = enemy;
        }
    }
    void EnemySetup()
    {
        enemy.RestartHealth();
      //  enemy.lifeDisplay.UpdateHealth();
    }
    #endregion


}
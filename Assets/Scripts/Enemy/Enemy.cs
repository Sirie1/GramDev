using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : MonoBehaviour
{
    string enemyName;
    float maxHealth;
    float battleHealth;
    float attackPower;
    bool isDead;
    public bool isAttackFinished;
    [SerializeField] int randomHero;

    List<int> possibleTargets = new List<int> ();
    public BattleScreen battleScreen;
  //  public Hero targetHero;
    public LifeDisplay lifeDisplay;
    #region Gets & Setters
    public string EnemyName
    {
        get { return enemyName; }
        set { enemyName = value; }
    }    
    public float MaxHealth 
    {   get { return maxHealth;}
        set { maxHealth = value; }
    }

    public float BattleHealth
    {
        get { return battleHealth;}
        set { battleHealth = value; }
    }
    public float AttackPower
    {
        get { return attackPower;}
        set { attackPower = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
    }
    #endregion

    private void OnEnable()
    {
        enemyName = "Roach";
        //      maxHealth = 75f;
        maxHealth = 75f;
        RestartHealth();
        //       attackPower = 10f;
        attackPower = 20f;
        isDead = false;
        possibleTargets.Add(0);
        possibleTargets.Add(1);
        possibleTargets.Add(2);
    }

    #region Battle Functionalities
    public void TakeDamage(float damage)
    {
        battleHealth -= damage;
        lifeDisplay.UpdateHealth();
        Debug.Log ($"Enemy {enemyName} attacked, now has {battleHealth} of health");
        if (battleHealth <= 0)
        {
            battleHealth = 0;
            Debug.Log($"Enemy {enemyName} is dead");
            isDead = true;
            GameManager.Instance.CheckEnd();
        }
        /*else
           Debug.Log($"Enemy attacked, {damage} of damage taken");*/

    }
    //When enemy attacks first checks which heroes are dead to erase them from the random target
    public void AttackHero()
    {
        for (int i= 0; i<3; i++)
        {
            if (battleScreen.heroTeam[i].IsDead)
            {
                possibleTargets.Remove(i);
            }
        }
        randomHero = Random.Range(0, possibleTargets.Count);
        Debug.Log($"Enemy attacking hero {battleScreen.heroTeam[possibleTargets[randomHero]].HeroName}");
        battleScreen.heroTeam[possibleTargets[randomHero]].TakeDamage(attackPower);

        //GameManager.Instance.EnemyTurnEnded();
    }
    public void RestartHealth()
    {
        battleHealth = maxHealth;
        isDead = false;
        lifeDisplay.UpdateHealth();
    }
    # endregion

}

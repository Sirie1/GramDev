using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class Hero : MonoBehaviour
{
    //Base Attributes. This attributes do not change when experience is gained or played a battle. They are changed if game balance or re-design is needed. 
    int heroID;
    Color heroColour;
    string heroName;
    float basemaxHealth;
    float baseAttackPower;
    float attributeIncreasePerLevel;

    //In Game variable attributes. This attributes may change after or during game. Max experience = 5, when reached 5 automatically levels up. Level increase increases hero's attack and health by 10%
    float maxHealth;
    float battleHealth;
    float attackPower;
    [SerializeField] bool isDead;
    bool isInCollection;
    int experience;
    int level;

    //Button display functionalities. Needed to references to objects needed by button and display functionalities. 
    public Button myButton;
    public LifeDisplay lifeDisplay;
    public GameObject statsBackground;
    public StatsDisplay statsDisplay;
    public ButtonLongHold buttonLongHold;
    public Transform selected;
    //Other References needed
    public Image heroImage;
    public Enemy enemyTarget;
    #region Gets&Setters

    public int HeroID
    {
        get { return heroID; }
    }
    public string HeroName
    {
        get { return heroName; }
        set { heroName = value; }
    }
    public float BattleHealth
    {
        get { return battleHealth; }
        set { battleHealth = value; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public float AttackPower
    {
        get { return attackPower; }
        set {attackPower = value;}
    }
    public int Experience
    {
        get { return experience; }
    }
    public int Level
    {
        get { return level; }
    }
    public bool IsDead
    {
        get { return isDead; }
    }
    public bool IsInCollection
    {
        get { return isInCollection; }
        set { isInCollection = value; }
    }
    #endregion
    void Start()
    {
        //To test we set health from here/*
        /*maxHealth = 100;
        RestartHealth();
        attackPower = 10;
        experience = 0;
        level = 0;
        isDead = false;*/

        HeroSelected(false);
        buttonLongHold.gameObject.SetActive(false);
    }
    //SetHeroByID sets and configures a Hero on an Hero created object. It gathers information from GameData for basic non-user information about the Hero, and from
    //UserData if the hero already exists in UserData and have saved attributes. It also sets the value of isInCollection, depending if the ID was found in heroes
    //UserData list. 
    public void SetHeroByID (int configID, bool AddToCollection)
    {
        heroID = configID;
        //We need to get stats from data manager, basic data as name, colour, basic attributes, 
        foreach (DataManager.HeroStats heroBlueprint in DataManager.Instance.gameDataLoad.HeroesStats)
        {
            if (heroBlueprint.heroID == heroID)
            {
                heroColour = heroBlueprint.heroColour;
                heroImage.color = heroColour;
                //heroImage.tintColor = heroColour;
                heroName = heroBlueprint.heroName;
                this.gameObject.name = heroName;
                //Debug.Log ($"{heroName} was configured");
                basemaxHealth = heroBlueprint.basemaxHealth;
                baseAttackPower = heroBlueprint.baseAttackPower;
                attributeIncreasePerLevel = heroBlueprint.attributeIncreasePerLevel;
            }
        }
        //Now we need to check if it is already unlocked
        isInCollection = false;
        foreach (DataManager.UserHeroProgress ownedHero in DataManager.Instance.userData.UserHeroes)
        {
            if (ownedHero.heroID == heroID)
            {
                maxHealth = ownedHero.maxHealth;
                attackPower = ownedHero.attackPower;
                experience = ownedHero.experience;
                level = ownedHero.level;
                isInCollection = true;
            }
        }
        //If it was not found in the list of owned, is a new hero. If it was asked to join collection, we add it here
        if (!isInCollection)
        {
            maxHealth = basemaxHealth;
            attackPower = baseAttackPower;
            experience = 0;
            level = 1;
            /*if (AddToCollection)
                isInCollection = true;*/
        }
    }

    #region OnHeroSprite
    //Sends the pointer to the hero to the game manager to manage the action to take
    public void OnHeroClick(Hero heroClicked)
    {
        GameManager.Instance.OnHeroSelect(heroClicked);
        
    }
    //Set if hero is selected by passing state of selection bool as argument
    public void HeroSelected(bool state)
    {
        selected.gameObject.SetActive(state);
    }
    //Starting holding the hero enables object with countintg time script
    public void OnHeroHold()
    {
        buttonLongHold.gameObject.SetActive(true);
    }
    //Closes the attribute display when player stops clicking and disables gameobject buttonLongHold, which counts time of pressing during its update. 
    public void OnHeroRelease()
    {
        buttonLongHold.gameObject.SetActive(false);
        statsBackground.SetActive(false);
        statsDisplay.gameObject.SetActive(false);
    }
    //If Hero is set on battle, lifebar should be visible.
    public void HeroOnBattleScreen()
    {
        lifeDisplay.gameObject.SetActive(true);
    }
    //If Hero is on select screen lifebar should not be visible. 
    public void HeroOnSelectScreen()
    {
        lifeDisplay.gameObject.SetActive(false);
    }
    //This function may be obsolete, need to work in isInCollection bool
    public void HeroInCollection(bool isInCollection)
    {
        this.isInCollection = isInCollection;
        if (isInCollection)
        {
            heroImage.color = heroColour;
            myButton.interactable = true;
            //Debug.Log($"Enabling collection for hero {HeroID}");

            
            //other option yourButton.enabled = false;
        }
        else
        {
            heroImage.color = Color.gray;
            myButton.interactable = false;
            //Debug.Log($"Disabling collection for hero {HeroID}");
        }
    }
    #endregion
    
    #region OnBattleScreen
    
    //When hero takes damage, function updates health, checks death and calls game manager to check if battle ended. 
    public void TakeDamage(float damage)
    {
        battleHealth -= damage;
        lifeDisplay.UpdateHealth();

        this.transform.DOPunchPosition(new Vector3(10, 0, 0), 0.5f);

        if (battleHealth <= 0)
        {
            battleHealth = 0;
            isDead = true;
            Debug.Log($"{heroName} took {damage} damage. {battleHealth} health remaining. He is dead.");
            GameManager.Instance.CheckEnd();
        }
        else 
            Debug.Log($"{heroName} took {damage} damage. {battleHealth} health remaining");
    }


    public void Attack()
    {
        Debug.Log($"{heroName} is attacking the enemy. Dealing {attackPower} of damage");

        this.transform.DOPunchPosition(new Vector3(10, 0, 0), 0.5f);
        
        enemyTarget.TakeDamage(attackPower);
    }
    /*
    private void OnAttackComplete()
    {
        this.transform.DOLocalMoveX(-4f, 0.5f);
    }*/
    #endregion

    #region OnEndScreen
    public void increaseExperience()
    {
        experience++;
        if ((experience == 5))
            increaseLevel();
    }
    void increaseLevel()
    {
        level++;
        maxHealth = maxHealth + maxHealth * attributeIncreasePerLevel;
        attackPower = attackPower + attackPower * attributeIncreasePerLevel;
        experience = 0;
    }
    public void RestartHealth()
    {
        battleHealth = maxHealth;
        isDead=false;
        lifeDisplay.UpdateHealth();
    }
    #endregion
}

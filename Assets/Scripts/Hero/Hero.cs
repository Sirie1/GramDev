using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
//using Debug = UnityEngine.Debug;


public class Hero : MonoBehaviour
{
    //Base Attributes
    int heroID;
    Color heroColour;
    string heroName;
    float basemaxHealth;
    float baseAttackPower;
    float attributeIncreasePerLevel;

    //In Game variable attributes. Max experience = 5, when reached 5 automatically levels up. Level increase increases hero's attack and health by 10%
    float maxHealth;
    float battleHealth;
    float attackPower;
    [SerializeField] bool isDead;
    bool isInCollection;
    int experience;
    
    int level;

    //Button display functionalities
    public Button myButton;
    public LifeDisplay lifeDisplay;
    public GameObject statsBackground;
    public StatsDisplay statsDisplay;
    public ButtonLongHold buttonLongHold;
    public Transform selected;

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
    //Hero config, has to check for base attributes, and see if already exists in user data. We can add it to user collection
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
                Debug.Log ($"{heroName} was configured");
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
            if (AddToCollection)
                isInCollection = true;
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

    public void OnHeroRelease()
    {
        buttonLongHold.gameObject.SetActive(false);
        statsBackground.SetActive(false);
        statsDisplay.gameObject.SetActive(false);
    }
    public void HeroOnBattleScreen()
    {
        lifeDisplay.gameObject.SetActive(true);
    }
    public void HeroOnSelectScreen()
    {
        lifeDisplay.gameObject.SetActive(false);
    }

    public void HeroInCollection(bool isInCollection)
    {
        if (!isInCollection)
        {
            heroImage.color = Color.gray;
            myButton.interactable = false;
            //other option yourButton.enabled = false;
        }
        else
        {
            heroImage.color = heroColour;
            myButton.interactable = true;
        }
    }
    #endregion



    #region OnBattleScreen
    public void TakeDamage(float damage)
    {

        battleHealth -= damage;

        lifeDisplay.UpdateHealth();

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
        enemyTarget.TakeDamage(attackPower);
    }


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

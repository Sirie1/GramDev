using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    //Screens references
    [SerializeField] BattleScreen battleScreen;
    public SelectScreen selectScreen;
    public EndScreen endScreen;
    //Object managed references
    public Hero battleHeroSelected;
    public Enemy enemySelected;
    public bool playerWonMatch;

    //Definitions needed
    public List<Hero> heroTeam;


    //State management
    private enum GameState
    {
        HeroSelection,
        BattlePlayerTurn,
        BattleEnemyTurn,
        EndBattle
    }
    GameState gameState;

    #region Singleton 
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return _instance;
        }
    }
    #endregion

    #region Awake and Start
    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {

        // GoToSelectScreenFirstTime();
        GoToSelectScreen();


    }
    # endregion

    #region State Managing
    //This is needed for battle screen Update, only place where a state of game is needed outside game manager
    public bool isInBattlePlayerTurn()
    {
        if (gameState == GameState.BattlePlayerTurn)
            return true;
        else
            return false;
    }
    public void EnemyTurnEnded()
    {
        gameState = GameState.BattlePlayerTurn;
    }
    public void PlayerTurnEnded()
    {
        gameState = GameState.BattleEnemyTurn;
    }
    #endregion

    #region Screen Managing

    /*public void GoToSelectScreenFirstTime()
    {

        heroTeam = new List<Hero>();

        selectScreen.gameObject.SetActive(true);
        battleScreen.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        gameState = GameState.HeroSelection;
    }*/
    public void GoToSelectScreen()
    {

        heroTeam = new List<Hero>();

        selectScreen.gameObject.SetActive(true);
        battleScreen.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        //selectScreen.UpdateHeroes();
        gameState = GameState.HeroSelection;
    }


    public void GoToBattleScreen()
    {
        if (heroTeam.Count == 3)
        {
            battleHeroSelected = null;
            selectScreen.gameObject.SetActive(false);
            battleScreen.gameObject.SetActive(true);
            endScreen.gameObject.SetActive(false);
            if (battleScreen.heroTeam.Count > 0)
            
                battleScreen.GridRestart();
            
            else
                battleScreen.GridFirstSetup();
            gameState = GameState.BattlePlayerTurn;
            Debug.Log("Going to battle");
        }
    }
    public void GoToEndScreen()
    {
        selectScreen.gameObject.SetActive(false);
        battleScreen.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        gameState = GameState.EndBattle;
    }
    #endregion

    #region Hero Select Functionalities

    void HeroSelectedOnSelection(Hero heroSelected)
    {
        //Logic if list is not full, recurring clicking on hero should add/remove from list
        if (heroTeam.Count <= 3)
        {
            if(!heroTeam.Contains(heroSelected))
            {
                if (heroTeam.Count < 3)
                {
                    heroTeam.Add(heroSelected);
                    heroSelected.HeroSelected(true);
                }

            }
            else
            {
                heroTeam.Remove(heroSelected);
                heroSelected.HeroSelected(false);
            }
        }
    }


    #endregion

    #region Battle Functionalities
    /*public void SetBattleScreen()
    {
        battleScreen.gameObject.SetActive(true);
    }*/
    public void AttackButton()
    {
        if (battleHeroSelected == null)
            Debug.Log("Please select Hero to attack");
        else if (battleHeroSelected.IsDead)
        {
            Debug.Log("Please select an alive Hero to attack");
        }
        else
        {
            battleHeroSelected.Attack();
            PlayerTurnEnded();
        }
    }
    public void CheckEnd()
    {
        //Logic if game Won
        if(enemySelected.IsDead)
        {
            playerWonMatch = true;
            IncreaseTeamExperience();
            DataManager.Instance.userData.MatchesPlayed++;
            if (DataManager.Instance.userData.MatchesPlayed % 5 == 0)
            {
                GainRandomHero();
                endScreen.SetEndScreenWonHero();
                DataManager.Instance.SaveUserData();
            }
            GoToEndScreen();
        }

        else if(battleScreen.heroTeam[0].IsDead && battleScreen.heroTeam[1].IsDead && battleScreen.heroTeam[2].IsDead)
        {
            playerWonMatch = false;
            DataManager.Instance.userData.MatchesPlayed++;
            DataManager.Instance.SaveUserData();
            GoToEndScreen();
        }

    }

    void HeroSelectedOnBattle(Hero heroSelected)
    {
        battleHeroSelected = heroSelected;
         if (battleHeroSelected == battleScreen.heroTeam[0])
         {
             battleScreen.heroTeam[0].HeroSelected(true);
             battleScreen.heroTeam[1].HeroSelected(false);
             battleScreen.heroTeam[2].HeroSelected(false);
         }
         else if(battleHeroSelected == battleScreen.heroTeam[1])
         {
             battleScreen.heroTeam[0].HeroSelected(false);
             battleScreen.heroTeam[1].HeroSelected(true);
             battleScreen.heroTeam[2].HeroSelected(false);
         }
         else if (battleHeroSelected == battleScreen.heroTeam[2])
        {
            battleScreen.heroTeam[0].HeroSelected(false);
            battleScreen.heroTeam[1].HeroSelected(false);
            battleScreen.heroTeam[2].HeroSelected(true);
        }
        else
            Debug.LogWarning("Invalid Hero Selected");
    }

    #endregion

    #region End Match Functionalities
    void IncreaseTeamExperience()
    {
        foreach (Hero hero in battleScreen.heroTeam)
        {
            if(!hero.IsDead)
            {
                for (int i = 0; i<DataManager.Instance.userData.UserHeroes.Count; i++)
                {
                    if(DataManager.Instance.userData.UserHeroes[i].heroID == hero.HeroID)
                    {
                        //DataManager.Instance.userData.UserHeroes[i].experience++;
                        hero.increaseExperience();
                        SaveTeamHeroNewStats();
                    }
                }
                Debug.Log($"Hero {hero.HeroName} gained experience for winning alive");
            }

        }

    }
    [ContextMenu("GainRandomHero")]
    void GainRandomHero()
    {

        if (DataManager.Instance.userData.UserHeroes.Count<10)
        {
            //First we remove heroes we already have
            Debug.Log ("Removing heroes owned from ballot");
            var possibleHeroes = Enumerable.Range(1,10).ToList();
            for(int i =0; i< DataManager.Instance.userData.UserHeroes.Count;i++)
            {
                possibleHeroes.Remove(DataManager.Instance.userData.UserHeroes[i].heroID);
            }
            //Now we chose a new hero
            int randomHeroIndex = Random.Range(0,possibleHeroes.Count);
            
            DataManager.Instance.InitializeHero(possibleHeroes[randomHeroIndex]);
            Debug.Log ($" ID {possibleHeroes[randomHeroIndex]} initialized");
        }
    }
    //Must execute bebefore disabling battle screen
    [ContextMenu ("Save User Data")]
    void SaveTeamHeroNewStats()
    {
        for (int i = 0; i < DataManager.Instance.userData.UserHeroes.Count; i++)
        {
            for (int j= 0; j<3;j++)
            {
                //Enters if when hero used in last battle is found in User save data heroes
                if (DataManager.Instance.userData.UserHeroes[i].heroID == battleScreen.heroTeam[j].HeroID)
                {
                    DataManager.Instance.userData.UserHeroes[i].maxHealth = battleScreen.heroTeam[j].MaxHealth;
                    DataManager.Instance.userData.UserHeroes[i].attackPower = battleScreen.heroTeam[j].AttackPower;
                    DataManager.Instance.userData.UserHeroes[i].experience = battleScreen.heroTeam[j].Experience;
                    DataManager.Instance.userData.UserHeroes[i].level = battleScreen.heroTeam[j].Level;
                }

            }

        }
        DataManager.Instance.SaveUserData();
    }

    #endregion

    #region Manage Hero Selection
    public void OnHeroSelect(Hero heroSelected)
    {
            if (gameState == GameState.BattlePlayerTurn)
            {
                HeroSelectedOnBattle(heroSelected);
            }
            else if (gameState == GameState.HeroSelection)
            {
                HeroSelectedOnSelection(heroSelected);
            }

    }

    #endregion

    #region Battle Enemy Turn
    public void EnemysTurn()
    {
        enemySelected.AttackHero();
    }

    #endregion





}

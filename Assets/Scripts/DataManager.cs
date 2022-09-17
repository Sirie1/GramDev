using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    private string gameDataSaveDirectory = "/GameData/";
    private string gameDataFilename = "GameData.sav";
    private string userDataSaveDirectory = "/UserData/";
    private string userDataFilename = "UserData.sav";


    [SerializeField] GameData gameDataSave;
    [SerializeField] GameData gameDataLoad;
    public UserData userData;

    [SerializeField] bool isNewPlayer;

    public GameData GameDataLoad
    {
        get { return gameDataLoad;}
    }

    #region Singleton
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DataManager");
                go.AddComponent<DataManager>();
            }

            return _instance;
        }
    }
    #endregion

    #region Awake and start
    void Awake()
    {
        _instance = this;
        LoadGameDataResource();
        isNewPlayer = false;
        LoadUserData();
        if(isNewPlayer)
            InitializeFirstHeroes();
        SaveUserData();
    }

    #endregion

    #region Class definitions formats to save
    [Serializable]
    public class HeroStats
    {
        public int heroID;
        public string heroName;
        public Color heroColour;
        public float basemaxHealth;
        public float baseAttackPower;
        public float attributeIncreasePerLevel;
    }

    //Creation of the simplified class UserHeroProgress to be able to script to json easily
    [Serializable]
    public class UserHeroProgress
    {
        public int heroID;
        public float maxHealth;
        public float attackPower;
        public int experience;
        public int level;
    }

    [Serializable]
    public class GameData
    {
        public List<HeroStats> HeroesStats;

    }
    [Serializable]
    public class UserData
    {
        public List<UserHeroProgress> UserHeroes;
        public int MatchesPlayed;
        public int LastLevelPassed;

    }
    #endregion

    #region Hero saving and formatting functions
    public void InitializeFirstHeroes()
    {
        UserHeroProgress starterHero1 = InitializeHero(1);
        UserHeroProgress starterHero2 = InitializeHero(2);
        UserHeroProgress starterHero3 = InitializeHero(3);
    }
    //Initialize a UserHeroProgress type hero for the first time. Takes the hero ID and searchs for the appropiate attributes given the gameData for that hero ID loaded in awake.
    public UserHeroProgress InitializeHero(int heroID)
    {
        UserHeroProgress myNewHero = new UserHeroProgress();
        myNewHero.heroID = heroID;
        myNewHero.experience = 0;
        myNewHero.level = 0;

        foreach (HeroStats hero in gameDataLoad.HeroesStats)
        {
            if (hero.heroID == myNewHero.heroID)
            {
                myNewHero.maxHealth = hero.basemaxHealth;
                myNewHero.attackPower = hero.baseAttackPower;
            }
        }
        userData.UserHeroes.Add(myNewHero);
        
        
        return myNewHero;
    }
    #endregion

    #region Save and Load 
    //LoadGameData loads heroes from a json anywhere in machine
    [ContextMenu("LoadGameData")]
    public void LoadGameDataMachine()
    {
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Debug.LogWarning($"Directory {dir} was not found");
        }
        string json = File.ReadAllText(dir + gameDataFilename);
        gameDataLoad = JsonUtility.FromJson<GameData>(json);
    }
    //LoadGameData loads heroes from a json anywhere in machine
    public void LoadGameDataResource()
    { 
        TextAsset json = Resources.Load<TextAsset>("GameData/GameData");
        if (json != null)
            gameDataLoad = JsonUtility.FromJson<GameData>(json.text);
        else
            Debug.Log ("GameData could not be loaded");
    }



    [ContextMenu ("SaveGameData")]
    //SaveGameData is used when designing the heroes
    public void SaveGameData()
    {
        string dir = Application.persistentDataPath + gameDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            Debug.Log ($"Directory {dir} was created");
        }
        string json = JsonUtility.ToJson(gameDataSave, true);
        File.WriteAllText(dir + gameDataFilename, json);
    }

    //SaveUserData and LoadUserData are used to keep user data along different game sessions. For data persistance. 
    [ContextMenu("SaveUserData")]
    public void SaveUserData()
    {
        string dir = Application.persistentDataPath + userDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            Debug.Log($"Directory {dir} was created");
        }
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(dir + userDataFilename, json);
    }
    [ContextMenu("LoadUserData")]
    public void LoadUserData()
    {
        string dir = Application.persistentDataPath + userDataSaveDirectory;
        if (!Directory.Exists(dir))
        {
            Debug.LogWarning($"Directory {dir} was not found, could not load/create user data");
            return;
        }
        if (File.Exists(dir + userDataFilename))
        {
            string json = File.ReadAllText(dir + userDataFilename);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log("User data loaded");
        }
        else
        {
            Debug.Log("User Data not found");
            isNewPlayer = true;
            return;
        }

    }
    #endregion

}

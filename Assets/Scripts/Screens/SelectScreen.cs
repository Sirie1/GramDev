using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SelectScreen : MonoBehaviour
{
    public Hero heroCompPrefab;
    public GameObject HeroesListPrefab;

    [SerializeField] List<Hero> heroesChoosingList = new List<Hero>();
    public Transform heroesListTransform;


    void Start()
    {
        
    }
    private void OnEnable()
    {
        ListHeroes();
    }
    public void ListHeroes()
    {
        if (heroesChoosingList.Count > 0)
        {

            UpdateHeroes();
            //Debug.Log("List of heroes already existed, only uptdating");
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                heroesChoosingList.Add(Instantiate(heroCompPrefab, heroesListTransform));
                heroesChoosingList[i].SetHeroByID(i + 1, true);
                heroesChoosingList[i].HeroOnSelectScreen();
                heroesChoosingList[i].HeroSelected(false);
                //We need to check if we already have the hero
         
                heroesChoosingList[i].HeroInCollection(false);
                for (int j = 0; j < DataManager.Instance.userData.UserHeroes.Count; j++)
                {
                    if (DataManager.Instance.userData.UserHeroes[j].heroID == heroesChoosingList[i].HeroID )
                    {
                        heroesChoosingList[i].HeroInCollection(true);
                       // Debug.Log("Heroe was enabled");
                    }
                }

            }
           // Debug.Log("List of heroes Created");
        }

    }

    public void UpdateHeroes ()
    {
        
        for (int i = 0; i < 10; i++)
        {
            heroesChoosingList[i].SetHeroByID(i + 1, true);
            heroesChoosingList[i].HeroOnSelectScreen();
            heroesChoosingList[i].HeroSelected(false);
            heroesChoosingList[i].HeroInCollection(false);
            for (int j = 0; j < DataManager.Instance.userData.UserHeroes.Count; j++)
            {
                if (DataManager.Instance.userData.UserHeroes[j].heroID == heroesChoosingList[i].HeroID)
                {
                    heroesChoosingList[i].HeroInCollection(true);
                   // Debug.Log($"Hero {heroesChoosingList[i].HeroID} enabled to choose");
                }
            }
        }
    }
}

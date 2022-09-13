using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SelectScreen : MonoBehaviour
{
    public Hero heroCompPrefab;
    public GameObject HeroesListPrefab;

    List<Hero> heroesChoosingList = new List<Hero>();
    public Transform heroesListTransform;


    void Start()
    {
        ListHeroes();
    }
    public void ListHeroes()
    {
        for (int i= 0;i<10;i++)
        {
            heroesChoosingList.Add( Instantiate(heroCompPrefab, heroesListTransform));
            heroesChoosingList[i].SetHeroByID(i + 1, true);
            heroesChoosingList[i].HeroOnSelectScreen();
            heroesChoosingList[i].HeroSelected(false);
            //We need to check if we already have the hero
            /*
            for (int j = 0; j < DataManager.Instance.userData.UserHeroes.Count; j++)
            {
                if (DataManager.Instance.userData.UserHeroes[j].heroID == i )
                {
                    heroesChoosingList[i].HeroInCollection(true);

                }
                else
                    heroesChoosingList[i].HeroInCollection(false);
            }*/

        }
        Debug.Log("List of heroes Created");

    }

    public void UpdateHeroes ()
    {
        
        for (int i = 0; i < 10; i++)
        {
            heroesChoosingList[i].SetHeroByID(i + 1, true);
            heroesChoosingList[i].HeroOnSelectScreen();
            heroesChoosingList[i].HeroSelected(false);

        }
    }
}

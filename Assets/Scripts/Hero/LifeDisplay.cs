using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public Image liferemaining;
    public bool isHero;

    void Start()
    {
        // On start we check if we are in hero or enemy 
        CheckIfHero();
        // We initiallize the filling amount to full
        liferemaining.fillAmount = 1;
    }
    [ContextMenu ("Update fillAmount")]
    public void UpdateHealth()
    {
        CheckIfHero();
        if (isHero)
        {
            liferemaining.fillAmount = GetComponentInParent<Hero>().BattleHealth / GetComponentInParent<Hero>().MaxHealth;
            Debug.Log($"{liferemaining.fillAmount} is the fill amount");
        }
        else
            liferemaining.fillAmount = GetComponentInParent<Enemy>().BattleHealth / GetComponentInParent<Enemy>().MaxHealth;
    }

    private void CheckIfHero()
    {
        if (!(GetComponentInParent<Hero>() == null))
            isHero = true;
        else if (!(GetComponentInParent<Enemy>() == null))
            isHero = false;
        else
            Debug.LogWarning("Could not find any valid parent ");
    }
}
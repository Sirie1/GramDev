using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] Hero hero;
    [SerializeField] TextMeshProUGUI StatsText;
    // Start is called before the first frame update
    string heroName;
    float maxHealth;
    float attackPower;
    int experience;
    int level;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GetCurrentStats();
    }

    private void GetCurrentStats()
    {
        StatsText.SetText($" Hero Name: {hero.HeroName} \n Health: {hero.MaxHealth} \n Attack: {hero.AttackPower} \n Level {hero.Level}");

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stemina { get { return uiCondition.stemina; } }

    public float noHungerHealthDecay;    
    
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stemina.Add(stemina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        if(health.curValue == 0) 
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    } 
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }
    public void Die()
    {
        Debug.Log("ав╬З╢ы");
    }
}

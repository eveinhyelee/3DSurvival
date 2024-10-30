using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public PlayerController controller;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stemina { get { return uiCondition.stemina; } }

    public float noHungerHealthDecay;

    private void Awake()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        if(health.curValue == 0) 
        {
            Die();
        }
        if (stemina.curValue > 0)
        {
            if (controller.IsRun)
            {
                stemina.Subtract(stemina.passiveValue * Time.deltaTime);
            }
        }
        if (stemina.curValue == 0)
        {
            CantRun();
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
    public void Drink(float amount)
    {
        stemina.Add(amount);
    }
    public void Die()
    {
        Debug.Log("ав╬З╢ы");
    }
    public void CantRun()
    {
        controller.IsRun = false;        
    }
}

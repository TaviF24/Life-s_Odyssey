using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat 
{
    public int maxVal;
    public int currVal;

    public Stat(int maxVal, int currVal)
    {
        this.maxVal = maxVal;
        this.currVal = currVal;
    }

    internal void Substract(int amount)
    {
        currVal -= amount;
    }

    internal void Add(int amount)
    {
        currVal += amount;
        if(currVal>maxVal){
            currVal = maxVal;
        }
    }

    internal void SetToMax()
    {
        currVal=maxVal;
    }
}
public class Character : MonoBehaviour
{
    public Stat hp;
    [SerializeField] StatusBar hpBar;
    public Stat stamina;
    [SerializeField] StatusBar staminaBar;
    public bool isDead;
    private bool isExhausted;

    void Start(){
        UpdateHPBar();
        UpdateStaminaBar(); 
    }

    private void UpdateStaminaBar()
    {
        staminaBar.Set(stamina.currVal,stamina.maxVal);
    }

    private void UpdateHPBar()
    {
        hpBar.Set(hp.currVal,hp.maxVal);
    }

    public void TakeDamage(int amount)
    {
        hp.Substract(amount);
        if(hp.currVal<0){
            isDead = true;
        }
        UpdateHPBar();
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
        UpdateHPBar();
        
    }

    public void FullHeal()
    {
        hp.SetToMax(); 
        UpdateHPBar();
    }

    public void getTired(int amount)
    {
        stamina.Substract(amount);
        if(stamina.currVal<0){
            isExhausted = true;
        }
        UpdateStaminaBar();
    }  

    public void Rest(int amount)
    {
        stamina.Add(amount);
        UpdateStaminaBar();
    }

    public void FullRest()
    {
        stamina.SetToMax();
        UpdateStaminaBar();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            TakeDamage(10);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            Heal(10);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            FullHeal();
        }
    }
}

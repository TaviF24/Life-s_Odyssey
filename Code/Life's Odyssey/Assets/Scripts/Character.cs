using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if(currVal > maxVal){
            currVal = maxVal;
        }
    }

    internal void SetToMax()
    {
        currVal = maxVal;
    }
}

public class Character : MonoBehaviour
{
    public Stat hp;
    [SerializeField] StatusBar hpBar;
    public Stat stamina;
    [SerializeField] StatusBar staminaBar;
    public bool isDead;
    public bool isExhausted;

    public Vector3 respawnPosition;
    [SerializeField] Text messageText;
    private bool showingMessage;

    void Start()
    {
        messageText.enabled = false;
        UpdateHPBar();
        UpdateStaminaBar();
        StartCoroutine(HandleStamina());
    }

    private void UpdateStaminaBar()
    {
        staminaBar.Set(stamina.currVal, stamina.maxVal);
    }

    private void UpdateHPBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);
    }

    public void TakeDamage(int amount)
    {
        hp.Substract(amount);
        if (hp.currVal <= 0)
        {
            Dead();
        }
        UpdateHPBar();
    }

    private void Dead()
    {
        isDead = true;
        hp.currVal=100;
        TeleportToRespawn();
        DisplayDeathMessage();
    }

    private void TeleportToRespawn()
    {
        transform.position = respawnPosition;
    }

    private void DisplayDeathMessage()
    {
        if (messageText != null && !showingMessage)
        {
            StartCoroutine(ShowMessage(5.0f));
        }
        else
        {
            Debug.LogWarning("MessageText UI element is not assigned or message is already showing.");
        }
    }

    private IEnumerator ShowMessage( float duration)
    {
    
        messageText.enabled = true;

        yield return new WaitForSeconds(duration);

        float timer = 0.0f;
        float fadeDuration = 2.0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, timer / fadeDuration);
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure text is completely faded out
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 0.0f);

        messageText.enabled = false;
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
        UpdateHPBar();
    }

    public void getTired(int amount)
    {
        stamina.Substract(amount);
        if (stamina.currVal <= 0)
        {
            isExhausted = true;
        }
        UpdateStaminaBar();
    }

    public void Rest(int amount)
    {
        stamina.Add(amount);
        if (stamina.currVal == stamina.maxVal)
        {
            isExhausted = false;
        }
        UpdateStaminaBar();
    }

    private IEnumerator HandleStamina()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !isExhausted)
            {
                getTired(1);
            }
            else
            {
                Rest(1);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Text targetText;
    [SerializeField] Text nameText;
    [SerializeField] Image portrait;

    DialogueContainer currentDialogue;
    int currentTextLine;

    [Range(0f, 1f)]
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;
    string lineToShow;

	[SerializeField] AudioClip onInteractAudio;

	private void Update()
    {
        // show the whole text if left clicked
        if (Input.GetMouseButtonDown(0))
        {
            PushText();
        }
        TypeOutText();
    }

    private void TypeOutText()
    {
        // animate text, to show bit by bit over period of time
        if (visibleTextPercent >= 1f) { return;  }
        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0f, 1f);
        UpdateText();
    }

    void UpdateText()
    {
        // update the text on screen to show new line
        int letterCount = (int)(lineToShow.Length * visibleTextPercent);
        targetText.text = lineToShow.Substring(0, letterCount);
    }

    private void PushText()
    {
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        // check whether there is more text to show or to close dialogue
        if (currentTextLine >= currentDialogue.lines.Count)
        {
            Conclude();
        }
        else
        {
            CycleLine();
        }
    }

    void CycleLine()
    {
        // get next String text to show
        lineToShow = currentDialogue.lines[currentTextLine];
        totalTimeToType = lineToShow.Length * timePerLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;
        targetText.text = "";

        currentTextLine += 1;
    }

    public void Initialize(DialogueContainer dialogueContainer)
    {
        // when interacting with NPC
        Show(true);
        currentDialogue = dialogueContainer;
        currentTextLine = 0;
		AudioManager.instance.Play(onInteractAudio);
		CycleLine();
        UpdatePortrait();
    }

    private void UpdatePortrait()
    {
        // show NPC's portrait on bottom left dialogue canvas
        portrait.sprite = currentDialogue.actor.portrait;
        nameText.text = currentDialogue.actor.name;
    }

    private void Show(bool v)
    {
        gameObject.SetActive(v);
    }

    private void Conclude()
    {
        // close dialogue box
        Debug.Log("dialogue Ended!");
        Show(false);
    }
}

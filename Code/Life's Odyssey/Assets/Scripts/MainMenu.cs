using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string nameEssentialScene;
    [SerializeField] string nameNewGameStartScene;

    [SerializeField] PlayerData playerData;

    public Gender selectedGender;
	public TMPro.TMP_Text genderText;
	public TMPro.TMP_InputField nameInputField;

	private void Start()
	{
		SetGenderFemale();
        UpdateName();
	}

	public void StartNewGame()
    {
        SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting the game.");
        Application.Quit();
    }

    public void SetGenderMale()
    {
        selectedGender = Gender.Male;
        playerData.characterGender = selectedGender;
        genderText.text = "Male";
	}
	public void SetGenderFemale()
	{
		selectedGender = Gender.Female;
		playerData.characterGender = selectedGender;
		genderText.text = "Female";
	}

    public void UpdateName()
    {
        playerData.characterName = nameInputField.text;
    }

    public void SetSavingSlot(int num)
    {
        playerData.saveSlotId = num;
    }
}

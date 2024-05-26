using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

	public void Awake()
	{
		instance = this;
	}

	[SerializeField] ScreenTint screenTint;
	[SerializeField] CameraConfiner cameraConfiner;
	string currentScene;
	AsyncOperation unload;
	AsyncOperation load;

	void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

	public void InitSwitchScene(string sceneTo, Vector3 targetPosition)
	{
		StartCoroutine(Transition(sceneTo, targetPosition));
	}

	IEnumerator Transition(string sceneTo, Vector3 targetPosition)
	{
		screenTint.Tint();

		yield return new WaitForSeconds(1f / screenTint.speed + 0.1f); // secunde tranzitie = 1 / viteza

		SwitchScene(sceneTo, targetPosition);

		while (load != null & unload != null)
		{
			if (load.isDone) { load = null; }
			if (unload.isDone) {  unload = null; }
			yield return new WaitForSeconds(0.1f);
		}

		cameraConfiner.UpdateBounds();
		screenTint.UnTint();
	}

	public void SwitchScene(string sceneTo, Vector3 targetPosition)
	{
		load = SceneManager.LoadSceneAsync(sceneTo, LoadSceneMode.Additive);
		unload = SceneManager.UnloadSceneAsync(currentScene);
		currentScene = sceneTo;

		Transform playerTransform = GameManager.instance.player.transform;

		CinemachineBrain currentCam = Camera.main.GetComponent<CinemachineBrain>();
		currentCam.ActiveVirtualCamera.OnTargetObjectWarped(
					playerTransform,
					targetPosition - playerTransform.position
					);

		playerTransform.position =  new Vector3(
			targetPosition.x, 
			targetPosition.y,
			playerTransform.position.z
			);
	}
}

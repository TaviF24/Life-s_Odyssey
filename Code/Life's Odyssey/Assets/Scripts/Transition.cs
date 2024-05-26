using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionType
{
	Warp,
	Scene
}

public class Transition : MonoBehaviour
{
	[SerializeField] TransitionType type;
	[SerializeField] string transToSceneName;
	[SerializeField] Vector3 targetPosition;
	
	Transform destination;

	void Start()
    {
        destination = transform.GetChild(1);
    }

	internal void InitiateTransition(Transform toTransition)
	{
		switch ((type))
		{
			case TransitionType.Warp:
				Cinemachine.CinemachineBrain currentCam = 
					Camera.main.GetComponent<Cinemachine.CinemachineBrain>();

				currentCam.ActiveVirtualCamera.OnTargetObjectWarped(
					toTransition,
					destination.position - toTransition.position
					);

				toTransition.position = new Vector3(
					destination.position.x,
					destination.position.y,
					toTransition.position.z
				);
				break;
			case TransitionType.Scene:
				GameSceneManager.instance.InitSwitchScene(transToSceneName, targetPosition);
				break;
		};
	}
}

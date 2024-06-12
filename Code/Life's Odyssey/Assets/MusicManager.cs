using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance;

	private void Awake()
	{
		instance = this;
	}

	[SerializeField] AudioSource audioSource;
	[SerializeField] float timeToSwitch;

    [SerializeField] AudioClip playOnStart;

	public void Start()
	{
		Play(playOnStart, true);
	}

	public void Play(AudioClip musicToPlay, bool intrerupt = false)
    {
		if (musicToPlay == null) { return; }

		if (intrerupt) 
		{
			audioSource.volume = 1f;
			audioSource.clip = musicToPlay;
			audioSource.Play();
		} else
		{
			switchTo = musicToPlay;
			StartCoroutine(SmoothSwitchMusic());
		}
		
    }

	AudioClip switchTo;
	float volume;
	IEnumerator SmoothSwitchMusic()
	{
		volume = 1f;
		while (volume > 0f)
		{
			volume -= Time.deltaTime / timeToSwitch;
			if (volume < 0f)
			{
				volume = 0f;
			}
			audioSource.volume = volume;
			yield return new WaitForEndOfFrame();
		}
		Play(switchTo, true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceRegister : MonoBehaviour {

	private AudioSource audioSource;

	public AudioMusicSO musicEvent;

	void Awake () 
	{
		// si es el primer elemento que aparece, crea el diccionario
		if(AudioManager.musicAudioSources==null)
		{
			AudioManager.musicAudioSources = new Dictionary<AudioManager.PossibleMusics,AudioSource> ();
		}

		if(audioSource=GetComponent<AudioSource>())
		{
			audioSource.clip = musicEvent.audioClip;
			if(!AudioManager.musicAudioSources.ContainsKey(musicEvent.possibleMusic))
			{
				AudioManager.musicAudioSources.Add(musicEvent.possibleMusic,audioSource);	
			}
		}
	}
}

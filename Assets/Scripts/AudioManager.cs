using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
	[Serializable]
	public struct Audio
	{
		public string tag;
		public AudioClip sound;
	}

	public static AudioManager Instance { get { return instance; } }
	private static AudioManager instance;

	[SerializeField]
	private AudioSource AudioSource;
	[SerializeField]
	private AudioSource SoundSource;

	public bool SoundOn;
	public bool AudioOn;

	[SerializeField]
	private List<Audio> Audios;
	[SerializeField]
	private List<Audio> Sounds;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		AudioOn = true;
		SoundOn = true;

		PlayAudio("Main");
	}

	public void PlaySound(string audioTag)
	{
		if (SoundOn)
			AudioSource.PlayOneShot(Sounds.Find(a => a.tag == audioTag).sound);
	}

	public void PlayLoopSound(string audioTag)
	{
		if (SoundOn && !SoundSource.isPlaying)
		{
			SoundSource.clip = Sounds.Find(a => a.tag == audioTag).sound;
			SoundSource.Play();
		}
	}

	public void StopLoopSound()
	{
		SoundSource.Stop();
		SoundSource.clip = null;
	}

	public void PlayAudio(string audioTag)
	{
		if (AudioOn)
		{
			AudioSource.clip = Audios.Find(a => a.tag == audioTag).sound;
			AudioSource.Play();
		}
	}

	public void PauseAudio()
	{
		AudioSource.Pause();
	}

	public void ResumeAudio()
	{
		AudioSource.UnPause();
	}

	public void StopAudio()
	{
		AudioSource.Stop();
		AudioSource.clip = null;
	}
}

using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

// https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
    public AudioMixer masterMixer;
    public AudioMixer soundEffectsMixer;
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup soundEffectsMixerGroup;
    public float FadeTime = 1;

	public Sound[] Music;
    public Sound[] SoundEffects;
    private Sound CurrentSong;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in Music)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = masterMixerGroup;
		}

        foreach (Sound s in SoundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
        }
    }

    private void Start()
    {
        PlaySong("Track1");
    }

    public void PlaySong(string sound)
	{
		Sound s = Array.Find(Music, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        if(CurrentSong != null)
        {
            StartCoroutine(FadeChange(CurrentSong, s, FadeTime));
        }
        else
        {
		    s.source.Play();
        }

        CurrentSong = s;
	}

    public void PlaySoundEffect(string sound)
    {
        Sound s = Array.Find(SoundEffects, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        s.source.PlayOneShot(s.clip);
    }

    // Source: https://forum.unity.com/threads/fade-out-audio-source.335031/
    public IEnumerator FadeChange(Sound sound1, Sound sound2, float FadeTime)
    {
        // Fade out
        float startVolume = sound1.source.volume;

        while (sound1.source.volume > 0)
        {
            sound1.source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        sound1.source.Stop();
        sound1.source.volume = startVolume;

        // Fade in
        float endVolume = sound2.source.volume;
        sound2.source.Play();

        while (sound2.source.volume < endVolume)
        {
            sound2.source.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }

    public void SetMusicVolumeLevel(float value)
    {
        masterMixer.SetFloat("Volume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }

    public void SetSoundEffectsVolumeLevel(float value)
    {
        soundEffectsMixer.SetFloat("Volume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }
}

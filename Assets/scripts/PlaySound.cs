using UnityEngine;
using System.Collections;

/// <summary>
/// For creating AudioSources in temporary objects at a given transform.
/// </summary>
public class PlaySound : MonoBehaviour
{
	public AudioClip sound;
	[Tooltip("The name of a key press. See Edit->Project Settings->Input->Axes. Example: Fire1")]
	public string buttonPress;
	[Tooltip("Will make the noise restart after it ends. Used for looping music.")]
	public bool loop = false;
	/// <summary>used to check if a looping sound has started</summary>
	bool played = false;
	[Tooltip("How loud to make this noise. Should be between 0 and 1, with 0 off, and 1 at max vlume.")]
	public float volume = 1;
	
	void Start()
	{
		if (buttonPress == null || buttonPress.Length == 0)
		{
			Play(sound, transform, loop, volume);
		}
	}
	
	void Update()
	{
		// if the button was pressed
		if (buttonPress != null && buttonPress.Length > 0 && Input.GetButtonDown(buttonPress)
		// and this noise is not currently looping
		&& (!loop || !played))
		{
			Play(sound, transform, loop, volume);
			played = true;
		}
	}
	
	/// <summary>
	/// Play the specified AudioClip, at the given location.
	/// </summary>
	public static AudioSource Play(AudioClip ac, Transform emitter)
	{
		return Play (ac, emitter, false, 1);
	}

	/// <summary>
	/// Play the specified AudioClip, at the given location. can be used to loop music as well.
	/// </summary>
	public static AudioSource Play(AudioClip ac, Transform emitter, bool loop, float volume)
	{
		if (ac == null)
		{
			print ("can't play sound!");
			return null;
		}
		GameObject go = new GameObject("sound: " + ac.name);
		AudioSource asrc = go.AddComponent<AudioSource>();
		asrc.loop = loop;
		asrc.clip = ac;
		asrc.Play();
		asrc.volume = volume;
		if(!loop) {
			Destroy(go, ac.length);
		}
		if (emitter != null)
		{
			go.transform.position = emitter.transform.position;
			go.transform.parent = emitter.transform.parent;
		}
		return asrc;
	}
}
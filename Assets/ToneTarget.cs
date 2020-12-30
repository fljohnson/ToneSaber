using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

public class ToneTarget : MonoBehaviour
{
	public static string[] keys = {
		"C",
		"C#",
		"D",
		"D#",
		"E",
		"F",
		"F#",
		"G",
		"G#",
		"A",
		"A#",
		"B"
	};
	
	public static float qnLength = 1f/(120f/60f); //seconds per quarter note, assuming 120 quarter notes/minute
	
	public static ArrayList materials = new ArrayList();
	private int keynum;
	private float duration; //counts or seconds?
	
	public string keyAndOctave ="C5";
	public int xthNote = 4;
	public int points = 10;
	private float tMinus = -1f;
	private AudioClip noteSound;
	private AudioSource notePlayer;
	private ParticleSystem glitters;
	private bool shuttingDown =false;
	private bool userDestroy = false;
	public int toneTargetThreshold = 4;


	// Start is called before the first frame update
	void Start()
    {
		
		materials.Add(Resources.Load("Materials/Eighthnote") as Material);
		materials.Add(Resources.Load("Materials/Quarternote") as Material);
		materials.Add(Resources.Load("Materials/Halfnote") as Material);
		glitters = GetComponent<ParticleSystem>();
        notePlayer = GetComponent<AudioSource>();
        SetKeyAndOctave(keyAndOctave);
        SetDuration(xthNote);
		AkSoundEngine.SetSwitch("Music", "jazz", Music);
	}

    // Update is called once per frame
    void Update()
    {
        if(tMinus > 0f) {
			Decrement();
		}
		if(shuttingDown) {
			if(glitters.isStopped) {
				Destroy(gameObject);
                ScoreManager.increaseScore(1);
				Debug.Log("Curr Score: " + ScoreManager.getScore());
				CheckfortoneTargetHit();
				
			}
		}
    }

	

    
    private void OnDestroy()
    {
        if (userDestroy) {
            Debug.Log("destory even test");
            //event here
            //AkSoundEngine.PostEvent("blockbreak", gameObject);

        }

	}

    public void SetKeyAndOctave(string setting) {
		string intermediate = setting.ToUpper();
		
		noteSound = Resources.Load("Sounds/"+intermediate) as AudioClip;
		//problem 1:break out the key and the octave
		//A piano allows us to assume the octave number to be 0-8 inclusive, so
		string key = intermediate.Substring(0,intermediate.Length-1);
		
		keynum = -1;
		if(int.TryParse(intermediate.Substring(intermediate.Length-1), out int octave)){
			 keynum = octave*12;
		}
		Assert.IsTrue(keynum > 0, "Oh crap");
		int tail = Array.IndexOf(keys,key);
		Assert.IsTrue(tail> -1, "Invalid key name");
		keynum+=(tail+1); 
	}
	
	public void SetDuration(int fraction) {
		Renderer drawer = GetComponent<Renderer>();
		switch(fraction) {
			case 8:
				drawer.material = materials[0] as Material;
				break;
			case 4:
				drawer.material = materials[1] as Material;
				break;
			case 2:
				drawer.material = materials[2] as Material;
				break;
			default:
				Assert.IsTrue(false,"Got "+fraction.ToString()+"th note request");
				break;
		}
		duration = qnLength*(4f/fraction);
		//var  mein = glitters.main;
		//mein.duration = duration;
	}

	public GameObject Music;
	public void CheckfortoneTargetHit()
	{
		if (ScoreManager.getScore() > toneTargetThreshold)
		{
			AkSoundEngine.SetSwitch("Music", "jazz", Music);

		}
		if (ScoreManager.getScore() >= toneTargetThreshold)
		{
			//fix this
			AkSoundEngine.SetSwitch("Music", "rock", Music);

		}
	}

	public void Play() {
		if(shuttingDown) {
			return;
		}
		GetComponent<Renderer>().enabled = false; //hide the geometry
		glitters.Play(); //show the bubble spray
		shuttingDown = true;
		tMinus = duration;
		//Run particle system
		if(noteSound != null) {
			notePlayer.clip = noteSound;
			notePlayer.Play();
			AkSoundEngine.PostEvent("blockbreak", gameObject);
			//Debug.Log("destory even test");
		}
	}

	public void setUserDestroy(bool i)
    {
		userDestroy = i;
		
    }

	

	private void Decrement() {
		tMinus -= Time.deltaTime;
		if(tMinus <= 0f) {
			if(noteSound != null)
				notePlayer.Stop();
		}
	}
}

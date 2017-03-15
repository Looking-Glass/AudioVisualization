using UnityEngine;
using System.Collections;

public class MicrophoneInput : MonoBehaviour {

//	// Use this for initialization
//	public AudioClip[] audioClips;
//	public AudioSource _as;
//	private int _index = 0;
//	void Start () {
//		foreach (string device in Microphone.devices) {
//			Debug.Log("Name: " + device);
//		}
//		SetSource ();
//	}
//	
//	// Update is called once per frame
//	void Update () {
////		if (Input.GetKeyDown(KeyCode.N)) {
////			_index++;
////			SetSource ();
////		}
//	}
//	void SetSource(){
//		_as.Stop();
//		if (_index > audioClips.Length ) {
//			_index = 0;
//		}
//		if (_index == 0) {
//			Microphone.End ("Built-in Microphone");
//			_as.clip = Microphone.Start ("Built-in Microphone", true, 10, 44100);
//			_as.Play ();
//		} else {
//			Microphone.End ("Built-in Microphone");
//			_as.clip = audioClips[_index-1];
//			_as.Play ();
//		}
//	}

	public AudioSource audioSauce;
	public string CurrentAudioInput = "none";
	int deviceNum = 0;

	void Start()
	{

		string[] inputDevices = new string[Microphone.devices.Length];
		deviceNum = 0;

		for (int i = 0; i < Microphone.devices.Length; i++) {
			inputDevices [i] = Microphone.devices [i].ToString ();
			if (inputDevices [i].Contains("Built-in")) {
				deviceNum = i;
			}
			Debug.Log("Device: " + inputDevices [i]);
		}
		CurrentAudioInput = Microphone.devices[deviceNum].ToString();
		StartMic ();
		AudioListener.volume = 1;
		AudioListener.pause = false;
	}

	public const float freq = 24000f;

	public void StartMic(){
		audioSauce.clip = Microphone.Start(CurrentAudioInput, true, 5, (int) freq); 
	}

//	public void OnGUI(){
//		GUI.Label (new Rect (10, 10, 400, 400), CurrentAudioInput);
//	}

	const int WINDOW_SIZE = 1<<13;

	public float[] spectrum = new float[WINDOW_SIZE];

	void Update() {
//		if (Input.GetKeyDown (KeyCode.Equals)) {
//			Microphone.End (CurrentAudioInput);
//			deviceNum += 1;
//			if (deviceNum > Microphone.devices.Length - 1)
//				deviceNum = 0;
//			CurrentAudioInput = Microphone.devices [deviceNum].ToString ();
//
//			StartMic ();
//		}
		if (Input.GetKeyDown (KeyCode.A)) {
			audioSauce.Play ();
		}

		float delay = 0.030f;
		int microphoneSamples = Microphone.GetPosition (CurrentAudioInput);
		Debug.Log ("Current samples: " + microphoneSamples);
		if (microphoneSamples / freq > delay) {
			if (!audioSauce.isPlaying) {
				Debug.Log ("Starting thing");
				audioSauce.timeSamples = (int)(microphoneSamples - (delay * freq));
				audioSauce.Play ();
//				audioSauce.mute = true;
			}
		}
		audioSauce.GetSpectrumData (spectrum, 0, FFTWindow.Hanning);
//		int i = 1;
//		while (i < WINDOW_SIZE) {
//			Debug.DrawLine (new Vector3 (i - 1, 50000f * spectrum [i - 1] + 10, 0), 
//				new Vector3 (i, 50000f * spectrum [i] + 10, 0), 
//				Color.red);
//			Debug.DrawLine (new Vector3 (i - 1, Mathf.Log (spectrum [i - 1]) + 10, 2),
//				new Vector3 (i, Mathf.Log (spectrum [i]) + 10, 2),
//				Color.cyan);
//			Debug.DrawLine (new Vector3 (Mathf.Log (i - 1), spectrum [i - 1] - 10, 1), 
//				new Vector3 (Mathf.Log (i), spectrum [i] - 10, 1), 
//				Color.green);
//			Debug.DrawLine (new Vector3 (Mathf.Log (i - 1), Mathf.Log (spectrum [i - 1]), 3), 
//				new Vector3 (Mathf.Log (i), Mathf.Log (spectrum [i]), 3), 
//				Color.yellow);
//			i++;
//		}
	}
}

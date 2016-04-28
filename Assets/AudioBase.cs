using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

public class AudioBase : MonoBehaviour {

	public float volumeHighest = 0.001f;

	public int dec = 128;
	public List<float> waveData = new List<float> ();



	public float high_volumeHighest = 0.001f;
	public List<float> highwaveData = new List<float> ();


	public float base_volumeHighest = 0.001f;
	public List<float> basewaveData = new List<float> ();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void UpdateData (float[] musicData) {
		if (waveData.Count < dec) {
			waveData.Add (musicData [0]);
		} else {
			waveData.RemoveAt (0);
			waveData.Add (musicData [0]);
		}
		volumeHighest = waveData.Max ();
		if (volumeHighest< 0.05f) {
			//0.01
			volumeHighest = 0.05f;
		}

	}

	public void UpdateHighData (float[] highData) {
		if (highwaveData.Count < dec) {
			highwaveData.Add (highData [0]);
		} else {
			highwaveData.RemoveAt (0);
			highwaveData.Add (highData [0]);
		}
		high_volumeHighest = highwaveData.Max ();
		if (high_volumeHighest< 0.05f) {
			//0.01
			high_volumeHighest = 0.05f;
		}

	}

	public void UpdateBassData (float[] baseData) {
		if (basewaveData.Count < dec) {
			basewaveData.Add (baseData [0]);
		} else {
			basewaveData.RemoveAt (0);
			basewaveData.Add (baseData [0]);
		}
		base_volumeHighest = baseData.Max ();
		if (base_volumeHighest< 0.05f) {
			//0.01
			base_volumeHighest = 0.05f;
		}

	}
}

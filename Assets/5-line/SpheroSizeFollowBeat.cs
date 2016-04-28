using UnityEngine;
using System.Collections;
using AudioVisualizer;
using DG.Tweening;
public class SpheroSizeFollowBeat : MonoBehaviour {

	// Use this for initialization
	private ParticleSystem ps;
	void Start () {
		ps = gameObject.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		float[] musicData;
		musicData = AudioSampler.instance.GetAudioSamples (0, 1, true);
//		
////		Debug.Log (musicData [0]*10);
		ps.startSize = Mathf.Clamp(musicData [0]*20,1,10);
//
//				ps.startLifetime = 10;
//		float adjust = Mathf.Clamp(musicData [0]*10,3,10);
//		transform.localScale = Vector3.one * musicData[0]*5;
//		transform.DOScale (Vector3.one * musicData [0] * 5, 0.2f).SetEase (Ease.OutCubic);
	}
}

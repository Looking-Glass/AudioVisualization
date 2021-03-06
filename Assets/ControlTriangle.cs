﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using AudioVisualizer;

public class ControlTriangle : AudioBase {
	public FrequencyRange frequencyRangeDecibal_ = FrequencyRange.Decibal;
	public FrequencyRange frequencyRangeHigh = FrequencyRange.High;
	public FrequencyRange frequencyRangeBass = FrequencyRange.Bass;
	public Transform volumeContainer;
	private List<Transform> volumeGO = new List<Transform>();
	public Transform highPitchContainer;
	private List<Transform> highPitchGO = new List<Transform>();
	public Transform bassContainer;
	private List<Transform> bassGO = new List<Transform>();
	public Transform camera;
	private Vector3 cameraOriginalPos;
	public Transform triangleLight;
	private int direction = 1;
	private Transform hypercubeContainer;
	private Transform hypercubeCamera;
	public Vector3 cameraTargetAngle  = Vector3.zero;
	private Vector3 defaultScale = new Vector3 (2.5f, 2, 7f);
	float heightMax = 3f;
	float heightMin = 0.5f;
	float originalHeight = 1;

	float easetime = 0.5f;
	public Transform center;
//	public Color[] colors;
	void Start(){
		volumeGO.Clear ();
		volumeGO = volumeContainer.GetComponentsInChildren<Transform> ().ToList();
		highPitchGO.Clear ();
		highPitchGO = highPitchContainer.GetComponentsInChildren<Transform> ().ToList();
		bassGO.Clear ();
		bassGO = bassContainer.GetComponentsInChildren<Transform> ().ToList();
	}
	void OnEnable(){

//		hypercubeContainer = Controller.instance.mainCamera;
//		hypercubeCamera = Controller.instance.mainCamera;
	}
	int angle = 0;
	void Update(){
//		if (hypercubeContainer == null) {
//			return;
//		}
//		if (Input.GetKeyDown(KeyCode.U)) {
//			MoveCamera ();
//		}
//		if (hypercubeContainer.transform.parent != camera) {
//			hypercubeContainer.transform.parent = camera;
//			hypercubeContainer.localPosition = new Vector3 (0.02f, 0, 3.69f);
//			hypercubeCamera.localScale = defaultScale;
//			hypercubeContainer.localEulerAngles = Vector3.zero;
			cameraOriginalPos = camera.localPosition;
//		}

//		hypercubeCamera.localEulerAngles = Vector3.zero;
		float radius = 0.5f;
		angle+=3;
		Controller.instance.mainCamera.localPosition = new Vector3 (cameraOriginalPos.x, cameraOriginalPos.y + radius * Mathf.Cos (angle * Mathf.Deg2Rad), cameraOriginalPos.z + radius * Mathf.Sin (angle * Mathf.Deg2Rad));
//		Controller.instance.mainCamera.transform.LookAt(center);
		Controller.instance.mainCamera.localEulerAngles = new Vector3(0,angle,0);
		float[] musicData_Decibal;
		float[] musicData_high;
		float[] musicData_bass;

		musicData_Decibal = AudioSampler.instance.GetAudioSamples (AudioSampler.instance.audioSource, 128, true);
//		musicData_high = AudioSampler.instance.GetFrequencyData (AudioSampler.instance.audioSource, frequencyRangeHigh, 128, true);
//		musicData_bass = AudioSampler.instance.GetFrequencyData (AudioSampler.instance.audioSource, frequencyRangeBass, 128, true);
		UpdateData (musicData_Decibal);
//		UpdateHighData (musicData_high);
//		UpdateBassData (musicData_bass);


//		Debug.Log ("musicData_Decibal [0]=" + musicData_Decibal [0]);
//		Debug.Log ("volumeHighest=" + volumeHighest);
		float beat = MathTool.Remap(musicData_Decibal [0],0,volumeHighest,0,1f);
//		if (beat > 0.5f) {
////			MoveCamera ();
//			LightRotate ();
//		} else if (beat > 0.8f) {
//			RotateCamera ();
//		}
		if (beat > 0.8f) {
//			RotateCamera ();
		}

//		if (float.IsNaN (volumeHighest)) {
//			return;
//		}
		int i = 0;
		while (i < volumeGO.Count) {
			float height = MathTool.Remap(musicData_Decibal [i],0,volumeHighest,heightMin,heightMax);
			volumeGO[i].transform.DOScaleY(height,easetime).SetEase(Ease.OutCubic);
			i++;
		}
//		i = 0;
//		while (i < highPitchGO.Length) {
//			float height = MathTool.Remap(musicData_high [i],0,high_volumeHighest,heightMin,heightMax);
//			highPitchGO[i].transform.DOScaleY(height,easetime).SetEase(Ease.OutCubic);
//			i++;
//		}
//		Debug.Log("volumeHighest="+volumeHighest);
//		i = 0;
//		while (i < bassGO.Length) {
//			float height = MathTool.Remap(musicData_bass [i],0,base_volumeHighest,heightMin,heightMax);
//			bassGO[i].transform.DOScaleY(height,easetime).SetEase(Ease.OutCubic);
//			i++;
//		}
	}

	void LightRotate(){
		triangleLight.DOLocalRotate (new Vector3 (0, triangleLight.localEulerAngles.y + 360, 0), 1, RotateMode.FastBeyond360).SetEase (Ease.OutCubic);
	}
	void RotateCamera(){

		direction*=-1;
		//			float angle = Mathf.Sign(Random.Range(-5,5)) * Random.Range(100,150);
		float angle = Random.Range(0,360);
		camera.localEulerAngles = new Vector3(0,angle,0);
	}
	void MoveCamera(){
		Debug.Log ("MoveCamera");
		DOTween.Kill (hypercubeCamera.transform);
		hypercubeCamera.transform.localScale = defaultScale;
		hypercubeCamera.transform.DOScale(defaultScale*0.6f,0.3f).SetEase (Ease.OutCubic).SetLoops(2,LoopType.Yoyo);
//		camera.transform.localPosition = cameraOriginalPos;
//		camera.transform.DOLocalMoveY (cameraOriginalPos.y + 2f, 0.2f).SetEase (Ease.OutCubic).SetLoops(2,LoopType.Yoyo);
//		camera.transform.DOLocalRotate (new Vector3(cameraTargetAngle.x+45.5f,cameraTargetAngle.y,cameraTargetAngle.z), 0.2f).SetEase (Ease.OutCubic).SetLoops(2,LoopType.Yoyo);
	}
	public void HighBeat(){
		int i = 0;
		while (i < highPitchGO.Count) {

			DOTween.Kill (highPitchGO [i].transform);
			highPitchGO [i].transform.localScale = new Vector3 (highPitchGO [i].transform.localScale.x, originalHeight, highPitchGO [i].transform.localScale.z);
			highPitchGO[i].transform.DOScaleY((heightMin+heightMax)/2,easetime).SetEase(Ease.OutCubic).SetLoops(2,LoopType.Yoyo);
			i++;
		}
	}
	public void BaseBeat(){
		int i = 0;
		while (i < bassGO.Count) {
			DOTween.Kill (bassGO [i].transform);
			bassGO [i].transform.localScale = new Vector3 (bassGO [i].transform.localScale.x, originalHeight, bassGO [i].transform.localScale.z);
			bassGO[i].transform.DOScaleY((heightMin+heightMax)/2,easetime).SetEase(Ease.OutCubic).SetLoops(2,LoopType.Yoyo);
			i++;
		}
	
	}
}

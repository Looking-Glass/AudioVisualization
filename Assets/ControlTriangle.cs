using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using AudioVisualizer;

public class ControlTriangle : AudioBase {
	public FrequencyRange frequencyRangeDecibal_ = FrequencyRange.Decibal;
	public FrequencyRange frequencyRangeHigh = FrequencyRange.High;
	public FrequencyRange frequencyRangeBass = FrequencyRange.Bass;

	public GameObject[] volumeGO;
	public GameObject[] highPitchGO;
	public GameObject[] bassGO;
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
	void Start(){
	}
	void OnEnable(){

		hypercubeContainer = Controller.instance.hypercubeContainer;
		hypercubeCamera = Controller.instance.mainCamera;
	}
	void Update(){
		if (hypercubeContainer == null) {
			return;
		}
		if (Input.GetKeyDown(KeyCode.U)) {
			MoveCamera ();
		}
		if (hypercubeContainer.transform.parent != camera) {
			hypercubeContainer.transform.parent = camera;
			hypercubeContainer.localPosition = new Vector3 (0.02f, 0, 3.69f);
			hypercubeCamera.localScale = defaultScale;
			hypercubeContainer.localEulerAngles = Vector3.zero;
			cameraOriginalPos = camera.localPosition;
		}

		hypercubeCamera.localEulerAngles = Vector3.zero;
		camera.localEulerAngles += new Vector3(0,direction*0.5f,0);
//		cameraTargetAngle = camera.localEulerAngles + new Vector3(0,direction*0.5f,0);
//		cameraTargetAngle.x = 0;
//		cameraTargetAngle.z = 0;
//		camera.localEulerAngles = cameraTargetAngle;

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
			RotateCamera ();
		}

//		if (float.IsNaN (volumeHighest)) {
//			return;
//		}
		int i = 0;
		while (i < volumeGO.Length) {
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
		while (i < highPitchGO.Length) {

			DOTween.Kill (highPitchGO [i].transform);
			highPitchGO [i].transform.localScale = new Vector3 (highPitchGO [i].transform.localScale.x, originalHeight, highPitchGO [i].transform.localScale.z);
			highPitchGO[i].transform.DOScaleY((heightMin+heightMax)/2,easetime).SetEase(Ease.OutCubic).SetLoops(2,LoopType.Yoyo);
			i++;
		}
	}
	public void BaseBeat(){
		int i = 0;
		while (i < bassGO.Length) {
			DOTween.Kill (bassGO [i].transform);
			bassGO [i].transform.localScale = new Vector3 (bassGO [i].transform.localScale.x, originalHeight, bassGO [i].transform.localScale.z);
			bassGO[i].transform.DOScaleY((heightMin+heightMax)/2,easetime).SetEase(Ease.OutCubic).SetLoops(2,LoopType.Yoyo);
			i++;
		}
	
	}
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

using AudioVisualizer;

public class CityController : AudioBase {
	public Transform wholeContainer;
	public int cubesNum = 20;
	public GameObject[] cityCube;
//	public Material[] cityCube_mats;
	public Transform cityContainer;
	private List<GameObject> cityCubes = new List<GameObject>();
	public float speed_multiplier = 1.0f;//Range 0~10
	private float targetSpeed =0;
	public static float multiplier_tiltX = -5.0f;//Range -10~-5
	public float multiplier_tiltY = 0.0f;//Range -10~10
	private float target_multiplier_tiltY = 0;
	private float multiplier_tiltZ = 0.0f;//Range -10~10
	private float targetAngleZ = 0;

	public FrequencyRange tiltX_frequency = FrequencyRange.High; // what frequency will we listen to? 
	public FrequencyRange tiltY_frequency = FrequencyRange.Bass; // what frequency will we listen to? 

	public float speedZ = 1;
	public float speedX = 1;
	public Material planeMat;
	private float _texSpeedZ = 0.015f;
	private float texSpeedZ = 0.015f;
	public float tex_tilingX = 3;

	public Color[] custumColorList;
	public  Color targetColor;
	public Color currentColor;

	void InitCubes(){
//		for (int i = 0; i < cubesNum; i++) {
//			CreateCity ();
//		}
	}
	void OnEnable(){
		tex_tilingX = planeMat.GetTextureScale ("_MainTex").x;
		texSpeedZ = _texSpeedZ * tex_tilingX;
		InitCubes ();
//		cityCubes = cityContainer.GetComponentsInChildren<CityCube> ().ToList();
	}
	void OnDisable(){
		cityCubes.Clear ();
		Transform[] childs = cityContainer.GetComponentsInChildren<Transform> ();
		foreach(Transform child in childs) {
			if (child!=cityContainer) {
				Destroy(child.gameObject);
			}
		}
		wholeContainer.localEulerAngles = Vector3.zero;
	}
	// Update is called once per frame
	void Update () {
		float[] musicData_Decibal;
		musicData_Decibal = AudioSampler.instance.GetAudioSamples (AudioSampler.instance.audioSource, 128, true);
		UpdateData (musicData_Decibal);
		float beat = MathTool.Remap(musicData_Decibal [0],0,volumeHighest,0,1f);
		if (beat>0.5f) {
			targetSpeed = 3;
			CreateCity2 ();
//			multiplier_tiltX = -10;
		}
		speed_multiplier += (targetSpeed - speed_multiplier) / 50f;

		multiplier_tiltY += (target_multiplier_tiltY - multiplier_tiltY) / 100;	
		multiplier_tiltZ = multiplier_tiltY * -1f;

		Vector2 offset = planeMat.GetTextureOffset ("_MainTex");
		planeMat.SetTextureOffset("_MainTex", new Vector2(offset.x + texSpeedZ * speed_multiplier , 0));

//		wholeContainer.transform.localEulerAngles = new Vector3 (multiplier_tiltX, multiplier_tiltY, multiplier_tiltZ);

		wholeContainer.transform.localEulerAngles = new Vector3 (multiplier_tiltX, 0, 0);
		Controller.instance.mainCamera.localEulerAngles = new Vector3(0,multiplier_tiltY, multiplier_tiltZ);
		targetSpeed -= 0.01f;
		if (targetSpeed <0) {
			targetSpeed = 0;
		}
		target_multiplier_tiltY += (0 - target_multiplier_tiltY) / 50;

		currentColor += (targetColor - currentColor) / 20;

		for(var i = cityCubes.Count - 1; i > -1; i--)
		{
			if (cityCubes[i] == null)
				cityCubes.RemoveAt(i);
		}

		for (int i = 0; i < cityCubes.Count; i++) {
			GameObject cube = cityCubes [i] as GameObject;
			if (cube != null) {
				cube.transform.localPosition += new Vector3 (cube.transform.localPosition.x * speedX * speed_multiplier, 0, speedZ * speed_multiplier);
				if (cube.transform.localPosition.z < -7) {
					Destroy (cube);
				}
			}

		}

	}
	void ResetCube(GameObject cube){
		cube.transform.localPosition = new Vector3 (Random.Range (-1.5f,1.5f),-1.75f, Random.Range(4,15));
		cube.transform.localEulerAngles = new Vector3 (0, Random.Range(0,360), 0);
		float size = Random.Range(0.5f,1.5f);
		cube.transform.localScale = new Vector3(size,Random.Range(1f,8f),size);
		Material[] mats = cube.GetComponent<Renderer> ().materials;
		foreach (Material mat in mats) {
			mat.color = currentColor;
		}
	}
	public void ChangeMultiplier_TiltX(float value){
		DOTween.Kill ("multiplier_tiltX");
		if (targetSpeed > 0.5f) {
			DOTween.To (() => multiplier_tiltX, x => multiplier_tiltX = x, value, 5f).SetEase (Ease.OutCubic);
		}
		DOTween.To (() => multiplier_tiltX, x => multiplier_tiltX = x, -5, 5f).SetEase (Ease.OutCubic).SetDelay (1.5f);
	}
	public void ChangeMultiplier_Tilty(float value){
		if (!System.Single.IsNaN (value)) {
			if (targetSpeed > 1f) {
				target_multiplier_tiltY = Mathf.Sign(Random.Range(-1,1))*value;
			}
//			Debug.Log ("BaseHandler=" + value);

		}
//		DOTween.To (()=> multiplier_tiltX, x=>multiplier_tiltX =x, Random.Range(-1,1)*, 0.2f).SetEase (Ease.InOutBounce).SetLoops(2,LoopType.Yoyo);
//		DOTween.To (multiplier_tiltY, multiplier_tiltY, -10, 0.2f).SetEase (Ease.InOutBounce).SetLoops(2,LoopType.Yoyo);
	}

	public void ChangeSpeedMultiplier(float value){
//		if (!System.Single.IsNaN (value)) {
//			Debug.Log ("BaseHandler=" + value);
//			targetSpeed = value;
//		}
		//		DOTween.To (()=> multiplier_tiltX, x=>multiplier_tiltX =x, Random.Range(-1,1)*, 0.2f).SetEase (Ease.InOutBounce).SetLoops(2,LoopType.Yoyo);
		//		DOTween.To (multiplier_tiltY, multiplier_tiltY, -10, 0.2f).SetEase (Ease.InOutBounce).SetLoops(2,LoopType.Yoyo);
	}
	public void ChangeColor(int colorIndex){
		targetColor = custumColorList[colorIndex];
	}

	public void CreateCity(){
//		if (targetSpeed > 0.4f) {
//			GameObject cube = Instantiate (cityCube [Random.Range (0, cityCube.Length)]) as GameObject;
//			cube.transform.parent = cityContainer;
//			cube.transform.localPosition = new Vector3 (Random.Range (-1.5f, 1.5f), -1.75f, 4.5f);
//			cube.transform.localEulerAngles = new Vector3 (0, Random.Range (0, 360), 0);
//			float size = Random.Range (0.5f, 1.5f);
//			cube.transform.localScale = new Vector3 (size, Random.Range (1f, 8f), size);
//			Material[] mats = cube.GetComponent<Renderer> ().materials;
//			foreach (Material mat in mats) {
//				mat.color = currentColor;
//			}
//			cityCubes.Add (cube);
//		}
	}
	public void CreateCity2(){
		GameObject cube = Instantiate (cityCube [Random.Range (0, cityCube.Length)]) as GameObject;
		cube.transform.parent = cityContainer;
		cube.transform.localPosition = new Vector3 (Mathf.Sign(Random.Range(-10,10))*Random.Range (0.5f, 1.5f), -1.75f, 6f);
		cube.transform.localEulerAngles = new Vector3 (0, Random.Range (0, 360), 0);
		float size = Random.Range (0.5f, 1.5f);
		cube.transform.localScale = new Vector3 (size, Random.Range (1f, 8f), size);
		Material[] mats = cube.GetComponent<Renderer> ().materials;
		foreach (Material mat in mats) {
			mat.color = currentColor;
		}
		cityCubes.Add (cube);
	}
}

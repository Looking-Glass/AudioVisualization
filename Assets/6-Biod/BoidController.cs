using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using AudioVisualizer;

namespace BoidBehavior{
	public class BoidController : AudioBase {
		public FrequencyRange frequencyRange = FrequencyRange.Decibal;

		public GameObject boidPrefab;
		public Transform boidContainer;
		public int boidNum = 10;
		private List<Boid> boidList = new List<Boid>();
		private Transform _camera;
		public bool cameraFollow = false;
		private int cameraFollowTarget = 0;
		public Transform targetfollow;
//		public Transform[] path;
//		private int pathPoint = 0;
		private int dir = 1;
		private bool forceStop = false;
		private int forceStopCount = 0;
		void Start () {
			

		}
		void OnEnable(){
			_camera = Controller.instance.mainCamera;
			InitialBoid ();
		}
		void OnDisable(){
			boidList.Clear ();
			Transform[] childs = boidContainer.GetComponentsInChildren<Transform> ();
			foreach(Transform child in childs) {
				if (child!=boidContainer) {
					Destroy(child.gameObject);
				}
			}
			targetfollow.localPosition = Vector3.zero;
			targetfollow.localEulerAngles = Vector3.zero;
		}
		void InitialBoid(){
			for (int i = 0; i < boidNum; i++) {
				GameObject go = Instantiate (boidPrefab);
				go.transform.parent = boidContainer;
				Boid boidscript = go.GetComponent<Boid> ();
				boidscript.Initial (Random.Range(0.1f,1f),Random.Range(0.2f,2f),targetfollow);
				boidList.Add (boidscript);


			}

//			targetfollow.transform.DOLocalMove (path [pathPoint].localPosition, 2.5f).SetEase (Ease.Linear).OnComplete(MoveComplete);

//			_target.x = mouseX;
//			_target.y = mouseY;
//
//			// Seek and arrive are similar, though arrive
//			// will cause the boid to slow down as it reaches it's target
//			boid.arrive(_target, 100, 0.8);
//
//			// Add some wander to keep it interesting
//			boid.wander();
		}
		// Update is called once per frame
		int count = 0;
		Vector3 targetPos = new Vector3(0,-2f,0);
//		float wanderDefaultValue = 0.2f;
//		float seekDefaultValue = 0.3f;
		float wanderValue = 0.2f;
		float seekValue = 0.5f;

//		float defaultPosY = 1f;
		float minPosY= -5;
		float maxPosY= 5;
	
		float defaultPosX = 0.1f;
		float minPosX= -0.1f;
		float maxPosX= 0.1f;
//		float default KeyValuePair;
		public void SetHigh(){
//			minPosX = 0.2f;
//			maxPosX = 0.5f;
//			targetPos.y += Random.Range (minPosX, maxPosX);
//			count = 0;
		}
		public void SetBase(){
//			minPosX = -0.5f;
//			maxPosX = -0.2f;
//			targetPos.y += Random.Range (minPosX, maxPosX);
//			targetPos = new Vector3 (Random.Range (minPosX, maxPosX),targetPos.y, targetPos.z);
//			count = 0;
		}
		public void SetVolumeBeat(){
			targetPos.x += dir  * 20;
			dir *= -1;
			forceStop = true;
		}
		void Update () {
//			minPosY += ((-1*defaultPosY) - minPosY) / 20f;
//			maxPosY += (defaultPosY - maxPosY) / 20f;
//			minPosX += ((-1*defaultPosX) - minPosX) / 20f;
//			maxPosX += (defaultPosX - maxPosX) / 20f;
			float[] musicData;
			if (frequencyRange == FrequencyRange.Decibal) {
				musicData = AudioSampler.instance.GetAudioSamples (AudioSampler.instance.audioSource, 128, true);
			}
			else
			{
				musicData = AudioSampler.instance.GetFrequencyData (AudioSampler.instance.audioSource, frequencyRange,128, true);
			}
			UpdateData (musicData);

//			if (Input.GetKeyDown(KeyCode.A)) {
//				wanderValue += 0.01f;
//			}
//			if (Input.GetKeyDown (KeyCode.D)) {
//				seekValue -= 0.01f;
//			}

			//			

//			targetPos.
//			if (count % 10 == 0) {
//				targetPos = new Vector3 (Random.Range (minPosX, maxPosX),0, 0);
//			}
			float beat = MathTool.Remap(musicData [0],0,volumeHighest,0,1f);
			if (beat> 0.5f) {
				Debug.LogWarning ("Beat");
				SetVolumeBeat ();
			}
			if (forceStop == false) {
				targetPos.z += 0.2f;
			} else {
				forceStopCount++;
				if (forceStopCount> 30) {
					forceStop = false;
					forceStopCount = 0;
				}
			}
//			count++;
			targetfollow.transform.localPosition += (targetPos - targetfollow.transform.localPosition) / 5;
//			targetfollow.transform.localPosition = targetPos;
//			float value =MathTool.Remap(musicData [0],0,volumeHighest,0,0.3f);
			for (int i = 0; i < boidList.Count; i++) {
//				boidList [i].SetAdjust(value);
//				float temp = boidList[i].GetAdjust ;
//				temp += (value - temp) / 10;
//				boidList [i].SetAdjust(temp);

//				wanderValue = 0.4f - value;
//				seekValue = value;
				boidList [i].wander (wanderValue);
//				boidList [i].flee (targetfollow.localPosition,0.2f,0.3f);
				boidList [i].seek(targetfollow.localPosition, seekValue);
				boidList [i].flock(boidList);
		
				boidList [i].BoidUpdate ();
//				if (forceStop == false) {
//					_camera.transform.LookAt(targetfollow);
//				}
				if (i == cameraFollowTarget ) {
					boidList [i].ChangeColor (true);
					if (cameraFollow == true) {

						//					_camera.transform.position = boidList[i].gameObject.transform.position;
						//					_camera.transform.eulerAngles = boidList[i].gameObject.transform.eulerAngles;

						_camera.transform.position += (boidList[i].gameObject.transform.position -_camera.transform.position)/5;
//						if (forceStop == true) {
							_camera.transform.DOLocalRotate (boidList [i].gameObject.transform.localEulerAngles, 2f, RotateMode.Fast);	
//						}
//						_camera.transform.LookAt(targetfollow);
						//					_camera.transform.eulerAngles += (boidList[i].gameObject.transform.eulerAngles - _camera.transform.eulerAngles)/20;
					}
				}else{
					boidList [i].ChangeColor (false);
				}
			}
		}
	}

}
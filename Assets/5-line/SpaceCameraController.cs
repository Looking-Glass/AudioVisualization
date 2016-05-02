using UnityEngine;
using System.Collections;
using AudioVisualizer;
public class SpaceCameraController : AudioBase {

	// Use this for initialization
	private Transform camera; 
	public Transform camera_substitute; 
	public Transform cameraPlaceHolder;
	public Transform lookingTarget;
	private Vector3 targetPos;
	float abs_rotation =0.05f;
	void Start () {
		camera = Controller.instance.mainCamera;
	}
	void OnEnable(){
		targetPos = lookingTarget.localPosition;
	}
	void Disable(){
	}
	void Update(){
		float[] musicData_Decibal;
		musicData_Decibal = AudioSampler.instance.GetAudioSamples (AudioSampler.instance.audioSource, 128, true);
		UpdateData (musicData_Decibal);
		float beat = MathTool.Remap(musicData_Decibal [0],0,volumeHighest,0,1f);
		if (beat > 0.5f) {
//			targetPos = Vector3.zero;
			targetPos = Random.onUnitSphere * 2f;
		} 

		cameraPlaceHolder.Rotate(Vector3.right * Time.deltaTime*20);
		cameraPlaceHolder.Rotate(Vector3.up * Time.deltaTime*20, Space.World);

		lookingTarget.localPosition += (targetPos - lookingTarget.localPosition) / 20;

//		transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);


		targetPos += (Vector3.zero - targetPos) / 20;
//		camera.RotateAround(center, Vector3.up, abs_rotation);
		camera.transform.position = camera_substitute.position;
		camera.LookAt (lookingTarget);
	}
//	IEnumerator RandomMove(){
//		while (true) {
//			yield return new WaitForSeconds (0.5f);
//			targetPos = Random.onUnitSphere * 2f;
//		}
//	}

	public void MoveLookingTarget(){
		
	}
}

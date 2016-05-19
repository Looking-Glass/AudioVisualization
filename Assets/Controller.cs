using UnityEngine;
using System.Collections;
using AudioVisualizer;
using DG.Tweening;
[System.Serializable]
public class CustomEffect{
	public GameObject effect;
	public Vector3 camera_Pos = new Vector3 (0, 0, 0);
	public Vector3 camera_Angle = new Vector3 (15, 0, 0);
	public Vector3 camera_Scale = new Vector3 (7, 7, 7);
	public float fileofView = 1;
}
public class Controller : MonoBehaviour {
	#region Singleton
	private static Controller _instance;
	public static Controller instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<Controller>();
			}
			return _instance;
		}
	}
	#endregion

//	public Transform hypercubeContainer;
	public Transform mainCamera;
	// Use this for initialization
	public CustomEffect[] effects;
	public int index = 0; 
	public bool autoSwitchEffect = true;
	public int autoSwitchTime = 1;
	void Start () {
//		mainCamera.transform.parent = hypercubeContainer;
//		hypercubeContainer = mainCamera;
//		mainCamera.localPosition = Vector3.zero;
//		mainCamera.localScale = Vector3.one;
//		mainCamera.localEulerAngles = new Vector3(180,0,0);
		Switch (index);

	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z)) {
			index--;
			if (index <0) {
				index = effects.Length-1;
			}
			Switch (index);
		}
		if (Input.GetKeyDown(KeyCode.X)) {
			index++;
			if (index >effects.Length-1) {
				index = 0;
			}
			Switch (index);
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			int temp = AudioSampler.instance.audioSource+1;
			if (temp >= AudioSampler.instance.audioSources.Count) {
				temp = 0;
			}

			AudioSampler.instance.SetAudioSource (temp);
		}
	}
	void AutioSwitch(){
		index++;
		if (index >effects.Length-1) {
			index = 0;
		}
		Switch (index);
	}
	void Switch(int targetIndex){
		
		for (int i =0;i<effects.Length;i++) {
			if (i != targetIndex) {
				effects [i].effect.SetActive (false);
			}

		}
		for (int i = 0; i < effects.Length; i++) {
			if (i == targetIndex) {
				effects [i].effect.SetActive (true);
				DOTween.Kill (mainCamera);
				mainCamera.transform.parent = null;
				mainCamera.transform.position = effects [i].camera_Pos;
				mainCamera.transform.localEulerAngles = effects [i].camera_Angle;
				mainCamera.transform.localScale = effects [i].camera_Scale;
				mainCamera.GetComponent<hypercubeCamera> ().fieldofView = effects [i].fileofView;
				mainCamera.GetComponent<hypercubeCamera> ().resetSettings ();
			}
		}
		CancelInvoke ("AutioSwitch");
		Invoke ("AutioSwitch",autoSwitchTime);
	}


}
public static class MathTool  {
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}
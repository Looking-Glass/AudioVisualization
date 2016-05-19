using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using AudioVisualizer;
public class BubbleEffect : AudioBase {


	public FrequencyRange frequencyRange = FrequencyRange.Decibal;
//	public int audioSource = 0;

	public GameObject bubbleCircle;
	public GameObject bubbleContainer;
	public float maxSize=5;
	public float startPosX = -20;
	public float endPosX = 20;
	public float startPosY = -20;
//	public List<GameObject> bubbleList = new List<GameObject>();
	public Color normalColor = Color.white;
	public Color veryHightColor = Color.red;
	public Color highColor = Color.magenta;
	public Color baseColor = Color.blue;

	public Color subBaseColor = Color.blue;
	private Color colorNow =Color.white;
	public Vector3[] angleOffset ;
//	public Vector3[] bubbleAngle ;
	private Vector3 targetAngle;
	private int angleIndex = 0;
	private Color targetColor = Color.white;


	public bool autioRotate = true;
	void Start () {
		
	}
	void OnEnable(){
		targetColor = normalColor;
		colorNow = targetColor;
		StartCoroutine ("loopBubble");
		targetAngle = angleOffset [angleIndex];
		bubbleContainer.transform.localEulerAngles = targetAngle;
	}
	void Update(){
		if (Input.GetKeyDown(KeyCode.R)) {
			angleIndex++;
			if (angleIndex >= angleOffset.Length) {
				angleIndex = 0;
			}
			targetAngle = angleOffset [angleIndex];
			bubbleContainer.transform.localEulerAngles = targetAngle;
		}

		if (autioRotate) {
			float xSpeed = Random.Range(5,20);
			float ySpeed = Random.Range(5,20);
			float zSpeed = Random.Range(5,20);
			transform.Rotate(
				xSpeed * Time.deltaTime,
				ySpeed * Time.deltaTime,
				zSpeed * Time.deltaTime
			);
		}
	}
	public void DecibalBeat(){
		ChangeColor (Color.yellow);
	}
	public void VeryHighBeat(){
		ChangeColor (Color.yellow);
	}
	public void BaseBeat(){
		ChangeColor (baseColor);
	}
	public void SubBaseBeat(){
		ChangeColor (subBaseColor);
	}
	public void HighBeat(){
//		CreateBeat (highColor);
	}
	void ChangeColor(Color beatColor){
		targetColor = beatColor;
	}
	public void CreateBeat(){
		float[] musicData;
		if (frequencyRange == FrequencyRange.Decibal) {
			musicData = AudioSampler.instance.GetAudioSamples (AudioSampler.instance.audioSource, 1, true);
		}
		else
		{
			musicData = AudioSampler.instance.GetFrequencyData (AudioSampler.instance.audioSource, frequencyRange, 1, true);
		}
		UpdateData (musicData);
		GameObject newBubble = Instantiate (bubbleCircle);
		newBubble.transform.parent = bubbleContainer.transform;
		Bubble bubbleScript = newBubble.GetComponent<Bubble> ();
		colorNow += (targetColor - colorNow) / 20; //2

		float size = MathTool.Remap(musicData [0],0,volumeHighest,0.01f,maxSize);
		bubbleScript.Init (new Vector3(startPosX,startPosY,0),size,colorNow,endPosX);

		//		bubbleContainer
	}
	IEnumerator loopBubble(){
		while (true) {
			CreateBeat ();
//			float[] musicData;
//			if (frequencyRange == FrequencyRange.Decibal) {
//				musicData = AudioSampler.instance.GetAudioSamples (audioSource, 100, true);
//			} else {
//				musicData = AudioSampler.instance.GetFrequencyData (audioSource, frequencyRange, 100, true);
//			}
//
//			GameObject newBubble = Instantiate (bubbleCircle);
//			newBubble.transform.parent = bubbleContainer.transform;
//			Bubble bubbleScript = newBubble.GetComponent<Bubble> ();
//			colorNow += (normalColor - colorNow) / 10;
//			bubbleScript.Init (new Vector3 (startPosX, startPosY, 0), musicData [0] * adjustSize,colorNow,endPosX);

			yield return new WaitForSeconds (0.001f);
		}
	}

}

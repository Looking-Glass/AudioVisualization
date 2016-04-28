using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using AudioVisualizer;
public class Effect2 : AudioBase {
//	public AudioSource _as;
	public FrequencyRange frequencyRange = FrequencyRange.Decibal;

	public bool rotateConainer= false;
	public int cubesNum = 10;
	public GameObject cubePrefab;
	public Transform cubeContainer;
	private List<GameObject> cubes = new List<GameObject> ();
	float tempScaleY = 0;
	public float radius = 10;
//	int total;
//	public float heightAdjust = 2f;
	public float heightMax = 5f;

	public Vector3 cameraTargetAngle  = Vector3.zero;
	private int direction = 1;
//	[Range(0f,1f)]
//	public float s = 1f;
//	[Range(0f,1f)]
//	public float v = 1f;
////	[Range(0f,10f)]
//	public float colorMultiplyer = 1f;
	public float easetime = 0.3f;


//	public Color startColor;
//	public Color endColor;
	public Vector3 offsetPos = new Vector3 (0, -2, 0);
//	public Vector3 centerPos = Vector3.zero;
	public float offset = 0;
	private int offsetDir = 1;
	public List<Vector3> localPos = new List<Vector3> ();
//	public Color[] OverTheTopColor;
	public Color[] _GradientTopColor;
	public Color[] _GradientBottomColor;
	private int colorCount = 0;
	private int colorIndex = 0;

	private Color topColor;
	private Color bottomColor;
//	public Color[] BelowTheBottomColor;
	public float bottom = -2;
	public float top = 1;
	public Material mat;

	public GameObject drop;
	public bool rainable = false;
	// Use this for initialization
	void Start () {
		
//		total = (int)( radius*2 *Mathf.PI/space);
		Init ();
		transform.position =offsetPos;
	}
	void Init(){
		for (int j = 0; j < cubesNum; j++) {
			GameObject _new = Instantiate (cubePrefab);
			_new.transform.localPosition = new Vector3 (radius * Mathf.Cos (j * 360 / cubesNum * Mathf.Deg2Rad), 0, radius * Mathf.Sin (j * 360 / cubesNum * Mathf.Deg2Rad));
			_new.transform.parent = cubeContainer;
			_new.transform.localScale = new Vector3 (_new.transform.localScale.x, tempScaleY, _new.transform.localScale.z);
			cubes.Add (_new);
			GameObject go2 = _new.transform.GetChild (0).gameObject;
//			go2.GetComponent<MeshRenderer> ().material = Instantiate(mat) as Material;
			go2.GetComponent<MeshRenderer> ().material = mat;

			localPos.Add (_new.transform.localPosition);
		}
		cubeContainer.transform.localEulerAngles = new Vector3 (0, 90, 0);
		topColor = _GradientTopColor [colorIndex];
		bottomColor = _GradientBottomColor [colorIndex];

	}

	void Update ()
	{
		if (rotateConainer == true) {
			cameraTargetAngle = cubeContainer.localEulerAngles + new Vector3(0,direction*0.5f,0);
			cubeContainer.localEulerAngles = cameraTargetAngle;

		}

		Visulization ();
	}

	void Visulization ()
	{
		float[] musicData;
		if (frequencyRange == FrequencyRange.Decibal) {
			musicData = AudioSampler.instance.GetAudioSamples (AudioSampler.instance.audioSource, cubesNum *4, true);
		}
		else
		{
			musicData = AudioSampler.instance.GetFrequencyData (AudioSampler.instance.audioSource, frequencyRange, cubesNum *4, true);
		}
		UpdateData (musicData);

		offset +=	offsetDir * 0.001f;
		if (offset >= 1) {
			offsetDir *= -1;
		} else if (offset <= 0) {
			offsetDir *= -1;
		}

		colorCount++;
		if (colorCount %50 == 0) {
			colorIndex++;
			if (colorIndex >=_GradientTopColor.Length) {
				colorIndex = 0;
			}
		}
		topColor += (_GradientTopColor [colorIndex] - topColor) / 50;
		bottomColor += (_GradientBottomColor [colorIndex] - bottomColor) / 50;

		mat.SetColor ("_GradientTopColor", topColor);
		mat.SetColor ("_GradientBottomColor",bottomColor);
		mat.SetColor ("_OverTopColor",topColor);
		mat.SetColor ("_BelowBottomColor", bottomColor);

		int i = 0;

		while (i <cubesNum) {
			float height = MathTool.Remap(musicData [i],0,volumeHighest,0.01f,heightMax);
			cubes[i].transform.DOScaleY(height,easetime).SetEase(Ease.OutCubic);
			GameObject go = cubes [i].transform.GetChild (0).gameObject;

			cubes[cubesNum - i-1].transform.DOScaleY(height,easetime).SetEase(Ease.OutCubic);
//			float accelx = 10;
//			float accely = 20;
//			float accelz = 10;
//
//			go.transform.Rotate (accelx * Time.deltaTime, accely * Time.deltaTime, accelz * Time.deltaTime);
//			cubes[cubesNum - i-1].transform.localPosition += localPos[i] + centerPos
//			cubes[cubesNum - i-1].transform.localPosition = centerPos * offset - localPos[i] *(1-offset);
//			cubes[cubesNum - i-1].transform.localEulerAngles = new Vector3 (i*360/cubesNum,0,i*360/cubesNum) * offset;
//			cubes[cubesNum - i-1].transform.localEulerAngles = new Vector3 ( 180* Mathf.Cos (i * 360 / cubesNum * Mathf.Deg2Rad), 0,  180*Mathf.Sin (i * 360 / cubesNum * Mathf.Deg2Rad));


			i++;
		}

		if (rainable == true) {
			float beat = MathTool.Remap(musicData [0],0,volumeHighest,0,1f);
			if (beat> 0.9f) {

				GoldenRain ();
			}
		}
//		cubeContainer.transform.localScale += (Vector3.one - cubeContainer.transform.localScale) / 30f;


	}
	void GoldenRain(){

		cubeContainer.transform.localScale = Vector3.one * Mathf.Sign(cubeContainer.transform.localScale.x)*1f;
		cubeContainer.transform.DOScale (-1 * cubeContainer.transform.localScale, 0.5f).SetEase (Ease.OutCubic);
//		cubeContainer.transform.localScale = Mathf.Sign (Random.Range (-1, 1)) * Vector3.one * 1.5f;
//		int i = 0;
//		while (i < 10) {
////			GameObject d = Instantiate (drop);
////			d.transform.parent = cubeContainer;
////			d.transform.localPosition = new Vector3 (Random.Range (-5f, 5f),10, Random.Range (-5f, 5f));
////			d.transform.DOLocalMoveY (-1, Random.Range (1f, 3f)).SetEase (Ease.OutCubic).OnComplete (() => CompleteHandler (d));
////			d.transform.DOLocalRotate (new Vector3 (0, 360, 0), 3).SetEase (Ease.Linear);
////			i++;
//		}
	}
	void CompleteHandler(GameObject d){
		Destroy (d);
	}
	IEnumerator HitCircle(int circleIndex){
//		if (circleIndex < circleNum && circleIndex>=0) {
////			float[] samples = AudioSampler.instance.GetAudioSamples (audioSource,lineAttributes.lineSegments, true); // get teh sames at the time of the ripple
//
//
//			Debug.Log ("HitCircle=" + circleIndex+"   cubes [circleIndex].count="+ cubes [circleIndex].Count);
//			for (int j = 0; j < cubes [circleIndex].Count; j++) {
//
//				cubes [circleIndex] [j].transform.DOScaleY ((circleNum-circleIndex)*0.3f, 0.1f).SetEase (Ease.OutCubic);
////				cubes [circleIndex] [j].transform.localScale = new Vector3 (1, circleIndex, 1);
//				cubes [circleIndex] [j].transform.DOScaleY (tempScaleY, 0.5f).SetEase (Ease.OutCubic).SetDelay(0.1f);
//
//			}
//			yield return new WaitForSeconds (0.2f);
//			ReCallHitCircle (circleIndex--);
//		}
		yield return new WaitForSeconds (0.2f);
	}
	void ReCallHitCircle(int circleIndex){
		Debug.Log ("ReCallHitCircle=" + circleIndex);
//		HitCircle (circleIndex);
		StartCoroutine(HitCircle (circleIndex));
	}



	#region Static
	public static Color HSVtoRGB (float hue, float saturation, float value, float alpha)
	{
		while (hue > 1f) {
			hue -= 1f;
		}
		while (hue < 0f) {
			hue += 1f;
		}
		while (saturation > 1f) {
			saturation -= 1f;
		}
		while (saturation < 0f) {
			saturation += 1f;
		}
		while (value > 1f) {
			value -= 1f;
		}
		while (value < 0f) {
			value += 1f;
		}
		if (hue > 0.999f) {
			hue = 0.999f;
		}
		if (hue < 0.001f) {
			hue = 0.001f;
		}
		if (saturation > 0.999f) {
			saturation = 0.999f;
		}
		if (saturation < 0.001f) {
			return new Color (value * 255f, value * 255f, value * 255f);

		}
		if (value > 0.999f) {
			value = 0.999f;
		}
		if (value < 0.001f) {
			value = 0.001f;
		}

		float h6 = hue * 6f;
		if (h6 == 6f) {
			h6 = 0f;
		}
		int ihue = (int)(h6);
		float p = value * (1f - saturation);
		float q = value * (1f - (saturation * (h6 - (float)ihue)));
		float t = value * (1f - (saturation * (1f - (h6 - (float)ihue))));
		switch (ihue) {
		case 0:
			return new Color (value, t, p, alpha);
		case 1:
			return new Color (q, value, p, alpha);
		case 2:
			return new Color (p, value, t, alpha);
		case 3:
			return new Color (p, q, value, alpha);
		case 4:
			return new Color (t, p, value, alpha);
		default:
			return new Color (value, p, q, alpha);
		}
	}
	#endregion
}

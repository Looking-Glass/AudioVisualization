using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

using AudioVisualizer;

public class Effect1 : AudioBase {
	public Vector3 positionOffset= Vector3.zero;
	public FrequencyRange frequencyRange = FrequencyRange.Decibal;
//	public int audioSource = 0;

	public int circleNum = 10;
//	public float heightAdjust = 2f;


	public float heightMax = 0f;

//	int dec = 128;
//	List<float> waveData = new List<float> ();

	public GameObject cubePrefab;
	public Transform cubeContainer;
	private List<List<GameObject>> cubes = new List<List<GameObject>> ();

	private List<List<Vector3>> targetScale = new List<List<Vector3>> ();
	float tempScaleY = 0;
	public float circleDis = 1.5f;
	public float space = 2f;
	public float easeTime = 0.1f;
	public Color[] colors;
	private int colorIndex = 0;
	private Color targetColor;
	public Material mat;
	// Use this for initialization
	void Start () {
		colorIndex = 0;
		targetColor = colors [colorIndex];
		Init ();
		transform.position = positionOffset;
//		StartCoroutine ("ColorChange");
	}
	void OnDisable(){
		StopCoroutine ("ColorChange");
	}
	void OnEnable(){
		StartCoroutine ("ColorChange");
	}

	void Init(){
		mat.color = colors [0];
		for (int i = 0; i < circleNum; i++) {
			List<GameObject> temp = new List<GameObject> ();

			List<Vector3> tempScale = new List<Vector3> ();


			int total = (int)( (i+1)*circleDis*2 *Mathf.PI/space);
			for (int j = 0; j < total; j++) {
				GameObject _new = Instantiate (cubePrefab);
				_new.transform.localPosition = new Vector3 ((i+1)*circleDis * Mathf.Cos (j * 360 / total * Mathf.Deg2Rad), 0, (i+1)*circleDis * Mathf.Sin (j * 360 / total * Mathf.Deg2Rad));
				_new.transform.parent = cubeContainer;
				_new.transform.localScale = new Vector3 (_new.transform.localScale.x, tempScaleY, _new.transform.localScale.z);
				temp.Add (_new);
				tempScale.Add (_new.transform.localScale);
				Transform cube = _new.transform.FindChild ("Cube");
				cube.GetComponent<MeshRenderer> ().sharedMaterial = mat;
			}

			cubes.Add (temp);
			targetScale.Add (tempScale);
		}
	}
	void Update ()
	{
		UpdateColor ();
		Visulization ();
	}
	void UpdateColor(){
		mat.color += (targetColor - mat.color) / 20;

//		mat.color = targetColor;
	}
	IEnumerator ColorChange(){
		while (true) {
			yield return new WaitForSeconds (1);
			colorIndex++;
			if (colorIndex>=colors.Length) {
				colorIndex = 0;

			}
			targetColor = colors [colorIndex];
		}
	}
	void Visulization ()
	{
		float[] musicData;
		if (frequencyRange == FrequencyRange.Decibal) {
			musicData = AudioSampler.instance.GetAudioSamples (AudioSampler.instance.audioSource, 10, true);
		}
		else
		{
			musicData = AudioSampler.instance.GetFrequencyData (AudioSampler.instance.audioSource, frequencyRange, circleNum, true);
		}

		if (musicData.Length<=0) {
			return;
		}
	
		UpdateData (musicData);

		for(int i =0; i<circleNum;i++){
//			Debug.Log ("cubes[i].Count = " + cubes [i].Count);
			for (int j = 0; j < cubes[i].Count; j++) {
//				float height = Mathf.Min(heightMax,musicData [i] * heightAdjust);
				float height = MathTool.Remap(musicData [0],0,volumeHighest,0f,heightMax);
//				Debug.Log ("height=" + height);

//				targetScale [i] [j] = new Vector3 (0.5f, height, 0.5f);
				if ( i == 0) {
//					float height = Mathf.Min(heightMax,musicData [0] * heightAdjust);

					targetScale [i] [j] = new Vector3 (i*0.1f, height, (circleNum - i)*0.1f);
				}else{
					Vector3 preHeight = targetScale [i - 1] [0];
					preHeight.y *= 0.8f;
					preHeight.x = (circleNum - i) * 0.1f;
					preHeight.z = (circleNum - i) * 0.1f;
					targetScale [i] [j] = preHeight;
				}

				cubes [i] [j].transform.DOScale (targetScale [i] [j], easeTime).SetEase (Ease.OutCubic);
			}
		}


	}

	public void Beat(){
//		if (gameObject.activeSelf == false) {
//			return;
//		}
//		StartCoroutine(HitCircle (0));
	}
	IEnumerator HitCircle(int circleIndex){
		if (circleIndex < circleNum) {
//			float[] samples = AudioSampler.instance.GetAudioSamples (audioSource,lineAttributes.lineSegments, true); // get teh sames at the time of the ripple


			Debug.Log ("HitCircle=" + circleIndex+"   cubes [circleIndex].count="+ cubes [circleIndex].Count);
			for (int j = 0; j < cubes [circleIndex].Count; j++) {

				cubes [circleIndex] [j].transform.DOScaleY ((circleNum-circleIndex)*0.3f, 0.1f).SetEase (Ease.OutCubic);
//				cubes [circleIndex] [j].transform.localScale = new Vector3 (1, circleIndex, 1);
				cubes [circleIndex] [j].transform.DOScaleY (tempScaleY, 0.5f).SetEase (Ease.OutCubic).SetDelay(0.1f);

			}
			yield return new WaitForSeconds (0.2f);
			ReCallHitCircle (circleIndex+1);
		}
		yield return new WaitForSeconds (0.2f);
	}
	void ReCallHitCircle(int circleIndex){
		Debug.Log ("ReCallHitCircle=" + circleIndex);
//		HitCircle (circleIndex);
		StartCoroutine(HitCircle (circleIndex));
	}

//	IEnumerator RipIt(float propegationTime, float[] rippleSamples)
//	{
//		//Vector3 firstPos = Vector3.zero; // the first position we use
//		float timer = 0;
//		float radiusStep = radius / (circleNum-1); // distance between each ring. i.e. ring0 has radius 0*radiusStep, ring10 had radius 10*radiusStep, etc.
//		float angle = 0; 
//		float angleStep = 360f / lineAttributes.lineSegments;//increase the angle by this much, for every point on every line, to draw each circle.
//		float percent = 0;
//		int maxIndex = numLines - 1;
//		int halfWidth = rippleWidth / 2; // (int) on purpose
//		float heightStep = (1f / (halfWidth + 1)); //ripple height step size
//
//		// another gradient, between start color and rippleColor
//		Gradient lineGradient = PanelWaveform.GetColorGradient(lineAttributes.startColor, rippleColor); 
//		Color[] rippleColors = new Color[maxIndex];
//		float step = 1f / (rippleWidth - 1);
//		for (int i = 0; i < rippleWidth; i++)
//		{
//			percent = i * step;
//			rippleColors[i] = lineGradient.Evaluate(percent);
//		}
//
//		Color[] lineColors = new Color[numLines];//color of each line using the gradient
//		float[] heightDamp = new float[numLines];//height damp array, how much we'll damp the ripple as it travels across the pad.
//		float dampStep = maxHeight/(numLines-1);
//		step = 1f / (numLines - 1);
//		for (int i = 0; i < numLines; i++)
//		{
//			percent = i * step;
//			lineColors[i] = padGradient.Evaluate(percent);
//			heightDamp[i] = maxHeight - i * dampStep;
//		}
//
//
//
//
//
//
//		//Debug.Log ("Ripple from " + rippleLines + " to " + numLines);
//		while (timer <= propegationTime)
//		{
//			//what line are we on, in the range rippleLines to numLines, based on the timer
//			percent =  (timer / propegationTime);
//			int lineIndex =(int)( percent * maxIndex);
//
//			//start/end index
//			int rippleStart = lineIndex - rippleWidth-1; // 1 outside the ripple
//			rippleStart = Mathf.Max(0,rippleStart);
//			int rippleEnd = lineIndex + rippleWidth;
//			rippleEnd = Mathf.Min(rippleEnd, numLines);
//			Vector3 firstPos = Vector3.zero;
//
//			for (int i = rippleStart; i < rippleEnd; i++)// for each line
//			{
//				int dist = Mathf.Abs(lineIndex - i); // our distance from the lineIndex
//				int invDist = rippleWidth - dist;
//				float heightMultiplier = (dist > halfWidth) ? 0 : (1f - heightStep * dist);
//				float thisRadius = radiusStep * i; // the radius of this ring
//				//color the ring
//				if(i == (lineIndex - rippleWidth -1))
//				{
//					lines[i].SetColors(lineColors[lineIndex], lineColors[lineIndex]);
//				}
//				else{
//					lines[i].SetColors(rippleColors[invDist], rippleColors[invDist]);
//				}
//				for (int j = 0; j < lineAttributes.lineSegments - 1; j++) // for each line segment
//				{
//					float rad = Mathf.Deg2Rad * angle; // get angle in radians
//					//get x,y,z of this lineSegment using equation for a circle
//					float x = Mathf.Cos(rad) * thisRadius;
//					float y = rippleSamples[j] * heightDamp[lineIndex] * heightMultiplier; // y value based on audio info (rippleSamples) * heightMultiplier
//					float z = Mathf.Sin(rad) * thisRadius;
//					Vector3 pos = this.transform.position + this.transform.right * x + this.transform.up * y + this.transform.forward * z;
//					lines[i].SetPosition(j, pos);
//					angle += angleStep; // increase angle by angleStep
//					if (j == 0)
//					{
//						firstPos = pos; // track the first lineSegment position
//					}
//				}
//
//				lines[i].SetPosition(lineAttributes.lineSegments-1,firstPos); // set the last pos = to the first pos.
//			}
//
//			timer += Time.fixedDeltaTime;
//
//			yield return null;
//		}
//
//
//	}
}

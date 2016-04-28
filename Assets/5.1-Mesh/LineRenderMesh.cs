using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AudioVisualizer;
public class LineRenderMesh : MonoBehaviour {
	public FrequencyRange frequencyRange = FrequencyRange.Decibal;
	public int audioSource = 0;

	public float TileWidth = 1;
	public float TileHeight = 1;
	public int NumTilesXY = 16;
	public Transform lineContainer;
	public Material lineMat;
	public float lineWidth =0.1f;
	public Vector3 offset = Vector3.zero;
	public List<LineRenderer> H_LR = new List<LineRenderer> ();
	public List<LineRenderer> V_LR = new List<LineRenderer> ();

	public float heightAdjust = 2f;
	public float heightMax = 5f;

	public AudioSource source; 
	private int samples = 1024; //採樣的層數，需要2的次方，
	private List<float[]> datas = new List<float[]>(); //為了讓Mesh有波動的效果，所以用個List保留採樣過的資料
	private int minLength; //這邊網格的寬我只有使用50，所以計算每個點之間間格的採樣層數


	void Start () {
		minLength = Mathf.FloorToInt((float)samples / (float)NumTilesXY); 

		for(int i = 0; i < NumTilesXY; ++i){
			datas.Add (new float[samples]);
		}

		Init ();
	}
	void Init(){
		
//		offset = new Vector3 (NumTilesXY / 2 * TileWidth, NumTilesXY / 2 * TileWidth, 0);
		for (int i = 0; i < NumTilesXY; i++) {
			GameObject go = new GameObject ();
			LineRenderer HLR_lr =go.AddComponent <LineRenderer>() as LineRenderer;
			HLR_lr.material = lineMat;
			go.transform.parent = lineContainer;
			HLR_lr.SetVertexCount(NumTilesXY);
			HLR_lr.SetWidth (lineWidth, lineWidth);
			HLR_lr.receiveShadows = false;
			HLR_lr.useLightProbes = false;
			HLR_lr.useWorldSpace = false;
			for (int j = 0; j < NumTilesXY; j++) {
				HLR_lr.SetPosition (j,new Vector3 (i-NumTilesXY/2* TileWidth , 0f,j-NumTilesXY/2 * TileWidth)+offset);
			}
			H_LR.Add (HLR_lr);
			go.name = "H_" + i;
		}
		for (int i = 0; i < NumTilesXY; i++) {
			GameObject go = new GameObject ();
			LineRenderer VLR_lr =go.AddComponent <LineRenderer>() as LineRenderer;
			VLR_lr.material = lineMat;
			go.transform.parent = lineContainer;
			VLR_lr.SetVertexCount(NumTilesXY);
			VLR_lr.SetWidth (lineWidth, lineWidth);
			for (int j = 0; j < NumTilesXY; j++) {
				VLR_lr.SetPosition (j,new Vector3 (j-NumTilesXY/2* TileWidth ,0f,  i-NumTilesXY/2 * TileWidth)+offset);
			}
			V_LR.Add (VLR_lr);
			go.name = "H_" + i;
		}
	}
	void Update () {
//		GetFreq ();
		Visulization2 ();
	}

	void GetFreq()
	{
		float[] newFreqData = new float[samples];
		source.GetSpectrumData(newFreqData, 0, FFTWindow.BlackmanHarris); 

		datas.Add (newFreqData);
		if(datas.Count > NumTilesXY)
			datas.RemoveAt(0);
	}
	void Visulization(){


		for(int y = 0; y < NumTilesXY; ++y)
		{
			float[] lineSamples = datas[y]; 
//			Debug.Log ("lineSamples="+lineSamples.Length);
			for(int x = 0; x < NumTilesXY; ++x)
			{
//				Debug.Log ("x*NumTilesXY=" + (x * NumTilesXY));
				float sample = lineSamples[x*NumTilesXY];

				float height = Mathf.Clamp (sample * (0.5f * ((x + 1) * (x + 1))), 0f, 50f);
//				Vector3 pos = H_LR[x]
				Debug.Log ("height" + height);
				H_LR[x].SetPosition (y,new Vector3 (y-NumTilesXY/2* TileWidth , x-NumTilesXY/2 * TileWidth, height)+offset);
				V_LR[y].SetPosition (x,new Vector3 (y-NumTilesXY/2* TileWidth , x-NumTilesXY/2 * TileWidth, height)+offset);
//				plane.matrix[(y*plane.lengthX) + x].y = ;
			}
		}
//		plane.mesh.vertices = plane.matrix; //Update mesh
	}
	void Visulization2(){

		float[] musicData;
		if (frequencyRange == FrequencyRange.Decibal) {
			musicData = AudioSampler.instance.GetAudioSamples (audioSource, NumTilesXY, true);
		}
		else
		{
			musicData = AudioSampler.instance.GetFrequencyData (audioSource, frequencyRange, NumTilesXY, true);
		}

		for(int y = 0; y < NumTilesXY; ++y)
		{
			float height = Mathf.Min(heightMax,musicData [y] * heightAdjust);
			for(int x = 0; x < NumTilesXY; ++x)
			{
				
				Debug.Log ("height" + height);
				H_LR[x].SetPosition (y,new Vector3 (y-NumTilesXY/2* TileWidth ,height, x-NumTilesXY/2 * TileWidth)+offset);
				V_LR[y].SetPosition (x,new Vector3 (y-NumTilesXY/2* TileWidth ,height, x-NumTilesXY/2 * TileWidth)+offset);
				//				plane.matrix[(y*plane.lengthX) + x].y = ;
			}
		}
		//		plane.mesh.vertices = plane.matrix; //Update mesh
	}
}

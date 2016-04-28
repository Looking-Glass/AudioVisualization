using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Bubble : MonoBehaviour {

	public GameObject line;
	public float radius = 2;
	public float totalLine = 20;
	public float width = 1;
	public Vector3 lineScale = new Vector3(0.3f,0.05f,0.3f);
//	public GameObject bubbleCicle;
	// Use this for initialization
	void Start(){
//		Init (new Vector3(-10,0,0));
	}
	public void Init(Vector3 posOffset,float _radius,Color _color,float endPosX){
		radius = _radius;
		int total = 90;
		transform.localEulerAngles = new Vector3(0,90,0f);

//		gameObject.GetComponent<SpriteRenderer> ().color = _color;
//		_radius = Mathf.Clamp (_radius, 0.5f, 4);
//		Vector3 targeScale =  Vector3.one * _radius
		Vector3 targeScale =  Vector3.one * _radius;
		targeScale.z = 0.5f;
		transform.localScale = targeScale;
//		for (int i = 0; i < total; i++) {
//			GameObject _new = Instantiate (line);
//			_new.transform.parent = transform;
//			_new.transform.localPosition = new Vector3 (radius * Mathf.Cos (360 / total * i * Mathf.Deg2Rad),0, radius * Mathf.Sin (360 / total * i  * Mathf.Deg2Rad));
////			_new.transform.localEulerAngles = new Vector3 (90,  -1 * 360 / total * i , 0); 
//			_new.transform.localScale = lineScale;
		MeshRenderer[] srs = gameObject.GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer sr in srs) {
			sr.material.color = _color;
		}
//
//		}
		transform.localPosition = posOffset;
//		transform.localEulerAngles = new Vector3 (0, 0, 90);
		transform.DOLocalMoveX (endPosX, 0.5f).SetEase(Ease.Linear).OnComplete(CompleteHandler);
//
//		SpriteRenderer[] srs2 = gameObject.GetComponentsInChildren<SpriteRenderer> ();
//		foreach (SpriteRenderer sr in srs2) {
////			sr.material.color = _color;
//			sr.DOFade(0f,1).SetEase(Ease.Linear);
//		}

		MeshRenderer[] srs2 = gameObject.GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer sr in srs2) {
			sr.material.DOFade(0f,0.05f).SetEase(Ease.OutQuad).SetDelay(0.45f);
//			sr.DOFade(0f,4).SetEase(Ease.OutQuart);
		}
	}
	void CompleteHandler(){
		Destroy (gameObject);
	}
}

// VerticalGradientRange by mgear / unitycoder.com
// demo script for moving top/bottom lines up & down

using UnityEngine;
using System.Collections;

public class gradient_DemoControls : MonoBehaviour {
	
	
	public Transform topBar;
	public Transform bottomBar;

	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey("q"))
		{
			topBar.Translate(Vector3.up * Time.deltaTime);
		}
		
		if (Input.GetKey("a"))
		{
			if (topBar.position.y>bottomBar.position.y)
				topBar.Translate(-Vector3.up * Time.deltaTime);
		}
		
		if (Input.GetKey("w"))
		{
			if (bottomBar.position.y<topBar.position.y)
				bottomBar.Translate(Vector3.up * Time.deltaTime);
		}
		
		if (Input.GetKey("s"))
		{
			bottomBar.Translate(-Vector3.up * Time.deltaTime);
		}
	
	}
}

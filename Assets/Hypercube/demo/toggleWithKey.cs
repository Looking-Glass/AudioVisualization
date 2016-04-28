using UnityEngine;
using System.Collections;

public class toggleWithKey : MonoBehaviour 
{

    public KeyCode key;
    public GameObject toggle;


	// Update is called once per frame
	void Update () 
    {

        if (Input.GetKeyDown(key))
        {
            if (toggle.activeSelf)
                toggle.SetActive(false);
            else
                toggle.SetActive(true);
        }
	
	}
}

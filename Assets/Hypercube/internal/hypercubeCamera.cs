using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class hypercubeCamera : MonoBehaviour {

    public float overlap = .2f; 
    public float brightness = 1f; //  a convenience way to set the brightness of the rendered textures. The proper way is to call 'setTone()' on the canvas
    public int slices = 12;
    public Camera[] cameras;
    public hypercubeCanvas canvasPrefab;
    public hypercubeCanvas localCanvas = null;
    public hypercubePreview preview = null;
    //public hypercubeCanvas getLocalCanvas() { return localCanvas; }
	public float fieldofView =1;


    void Start()
    {
        if (!localCanvas)
        {
            localCanvas = GameObject.FindObjectOfType<hypercubeCanvas>();
            if (!localCanvas)
                localCanvas = Instantiate(canvasPrefab);  //if no canvas exists. we need to have one or the hypercube is useless.
        }

        //use our save values only in the player only to avoid confusing behaviors in the editor
        //LOAD OUR PREFS
        if (!Application.isEditor)
        {
            dataFileAssoc d = GetComponent<dataFileAssoc>();
            if (d)
            {
                slices = d.getValueAsInt("sliceCount", 10);
                localCanvas.sliceOffsetX = d.getValueAsFloat("offsetX", 0);
                localCanvas.sliceOffsetY = d.getValueAsFloat("offsetY", 0);
                localCanvas.sliceWidth = d.getValueAsFloat("sliceWidth", 800f);
                localCanvas.sliceHeight = d.getValueAsFloat("pixelsPerSlice", 68f);
                localCanvas.flipX = d.getValueAsBool("flipX", false);
            }
        }

        localCanvas.updateMesh(slices);
        resetSettings();
    }

    

    void Update()
    {
        if (transform.hasChanged)
        {
            resetSettings(); //comment this line out if you will not be scaling your cube during runtime
        }
        render();
    }

    void OnValidate()
    {
        if (slices < 1)
            slices = 1;
        if (slices > cameras.Length)
            slices = cameras.Length;

        if (localCanvas)
        {
            localCanvas.setTone(brightness);
            localCanvas.updateMesh(slices);
        }
        if (preview)
        {
            preview.sliceCount = slices;
            preview.sliceDistance = 1f / (float)slices;
            preview.updateMesh();
        }
    }

    public void render()
    {
        for (int i = 0; i < slices; i++)
        {
            cameras[i].Render();
        }
    }


    public void resetSettings()
    {

        float sliceDepth = transform.localScale.z/(float)slices;
        float aspectRatio = transform.localScale.x / transform.localScale.y;

        float cameraSize = .5f * transform.localScale.y;

        for (int i = 0; i < slices && i < cameras.Length; i ++ )
        {
            cameras[i].nearClipPlane = i * sliceDepth - (sliceDepth * overlap);
            cameras[i].farClipPlane = (i + 1) * sliceDepth + (sliceDepth * overlap);
//            cameras[i].orthographicSize = cameraSize;
			cameras[i].orthographicSize = cameraSize + (fieldofView - 1) /slices* i; 
            cameras[i].aspect = aspectRatio;
        }
    }

    void OnApplicationQuit()
    {
        //save our settings whether in editor mode or play mode.
        dataFileAssoc d = GetComponent<dataFileAssoc>();
        if (!d)
            return;
        d.setValue("sliceCount", slices.ToString(), true);
        d.setValue("offsetX", localCanvas.sliceOffsetX.ToString(), true);
        d.setValue("offsetY", localCanvas.sliceOffsetY.ToString(), true);
        d.setValue("sliceWidth", localCanvas.sliceWidth.ToString(), true);
        d.setValue("pixelsPerSlice", localCanvas.sliceHeight.ToString(), true);
        d.setValue("flipX", localCanvas.flipX.ToString(), true);
        d.save();
    }
}

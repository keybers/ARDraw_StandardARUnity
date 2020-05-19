using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDraw : MonoBehaviour
{
    private GameObject clone;
    private LineRenderer line;
    private int i;
    public GameObject LineObject;
    public Camera TargetCamera;
    public float LineWidth;

    private GameObject DrawSpace;

    private Material CurrentMaterial;

    //Color Picker
    private GameObject go;
    public Material ColorPickerMaterial;

    // Start is called before the first frame update
    void Start()
    {
        DrawSpace = GameObject.Find("DrawSpace");
        //CurrentMaterial = new Material(Shader.Find("White"));
        CurrentMaterial = (Material)Instantiate(Resources.Load("Materials/White", typeof(Material)) as Material);
        //Debug.Log(Application.dataPath + "/Scenes/ARDraw/Material/White");
        Debug.Log(CurrentMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clone = (GameObject)Instantiate(LineObject, LineObject.transform.position, transform.rotation);
            clone.GetComponent<LineRenderer>().material = CurrentMaterial;
            clone.transform.parent = DrawSpace.transform;
            line = clone.GetComponent<LineRenderer>();
            line.startWidth = LineWidth;
            line.endWidth = LineWidth;
            i = 0;
        }
        if (Input.GetMouseButton(0))
        {
            i++;
            line.positionCount = i;
            line.SetPosition(i - 1, TargetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15)));
        }

    }

    public void Clean()
    {
        int childCount = DrawSpace.transform.childCount;
        for (int j = 0; j < childCount; j++)
        {
            Destroy(DrawSpace.transform.GetChild(j).gameObject);
        }

    }

    public void ChangeTexure(string materialName)
    {
        CurrentMaterial = (Material)Instantiate(Resources.Load("Materials/" + materialName, typeof(Material)) as Material);
        //int childCount = DrawSpace.transform.childCount;
        //for (int j = 0; j < childCount; j++)
        //{
        //    Destroy(DrawSpace.transform.GetChild(j).gameObject);
        //}

    }

    public void ChangeTexure(Material material)
    {
        CurrentMaterial = (Material)Instantiate(material);
    }


}

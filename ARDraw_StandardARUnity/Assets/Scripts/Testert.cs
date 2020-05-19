using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testert : MonoBehaviour
{
    [SerializeField]
    public Color[] AllColors;

    // Start is called before the first frame update
    void Start()
    {
        //CreateMaterial();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateMaterial()
    { 
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = Color.red;
        transform.GetComponent<MeshRenderer>().material = mat;
    }
}

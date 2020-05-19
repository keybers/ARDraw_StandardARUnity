using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerControl : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        transform.Find("R").GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck("R"); });
        transform.Find("G").GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck("G"); });
        transform.Find("B").GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck("B"); });
        transform.Find("A").GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck("A"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Color GetColorPcikerValue()
    {
        float R = transform.Find("R").GetComponent<Slider>().value / 255;
        float G = transform.Find("G").GetComponent<Slider>().value / 255;
        float B = transform.Find("B").GetComponent<Slider>().value / 255;
        float A = transform.Find("A").GetComponent<Slider>().value / 255;
        Color color = new Color(R,G,B,A);
        Debug.Log(color);
        return color;
    }

    public void ValueChangeCheck(string name)
    {
        transform.Find(name+ "/Text Amount").GetComponent<Text>().text = transform.Find(name).GetComponent<Slider>().value.ToString();
    }
}

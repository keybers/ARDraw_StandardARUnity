using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewLine : MonoBehaviour
{
    public FlexibleColorPicker fcp_Start;
    public FlexibleColorPicker fcp_End;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        lineRenderer.startColor = fcp_Start.color;
        lineRenderer.endColor = fcp_End.color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCP_Line : MonoBehaviour
{
    public FlexibleColorPicker fcp_Start;
    public FlexibleColorPicker fcp_End;
    //public Material material;
    private LineRenderer lineRenderer;

    public Color externalColor;
    private Color internalColor;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        internalColor = externalColor;
    }

    private void Update()
    {
        //apply color of this script to the FCP whenever it is changed by the user
        if (internalColor != externalColor)
        {
            fcp_End.color = externalColor;
            fcp_Start.color = externalColor;
            internalColor = externalColor;
        }

        //extract color from the FCP and apply it to the object material
        lineRenderer.startColor = fcp_Start.color;
        lineRenderer.endColor = fcp_End.color;
    }
}

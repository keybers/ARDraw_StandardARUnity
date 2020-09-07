using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.SenseAR;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARCameraManager))]
public class SenseARGuesturePainter : SenseARGuesture
{

    [SerializeField]
    private Material mat = null;

    [SerializeField]
    private Text text;

    private ARCameraManager arCameraManager;
    private Camera m_camera;
    private XRCameraImage image;

    private bool hasImage = false;

    List<Vector3> fingerPoint;
    private StringBuilder stringBuilder;

    protected override void Start()
    {
        base.Start();
        arCameraManager = gameObject.GetComponent<ARCameraManager>();
        m_camera = gameObject.GetComponent<Camera>();
        fingerPoint = new List<Vector3>();
        stringBuilder = new StringBuilder();
    }

    public override void SetGuestureInfo(SenseARGuestureData guestureInfo)
    {
        base.SetGuestureInfo(guestureInfo);
        if (m_Manager != null)
        {
            m_Manager.GetGesture2DPoints();
        }

        stringBuilder.Clear();
        stringBuilder.Append("GestureType: ").Append(guestureInfo.HandGestureType.ToString()).Append("\n");
        stringBuilder.Append("HandSide: ").Append(guestureInfo.HandSide.ToString()).Append("\n");
        stringBuilder.Append("HandTowards: ").Append(guestureInfo.HandTowards.ToString()).Append("\n");
        stringBuilder.Append("Rect: ").Append(guestureInfo.Rect.ToString()).Append("\n");
        stringBuilder.Append("PalmCenter: ").Append(guestureInfo.PalmCenter.ToString()).Append("\n");
        stringBuilder.Append("PalmNormal: ").Append(guestureInfo.PalmNormal.ToString()).Append("\n");
        stringBuilder.Append("Confidence: ").Append(guestureInfo.Confidence.ToString());

        text.text = stringBuilder.ToString();
    }

    float ratiox;
    float ratioy;
    public override void SetPoints(List<Vector3> points)
    {
        base.SetPoints(points);
        if (arCameraManager == null)
            return;
        hasImage = false;
        image.Dispose();

        hasImage = arCameraManager.TryGetLatestImage(out image);

        ratiox = Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight ?
            (float)Screen.height / (float)Screen.width : 1f;
        ratioy = Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight ?
            1f : (float)Screen.width / (float)Screen.height;

    }

    public override void ClearInfos()
    {
        base.ClearInfos();
    }

    private void OnPostRender()
    {

        if (!hasImage)
            return;
        fingerPoint.Clear();

        if (m_Points == null || m_Points.Count == 0)
        {
            DrawFingers(fingerPoint);
            return;
        }

        int index;
        for (int i = 0; i < 5; i++)
        {
            fingerPoint.Clear();
            if (i != 0)
                fingerPoint.Add(SenseARGestureUtil.TrasnPosition2D(m_Points[0], image.width, image.height));
            for (int j = 0; j < 4; j++)
            {
                index = i * 4 + j;
                if (index >= m_Points.Count)
                    continue;

                fingerPoint.Add(SenseARGestureUtil.TrasnPosition2D(m_Points[index], image.width, image.height));
            }
            DrawFingers(fingerPoint);
        }

    }

    private void DrawFingers(List<Vector3> points)
    {
        if (points.Count < 2 || !hasImage || mat == null)
        {
            Debug.Log("clear lines");
            return;
        }
        float pointWidth;

        GL.PushMatrix();
        mat.SetPass(0);

        pointWidth = 0.02f;

        GL.LoadOrtho();

        for (int i = 0, len = points.Count; i < len; i++)
        {
            GL.Begin(GL.LINES);
            GL.Color(Color.green);
            GL.Vertex3(points[i].x, points[i].y, 0f);
            if (i + 1 != points.Count)
            {
                GL.Vertex3(points[i + 1].x, points[i + 1].y, 0f);
            }

            GL.End();

            GL.Begin(GL.QUADS);
            GL.Color(Color.red);
            GL.Vertex3(points[i].x - pointWidth * ratiox, points[i].y - pointWidth * ratioy, points[i].z);
            GL.Vertex3(points[i].x - pointWidth * ratiox, points[i].y + pointWidth * ratioy, points[i].z);
            GL.Vertex3(points[i].x + pointWidth * ratiox, points[i].y + pointWidth * ratioy, points[i].z);
            GL.Vertex3(points[i].x + pointWidth * ratiox, points[i].y - pointWidth * ratioy, points[i].z);
            GL.End();

        }

        
        GL.PopMatrix();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class ReferencePointCreator : MonoBehaviour
{
    public void RemoveAllReferencePoints()
    {
        foreach (var referencePoint in m_Anchors)
        {
            m_AnchorManager.RemoveAnchor(referencePoint);
        }
        m_Anchors.Clear();
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_Anchors = new List<ARAnchor>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            var referencePoint = m_AnchorManager.AddAnchor(hitPose);
            if (referencePoint == null)
            {
                Debug.Log("Error creating reference point");
            }
            else
            {
                m_Anchors.Add(referencePoint);
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_Anchors;

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;
}

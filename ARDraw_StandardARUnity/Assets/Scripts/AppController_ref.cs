using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AppController_ref : MonoBehaviour
{
    public GameObject spawnPrefab;
    static List<ARRaycastHit> Hits;
    private ARRaycastManager mRaycastManager;
    private AREnvironmentProbeManager mProbeManager;
    private AREnvironmentProbe mProbe;
    private GameObject spawnedObject = null;
    private void Awake()
    {
        Hits = new List<ARRaycastHit>();
        mRaycastManager = GetComponent<ARRaycastManager>();
        mProbeManager = GetComponent<AREnvironmentProbeManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;
        var touch = Input.GetTouch(0);
        if (mRaycastManager.Raycast(touch.position, Hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneWithinBounds))
        {
            var hitPose = Hits[0].pose;
            var probePose = hitPose;
            probePose.position.y += 0.2f;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(spawnPrefab, hitPose.position + new Vector3(0f, 0.05f, 0f), hitPose.rotation);
                mProbe = mProbeManager.AddEnvironmentProbe(probePose, new Vector3(0.6f, 0.6f, 0.6f), new Vector3(1.0f, 1.0f, 1.0f));
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
                mProbe.transform.position = hitPose.position;
            }
        }
    }
}

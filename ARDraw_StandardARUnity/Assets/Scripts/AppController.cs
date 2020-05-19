using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]//添加此脚本也添加ARRaycastManager组件
public class AppController : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject ARPlane;
    private GameObject spawnedObject = null;

    static List<ARRaycastHit> Hits;
    private ARRaycastManager aRRaycastManager;
    private float mARCoreAngle = 180f;
    // Start is called before the first frame update
    private void Start()
    {
        Hits = new List<ARRaycastHit>();
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;
        var touch = Input.GetTouch(0);
        if (aRRaycastManager.Raycast(touch.position, Hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneWithinBounds))
        {
            var hitPose = Hits[0].pose;
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                spawnedObject.transform.Rotate(Vector3.up, mARCoreAngle);
                var p = Instantiate(ARPlane, hitPose.position, hitPose.rotation);
                p.transform.parent = spawnedObject.transform;
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }
}

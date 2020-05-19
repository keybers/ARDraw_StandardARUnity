using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
//using UnityEngine.Experimental.XR;

public class ARTopToPlaceObject : MonoBehaviour
{
    public GameObject objectTopPlace;
    public GameObject placementIndicator;

    private ARRaycastManager aRRaycastManager;
    private ARSessionOrigin aROrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    // Start is called before the first frame update
    void Start()
    {
        aROrigin = FindObjectOfType<ARSessionOrigin>();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();//更新场地位置
        UpdatePlacementIndicator();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)//判断是否点击，点击是否是第一次
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectTopPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)//场地位置是否准备好
        {
            placementIndicator.SetActive(true);//场地指示出现了
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            //设置场地指示的位置和方向
        }
        else
        {
            placementIndicator.SetActive(false);//没发生
        }
    }

    private void UpdatePlacementPose()
    {
        var screentCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));//相机拍摄屏幕的中间位置中间
        var hits = new List<ARRaycastHit>();//点击屏幕存储为数组
        aRRaycastManager.Raycast(screentCenter, hits,TrackableType.Planes);//从屏幕中央射到能检测到的planes

        placementPoseIsValid = hits.Count > 0;//若果点击了就为true
        if (placementPoseIsValid)//调整场地位置朝向摄像机
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}

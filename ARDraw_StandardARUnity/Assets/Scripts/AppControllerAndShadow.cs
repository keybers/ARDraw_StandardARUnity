﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastHit))]
public class AppControllerAndShadow : MonoBehaviour
{
    public GameObject aRPlane;
    public GameObject placementIndicator;
    public GameObject objectToPlace;

    private Pose placementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private float mARCoreAngle = 0f;
    void Start()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }
    
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (Input.touchCount == 0)
            return;
        if (placementPoseIsValid && Input.touchCount > 0
            && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()//生成管理
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
        objectToPlace.transform.Rotate(Vector3.up, mARCoreAngle);
        var p = Instantiate(aRPlane, placementPose.position, placementPose.rotation);
        p.transform.parent = objectToPlace.transform;
    }

    private void UpdatePlacementIndicator()//开始先扫扫看看平面是否准备好
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

    private void UpdatePlacementPose()//调整标志平面位置随着摄像机中间摆动
    {
        var screentCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));//相机拍摄屏幕的中间位置中间
        var hits = new List<ARRaycastHit>();//点击屏幕存储为数组
        aRRaycastManager.Raycast(screentCenter, hits, TrackableType.Planes);//从屏幕中央射到能检测到的planes

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

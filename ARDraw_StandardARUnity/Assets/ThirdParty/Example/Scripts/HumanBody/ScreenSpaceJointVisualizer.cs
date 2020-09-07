using System;
using System.Text;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ScreenSpaceJointVisualizer : MonoBehaviour
{
    // 2D joint skeleton
    enum JointIndices
    {
        Invalid = -1,
        Root = 0, // parent: <none> [-1]
        RightUpLeg = 1, // parent: Root [0]
        RightLeg = 2, // parent: RightUpLeg [1]
        RightFoot = 3, // parent: RightLeg [2]
        LeftUpLeg = 4, // parent: Root [0]
        LeftLeg = 5, // parent: LeftUpLeg [4]
        LeftFoot = 6, // parent: LeftLeg [5]
        SpineMid = 7, // parent: Root [0]
        Neck = 8, // parent: SpineMid [7]
        Nose = 9, // parent: Head [10]
        Head = 10, // parent: Neck [8]
        LeftShoulder = 11, // parent: Neck [8]
        LeftForearm = 12, // parent: LeftShoulder [11]
        LeftHand = 13, // parent: LeftForearm [12]
        RightShoulder = 14, // parent: Neck [8]
        RightForearm = 15, // parent: RightShoulder [14]
        RightHand = 16, // parent: RightForearm [15]
    }

    [SerializeField]
    [Tooltip("The AR camera being used in the scene.")]
    Camera m_ARCamera;

    /// <summary>
    /// Get or set the <c>Camera</c>.
    /// </summary>
    public Camera arCamera
    {
        get { return m_ARCamera; }
        set { m_ARCamera = value; }
    }

    [SerializeField]
    [Tooltip("The ARHumanBodyManager which will produce human body anchors.")]
    ARHumanBodyManager m_HumanBodyManager;

    /// <summary>
    /// Get or set the <c>ARHumanBodyManager</c>.
    /// </summary>
    public ARHumanBodyManager humanBodyManager
    {
        get { return m_HumanBodyManager; }
        set { m_HumanBodyManager = value; }
    }

    [SerializeField]
    [Tooltip("A prefab that contains a LineRenderer component that will be used for rendering lines, representing the skeleton joints.")]
    GameObject m_LineRendererPrefab;

    /// <summary>
    /// Get or set the Line Renderer prefab.
    /// </summary>
    public GameObject lineRendererPrefab
    {
        get { return m_LineRendererPrefab; }
        set { m_LineRendererPrefab = value; }
    }

    Dictionary<int, GameObject> m_LineRenderers;
    static HashSet<int> s_JointSet = new HashSet<int>();

    void Awake()
    {
        m_LineRenderers = new Dictionary<int, GameObject>();
    }

    void UpdateRenderer(NativeArray<XRHumanBodyPose2DJoint> joints, int index)
    {
        GameObject lineRendererGO;
        if (!m_LineRenderers.TryGetValue(index, out lineRendererGO))
        {
            lineRendererGO = Instantiate(m_LineRendererPrefab, transform);
            m_LineRenderers.Add(index, lineRendererGO);
        }

        var lineRenderer = lineRendererGO.GetComponent<LineRenderer>();

        // Traverse hierarchy to determine the longest line set that needs to be drawn.
        var positions = new NativeArray<Vector2>(joints.Length, Allocator.Temp);
        try
        {
            var boneIndex = index;
            int jointCount = 0;
            while (boneIndex >= 0)
            {
                var joint = joints[boneIndex];
                if (joint.tracked)
                {
                    positions[jointCount++] = joint.position;
                    if (!s_JointSet.Add(boneIndex))
                        break;
                }
                else
                    break;

                boneIndex = joint.parentIndex;
            }

            // Render the joints as lines on the camera's near clip plane.
            lineRenderer.positionCount = jointCount;
            lineRenderer.startWidth = 0.001f;
            lineRenderer.endWidth = 0.001f;
            for (int i = 0; i < jointCount; ++i)
            {
                var position = positions[i];
                var worldPosition = m_ARCamera.ViewportToWorldPoint(
                    new Vector3(position.x, position.y, arCamera.nearClipPlane));
                lineRenderer.SetPosition(i, worldPosition);
            }
            lineRendererGO.SetActive(true);
        }
        finally
        {
            positions.Dispose();
        }
    }

    void Update()
    {
        Debug.Assert(m_HumanBodyManager != null, "Human body manager cannot be null");
        var joints = m_HumanBodyManager.GetHumanBodyPose2DJoints(Allocator.Temp);
        if (!joints.IsCreated || joints.Length == 0)
        {
            HideJointLines();
            return;
        }

        using (joints)
        {
            s_JointSet.Clear();
            for (int i = joints.Length - 1; i >= 0; --i)
            {
                if (joints[i].parentIndex != -1)
                    UpdateRenderer(joints, i);
            }
        }
    }

    void HideJointLines()
    {
        foreach (var lineRenderer in m_LineRenderers)
        {
            lineRenderer.Value.SetActive(false);
        }
    }
}

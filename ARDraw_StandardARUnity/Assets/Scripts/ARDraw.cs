using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Michsky.UI.ModernUIPack;

public enum RenderingMode
{
    Opaque,//不透明的
    Cutout,//剪切
    Fade,//渐变
    Transparent,//明亮
}

public class ARDraw : MonoBehaviour
{
    public GameObject LineObject;
    public GameObject DrawSpace;
    public GameObject SpaceStandard;
    public GameObject PlanStandard;
    public GameObject Double;

    public float DrawOffset;
    public float LineWidth;

    public FlexibleColorPicker fcp_Start;
    public FlexibleColorPicker fcp_End;

    [HideInInspector] public bool notDoubleDrawing;
    [HideInInspector] public Material CurrentMaterial; //白色Materials

    private ARRaycastManager aRRaycastManager;
    private ARReferencePointManager aRReferencePointManager;
    List<ARReferencePoint> aRReferencePoints;

    private Camera TargetCamera; //主相机
    private LineRenderer line;
    private LineRenderer A_line;
    private bool IsDrawing;
    private GameObject clone;
    private GameObject A_clone;
    private int touchCount;
    private int lineCounter; //笔画次数
    private int currentNum = 0;
    private Quaternion rotation = Quaternion.identity;
    private UIManager UIManager;

    void Awake()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        aRReferencePointManager = FindObjectOfType<ARReferencePointManager>();
        aRReferencePoints = new List<ARReferencePoint>();
        UIManager = GetComponent<UIManager>();
    }
    void Start()
    {
        FindObjectOfType<ARSession>().enabled = false;
        TargetCamera = Camera.main;

        //依据触摸的状态，确定绘画的状态

        IsDrawing = false;

        CurrentMaterial = Resources.Load("Materials/White", typeof(Material)) as Material;

        lineCounter = 0;

        Debug.Log(Screen.width + "*" + Screen.height);
    }

    void Update()
    {
        touchCount = Input.touchCount;
        if (touchCount == 0)
            return;
        if (touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit rayCastHit;
            if (!this.GetComponent<UIManager>().IsPointerOverUIObject())
            {

                if (UIManager.IsDraw)//空间上绘画
                {
                    //空间坐标没有平面或者点作为锚点
                    Vector3 touchPosition = TargetCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, DrawOffset));
                    Pose hitPose = new Pose(touchPosition, rotation);

                    ARDrawType(hitPose, false);
    
                }
                else if(UIManager.IsDrawOnPlan && Physics.Raycast(ray, out rayCastHit))
                    //在检测平面上绘画
                {

                    if (rayCastHit.collider.tag == "DP")
                    {
                        return;
                    }

                    Pose hitPose = new Pose(rayCastHit.point,rotation);

                    ARDrawType(hitPose, true);

                }
            }
        }
    }

    public void ARDrawType(Pose hitPose, bool notDoubleDrawing)
    {
        ARReferencePoint referencePoint = aRReferencePointManager.AddReferencePoint(hitPose);
        if (referencePoint == null)
        {
            Debug.Log("Error creating reference point");
        }
        else
        {
            aRReferencePoints.Add(referencePoint);
        }
        this.notDoubleDrawing = notDoubleDrawing;
        Drawing(hitPose.position, Input.GetTouch(0));

    }

    public void Drawing(Vector3 ARTouchPosition,Touch touch)
    {
        switch (touch.phase)
        {
            // 记录初始的点击位置
            case TouchPhase.Began:
                line = LineBegan(clone, line);

                if (UIManager.IsDoubleDrawing)
                {
                    A_line = LineBegan(A_clone, A_line);

                }

                lineCounter = 0;
                IsDrawing = true; //在画画
                break;

            case TouchPhase.Moved: //移动手指
                break;

            case TouchPhase.Stationary://长按
                break;

            case TouchPhase.Ended: //移出手指
                lineCounter = 0;
                currentNum++;

                IsDrawing = false;
                break;

            case TouchPhase.Canceled:
                break;
        }

        if (IsDrawing)
        {

            lineCounter++;
            line.positionCount = lineCounter;

            line.SetPosition(lineCounter - 1, ARTouchPosition);


            if (UIManager.IsDoubleDrawing && !notDoubleDrawing)
            {

                A_line.positionCount = lineCounter;
                A_line.SetPosition(lineCounter - 1, TargetCamera.ScreenToWorldPoint(new Vector3(Screen.width - touch.position.x, Screen.height - touch.position.y, DrawOffset)));//手机屏幕分辨率700,1650

            }
        }
    }

    public LineRenderer LineBegan(GameObject clone,LineRenderer line)
    {
        clone = Instantiate(LineObject); //实例化线段
        clone.tag = "ARLines "; //标上tag
        clone.GetComponent<LineRenderer>().material = CurrentMaterial; //附加上线段材料
        clone.transform.parent = DrawSpace.transform; //将位置依赖在drawSpace上

        line = clone.GetComponent<LineRenderer>(); //实例组件Line
        line.name = "line:" + currentNum; //名称
        line.startWidth = LineWidth; //线的开始宽度
        line.endWidth = LineWidth; //线的结束宽度
        line.startColor = fcp_Start.color;
        line.endColor = fcp_End.color;
        return line;
    }

    public void Paste()
    {
        int childCount = DrawSpace.transform.childCount;
        childCount = childCount - 1;
        Destroy(DrawSpace.transform.GetChild(childCount).gameObject);
    }

    public void ChangeTexure(string materialName)
    {
        CurrentMaterial = (Material)Instantiate(Resources.Load("Materials/" + materialName, typeof(Material)) as Material);
    }

    public void ChangeTexure(Material material)
    {

        SetMaterialRenderingMode(material, RenderingMode.Transparent);
        CurrentMaterial = (Material)Instantiate(material);

    }

    public void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

}

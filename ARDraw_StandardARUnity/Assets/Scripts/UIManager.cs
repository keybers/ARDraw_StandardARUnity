using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class UIManager : Singleton<UIManager>
{
    public enum GameState
    {
        MAIN,
        DRAWING,
        CREATECOLOR,
    }

    public List<CanvasGroup> MainMenuLists;
    public List<CanvasGroup> MenuLists;
    public List<CanvasGroup> MainMenuListsCanvasGroup;

    public Camera PreviewCamera;
    public Camera ARCamera;

    //public GameObject PreviewPrefabs;
    public GameObject SpaceStandard;
    public GameObject PlanStandard;
    public GameObject Double;

    [HideInInspector] public bool IsDraw;
    [HideInInspector] public bool IsDrawOnPlan;
    [HideInInspector] public bool IsDoubleDrawing;

    GameState _currentGameState = GameState.MAIN;

    void Start()
    {
        //Handheld.PlayFullScreenMovie("MP4/BeginMV.mp4", Color.black, FullScreenMovieControlMode.Hidden);

        SpaceStandard.GetComponent<SwitchManager>().isOn = false;
        PlanStandard.GetComponent<SwitchManager>().isOn = false;
        Double.GetComponent<SwitchManager>().isOn = false;
    }

    void Update()
    {
        IsDraw = SpaceStandard.GetComponent<SwitchManager>().isOn;
        IsDrawOnPlan = PlanStandard.GetComponent<SwitchManager>().isOn;
        IsDoubleDrawing = Double.GetComponent<SwitchManager>().isOn;
    }

    #region Popups
    public void ToggleColorPopup()
    {
        PopupStart(0);
    }

    public void ToggleMaterialPopup()
    {
        PopupStart(1);
    }

    public void ToggleBoxPopup()
    {
        SpaceStandard.GetComponent<SwitchManager>().isOn = IsDraw = false;
        PlanStandard.GetComponent<SwitchManager>().isOn = IsDrawOnPlan = false;
        SpaceStandard.GetComponent<Animator>().Play("Switch Off");
        PlanStandard.GetComponent<Animator>().Play("Switch Off");

        PopupStart(2);
    }

    public void ToggleTogglePopup()
    {
        PopupStart(3);
    }

    public void ToggleColorPlatePopup()
    {
        SpaceStandard.GetComponent<SwitchManager>().isOn = false;
        PlanStandard.GetComponent<SwitchManager>().isOn = false;
        Double.GetComponent<SwitchManager>().isOn = false;

        SpaceStandard.GetComponent<Animator>().Play("Switch Off");
        PlanStandard.GetComponent<Animator>().Play("Switch Off");
        Double.GetComponent<Animator>().Play("Switch Off");

        Material mat = Resources.Load("Materials/AnimatedLineRendererAlphaBlendMaterial", typeof(Material)) as Material;
        this.GetComponent<ARDraw>().ChangeTexure(mat);
        //替换Line的材料

        for (int i = 0; i < MenuLists.Count - 3; i++)
        {
            MenuLists[i].alpha = 0;
            MenuLists[i].interactable = false;
            MenuLists[i].blocksRaycasts = false;
        }
        PopupStart(4);

        PreviewCamera.tag = "MainCamera";
        ARCamera.enabled = false;
        PreviewCamera.enabled = true;
        //PreviewPrefabs.SetActive(true);

        UpdateState(GameState.CREATECOLOR);
    }

    private void PopupStart(int currentPopup)
    {
        for(int i = 0; i< MainMenuListsCanvasGroup.Count;i++)
        {
            if(i == currentPopup)
            {
                continue;
            }

            MainMenuListsCanvasGroup[i].alpha = 0;
            MainMenuListsCanvasGroup[i].interactable = false;
            MainMenuListsCanvasGroup[i].blocksRaycasts = false;
        }

        if (MainMenuListsCanvasGroup[currentPopup].alpha == 0)
        {
            MainMenuListsCanvasGroup[currentPopup].alpha = 1;
            MainMenuListsCanvasGroup[currentPopup].interactable = true;
            MainMenuListsCanvasGroup[currentPopup].blocksRaycasts = true;
        }
        else
        {
            MainMenuListsCanvasGroup[currentPopup].alpha = 0;
            MainMenuListsCanvasGroup[currentPopup].interactable = false;
            MainMenuListsCanvasGroup[currentPopup].blocksRaycasts = false;
        }
    }
    #endregion

    public void Back()
    {
        switch (CurrentGameState)
        {
            case GameState.DRAWING:
                for (int i = 0; i < MainMenuLists.Count; i++)
                {
                    MainMenuLists[i].alpha = 1;
                    MainMenuLists[i].interactable = true;
                    MainMenuLists[i].blocksRaycasts = true;
                }
                for (int i = 0; i < MenuLists.Count; i++)
                {
                    MenuLists[i].alpha = 0;
                    MenuLists[i].interactable = false;
                    MenuLists[i].blocksRaycasts = false;
                }
                for (int i = 0; i < MainMenuListsCanvasGroup.Count; i++)
                {
                    MainMenuListsCanvasGroup[i].alpha = 0;
                    MainMenuListsCanvasGroup[i].interactable = false;
                    MainMenuListsCanvasGroup[i].blocksRaycasts = false;
                }
                FindObjectOfType<ARSession>().enabled = false;
                UpdateState(GameState.MAIN);
                break;

            case GameState.CREATECOLOR:
                PreviewCamera.tag = "PreviewCamera";
                ARCamera.enabled = true;
                PreviewCamera.enabled = false;

                for (int i = 0; i < MenuLists.Count - 3; i++)
                {
                    MenuLists[i].alpha = 1;
                    MenuLists[i].interactable = true;
                    MenuLists[i].blocksRaycasts = true;
                }
                PopupStart(4);
                UpdateState(GameState.DRAWING);
                break;
            default:
                break;
        }
    }

    public void StartDraw()
    {
        SpaceStandard.GetComponent<SwitchManager>().isOn = false;
        PlanStandard.GetComponent<SwitchManager>().isOn = false;
        Double.GetComponent<SwitchManager>().isOn = false;
        SpaceStandard.GetComponent<Animator>().Play("Switch Off");
        PlanStandard.GetComponent<Animator>().Play("Switch Off");
        Double.GetComponent<Animator>().Play("Switch Off");

        for (int i = 0; i < MainMenuLists.Count; i++)
        {
            MainMenuLists[i].alpha = 0;
            MainMenuLists[i].interactable = false;
            MainMenuLists[i].blocksRaycasts = false;
        }
        for (int i = 0; i < MenuLists.Count; i++)
        {
            MenuLists[i].alpha = 1;
            MenuLists[i].interactable = true;
            MenuLists[i].blocksRaycasts = true;
        }

        FindObjectOfType<ARSession>().enabled = true;
        UpdateState(GameState.DRAWING);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public bool IsPointerOverUIObject()//判断当前鼠标上重叠的UI有多少个
    {
        //判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    public void PasteButton()
    {
        GetComponent<ARDraw>().Paste();
    }

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    void UpdateState(GameState state)
    {
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.MAIN:
                break;

            case GameState.DRAWING:
                break;

            case GameState.CREATECOLOR:
                break;

            default:
                break;
        }
    }
    public void SpaceStandardBt()
    {
        if (IsDrawOnPlan)
        {
            IsDrawOnPlan = !IsDrawOnPlan;
            PlanStandard.GetComponent<SwitchManager>().isOn = false;
            PlanStandard.GetComponent<Animator>().Play("Switch Off");
        }
    }

    public void PlanStandardBt()
    {
        if (IsDraw)
        {
            IsDraw = !IsDraw;
            SpaceStandard.GetComponent<SwitchManager>().isOn = false;
            SpaceStandard.GetComponent<Animator>().Play("Switch Off");
        }
    }

    public void PlayVedio()
    {
        Handheld.PlayFullScreenMovie("MP4/BeginMV.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
    }
}

// Copyright (C) 2015, 2016 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ricimi
{
    /// <summary>
    /// Basic button class used throughout the demo.
    /// </summary>
    public class ColorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler //指针进入，移出，按下，释放
    {
        public float fadeTime = 0.2f;
        public float onHoverAlpha;//先深
        public float onClickAlpha;//再浅

        //[Serializable]
        //public class ButtonClickedEvent : UnityEvent { }

        //[SerializeField]
        //private ButtonClickedEvent onClicked = new ButtonClickedEvent();

        private Color colorValue;

        private CanvasGroup canvasGroup;

        public void Start()
        {
            colorValue = transform.Find("Background").GetComponent<Image>().color;
        }

        public void onClicked()
        {
            GetComponent<AudioSource>().Play();
            //Debug.Log(colorValue);
            Material mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = colorValue;
            //GameObject.Find("Cube").GetComponent<MeshRenderer>().material = mat;
            GameObject.Find("Manager").GetComponent<ARDraw>().ChangeTexure(mat);

            for (int i=0;i< transform.parent.childCount; i++)
            {
                transform.parent.GetChild(i).Find("CheckedIcon").gameObject.SetActive(false);
            }
            transform.Find("CheckedIcon").gameObject.SetActive(true);
        }

        private void Awake()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
			StopAllCoroutines();
            StartCoroutine(Utils.FadeOut(canvasGroup, onHoverAlpha, fadeTime));
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

			StopAllCoroutines();
            StartCoroutine(Utils.FadeIn(canvasGroup, 1.0f, fadeTime));
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
				
			canvasGroup.alpha = onClickAlpha;
            onClicked();
            //onClicked.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
				
			canvasGroup.alpha = onHoverAlpha;
        }
    }
}

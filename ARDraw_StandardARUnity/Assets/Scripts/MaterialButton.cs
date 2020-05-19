using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ricimi
{
    public class MaterialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public float fadeTime = 0.2f;
        public float onHoverAlpha;
        public float onClickAlpha;
        public Material mat;

        private Color colorValue;

        private CanvasGroup canvasGroup;

        public void Start()
        {
            colorValue = transform.Find("Background").GetComponent<Image>().color;
        }

        public void onClicked()
        {
            GetComponent<AudioSource>().Play();
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

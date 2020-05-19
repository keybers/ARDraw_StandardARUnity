using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
    public class DemoTopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Animator buttonAnimator;

        void Start()
        {
            buttonAnimator = this.GetComponent<Animator>();
        }


        public void OnPointerEnter(PointerEventData eventData) //移入
        {
            if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed") && !buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
                buttonAnimator.Play("Normal to Hover");
        }

        public void OnPointerExit(PointerEventData eventData) //移出
        {
            if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed") && !buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
                buttonAnimator.Play("Hover to Normal");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Pressed
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
    }
}

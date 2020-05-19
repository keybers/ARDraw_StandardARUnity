using UnityEngine;
using UnityEngine.EventSystems;

public class TopButton : MonoBehaviour, IPointerUpHandler
{
    private Animator buttonAnimator;
    void Start()
    {
        buttonAnimator = this.GetComponent<Animator>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed Again"))
        {
            buttonAnimator.Play("Normal to Pressed Again");
        }
        else
        {
            buttonAnimator.Play("Normal to Pressed");
        }
    }
}
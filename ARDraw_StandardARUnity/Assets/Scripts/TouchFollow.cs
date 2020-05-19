using UnityEngine;

public class TouchFollow : MonoBehaviour
{

    public float speed = 8.0f;
    public float distanceFromCamera = 0.4f;
    public bool ignoreTimeScale;
    void Update()
    {
        if (FindObjectOfType<UIManager>().IsDraw)
        {
            Vector3 touchPosition = Input.GetTouch(0).position;
            touchPosition.z = distanceFromCamera;

            Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(touchPosition);

            float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;//
            Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * deltaTime));

            transform.position = position;
        }
    }
}


using UnityEngine;

public class VFXController : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    private UIManager UIManager;
    public float speed = 8.0f;
    public float distanceFromCamera = 5.0f;

    public bool ignoreTimeScale;
    void Start()
    {
        UIManager = FindObjectOfType<UIManager>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play(true);
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;
        Touch touch = Input.GetTouch(0);

        if (!UIManager.IsPointerOverUIObject())
        {
            Vector3 touchPosition = touch.position;
            touchPosition.z = distanceFromCamera;

            Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(touchPosition);

            float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
            Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * deltaTime));

            transform.position = position;
        }
    }
}

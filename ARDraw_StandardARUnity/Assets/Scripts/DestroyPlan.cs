using UnityEngine;

public class DestroyPlan : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}

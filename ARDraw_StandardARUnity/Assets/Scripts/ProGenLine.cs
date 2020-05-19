using UnityEngine;

public class ProGenLine : MonoBehaviour
{

    [SerializeField]
    [Range(0.0f,10.0f)]
    public float radius = 1.0f;

    [SerializeField]
    public Vector3 randomOffSet = Vector3.zero;

    [SerializeField]
    [Range(0.00f,10.0f)]
    public float lineStartWidth = 0.05f;

    [SerializeField]
    [Range(0.00f, 10.0f)]
    public float lineEndWidth = 0.05f;

    [SerializeField]
    [Range(0, 1000)]
    public int howManyPoints = 100;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float upDateInSeconds = 0.5f;

    //[SerializeField]
    //[Range(-10.0f, 10.0f)]
    private float heartX;

    //[SerializeField]
    //[Range(-10.0f, 10.0f)]
    private float heartY;

    //[SerializeField]
    //[Range(-10.0f, 10.0f)]
    private float heartZ;

    private float internalTimer = 0;
    private LineRenderer LineRenderer;

    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    void DrawCircle()
    {
        LineRenderer.positionCount = howManyPoints + 1;
        LineRenderer.startWidth = lineStartWidth;
        LineRenderer.endWidth = lineEndWidth;

        Vector3 heartPosition = gameObject.transform.position;
        heartX = heartPosition.x / 2;
        heartY = heartPosition.y / 2;
        heartZ = heartPosition.z;

        for (int i = 0; i < howManyPoints; i++)
        {
            float angle = 2f * Mathf.PI / (float)howManyPoints * i;

            float LineXPosition = Mathf.Cos(angle) * radius + heartX;
            //float LineYPosition = Mathf.???(angle) * radius;
            float LineYPosition = Mathf.Sin(angle) * radius + heartY;
            float LineZPosition = heartZ;

            float offsetX = Random.Range(LineXPosition - randomOffSet.x, LineXPosition + randomOffSet.x); //(LineYPosition * randomOffSet.x)中心位置，randomOffSet.x没有混乱
            float offsetY = Random.Range(LineYPosition - randomOffSet.y, LineYPosition + randomOffSet.y);
            //float offsetZ = Random.Range(LineZPosition - randomOffSet.z, LineZPosition + randomOffSet.z);//(LineYPosition * randomOffSet.z)



            LineRenderer.SetPosition(i, new Vector3(LineXPosition + offsetX, LineYPosition + offsetY, LineZPosition));

            //LineRenderer.SetPosition(i, new Vector3(LineXPosition - offsetX, 0, LineZPosition - offsetZ));美妙邪眼

        }
        LineRenderer.SetPosition(howManyPoints
            ,new Vector3(LineRenderer.GetPosition(0).x, LineRenderer.GetPosition(0).y, LineRenderer.GetPosition(0).z));
    }

    // Update is called once per frame
    void Update()
    {
        //SetHeartPosition();
        if (internalTimer >= upDateInSeconds)
        {
            DrawCircle();
            internalTimer = 0;
        }
        else
        {
            internalTimer += Time.deltaTime * 1.0f;
        }
    }

}

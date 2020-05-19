using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
public class DemoOne_Test : MonoBehaviour
{
   

    [FormerlySerializedAs("hp")]
    public int hp = 1;

    private void Start()
    {
        a();
    }

    IEnumerable b()
    {
        yield return 0;
    }
    IEnumerable a()
    {
        yield return b();
    }

}

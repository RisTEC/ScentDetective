using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public static SpotlightController Instance;

    public Light spotLight;
    public float fixedHeight = 4f;   

    Transform target;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(
            target.position.x,
            fixedHeight,
            target.position.z
        );

        spotLight.transform.position = targetPos;


        // Optional: always point down
        spotLight.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    public void FocusOn(Transform t)
    {
        target = t;
        spotLight.enabled = true;
    }

    public void ClearFocus()
    {
        target = null;
        spotLight.enabled = false;
    }
}

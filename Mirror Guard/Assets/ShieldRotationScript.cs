using UnityEngine;

public class ShieldRotationScript : MonoBehaviour
{
    public Transform centerPoint; // The point to rotate around
    public float scrollSensitivity = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            float scroll = Input.mouseScrollDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
    float scroll = Input.mouseScrollDelta.y;
    transform.RotateAround(centerPoint.position, Vector3.forward, scroll * scrollSensitivity);
    }
}

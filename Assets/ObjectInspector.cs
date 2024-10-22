using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float damping = 5f;
    private float inertia = 0f;
    private bool isDragging = false;
    private float lastInputX;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastInputX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.position.x - lastInputX;
                inertia = deltaX * rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, -inertia, Space.World);
                lastInputX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                lastInputX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                float deltaX = Input.mousePosition.x - lastInputX;
                inertia = deltaX * rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, -inertia, Space.World);
                lastInputX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        if (!isDragging)
        {
            if (Mathf.Abs(inertia) > 0.01f)
            {
                transform.Rotate(Vector3.up, -inertia, Space.World);
                inertia = Mathf.Lerp(inertia, 0f, damping * Time.deltaTime);
            }
            else
            {
                inertia = 0f;
            }
        }
    }
}
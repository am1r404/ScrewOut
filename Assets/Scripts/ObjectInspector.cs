using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float verticalRotationLimit = 30f;
    public float inertiaDecay = 0.95f;

    private Vector2 angularVelocity = Vector2.zero;
    private bool isDragging = false;
    private float lastInputX;
    private float lastInputY;

    private float horizontalAngle = 0f;
    private float verticalAngle = 0f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleInput(touch.position.x, touch.position.y, touch.phase == TouchPhase.Began, touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                HandleInput(Input.mousePosition.x, Input.mousePosition.y, true, false);
            else if (Input.GetMouseButton(0))
                HandleInput(Input.mousePosition.x, Input.mousePosition.y, false, false);
            else if (Input.GetMouseButtonUp(0))
                HandleInput(Input.mousePosition.x, Input.mousePosition.y, false, true);
        }

        if (!isDragging && angularVelocity.sqrMagnitude > 0.0001f)
        {
            ApplyInertia();
            angularVelocity *= inertiaDecay;
        }
    }

    private void HandleInput(float inputX, float inputY, bool inputBegan, bool inputEnded)
    {
        if (inputBegan)
        {
            isDragging = true;
            lastInputX = inputX;
            lastInputY = inputY;
            angularVelocity = Vector2.zero;
        }
        else if (isDragging)
        {
            float deltaX = inputX - lastInputX;
            float deltaY = inputY - lastInputY;

            float verticalCorrectionFactor = Mathf.Cos(horizontalAngle * Mathf.Deg2Rad);
            
            angularVelocity.x = -deltaX;
            angularVelocity.y = deltaY * Mathf.Sign(verticalCorrectionFactor);

            HandleRotation(angularVelocity.x * Time.deltaTime, angularVelocity.y * Time.deltaTime);

            lastInputX = inputX;
            lastInputY = inputY;
        }

        if (inputEnded)
            isDragging = false;
    }

    private void HandleRotation(float horizontalRotation, float verticalRotation)
    {
        horizontalAngle += horizontalRotation * rotationSpeed;
        horizontalAngle = Mathf.Repeat(horizontalAngle + 180f, 360f) - 180f;
        
        verticalAngle += verticalRotation * rotationSpeed;
        verticalAngle = Mathf.Clamp(verticalAngle, -verticalRotationLimit, verticalRotationLimit);

        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
        transform.rotation = rotation;
    }

    private void ApplyInertia()
    {
        HandleRotation(angularVelocity.x * Time.deltaTime, angularVelocity.y * Time.deltaTime);
    }
}
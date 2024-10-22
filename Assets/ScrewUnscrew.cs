using UnityEngine;

public class ScrewUnscrew : MonoBehaviour
{
    public float rotationSpeed = 100f; // degrees per second
    public float moveUpSpeed = 0.1f;   // units per second
    public float totalRotation = 720f; // degrees
    public float totalMoveUp = 2f;     // units

    private bool isUnscrewing = false;
    private float rotatedAmount = 0f;
    private float movedUpAmount = 0f;

    void Update()
    {
        // Handle mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the raycast hit this object
                if (hit.transform == transform)
                {
                    isUnscrewing = true;
                }
            }
        }

        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        isUnscrewing = true;
                    }
                }
            }
        }

        if (isUnscrewing)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            float moveUpThisFrame = moveUpSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, rotationThisFrame, Space.Self);
            transform.Translate(Vector3.up * moveUpThisFrame, Space.Self);

            rotatedAmount += rotationThisFrame;
            movedUpAmount += moveUpThisFrame;

            if (rotatedAmount >= totalRotation || movedUpAmount >= totalMoveUp)
            {
                isUnscrewing = false;
            }
        }
    }
}

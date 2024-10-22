using UnityEngine;

public class Screw : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float moveUpSpeed = 0.3f;
    public float totalRotation = 720f;
    public float totalMoveUp = 2f;

    private bool isUnscrewing = false;
    private float rotated = 0f;
    private float movedUp = 0f;

    void OnMouseDown()
    {
        isUnscrewing = true;
    }

    void Update()
    {
        if (isUnscrewing)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            float moveUpThisFrame = moveUpSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, -rotationThisFrame, Space.Self);
            transform.Translate(Vector3.up * moveUpThisFrame, Space.Self);

            rotated += rotationThisFrame;
            movedUp += moveUpThisFrame;

            if (rotated >= totalRotation || movedUp >= totalMoveUp)
            {
                isUnscrewing = false;
            }
        }
    }
}

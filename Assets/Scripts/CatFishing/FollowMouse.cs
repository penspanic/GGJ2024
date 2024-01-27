using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        targetPosition.z = 0f;
        transform.position = targetPosition;
    }
}
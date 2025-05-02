using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // 플레이어
    public Vector3 offset = new Vector3(0, 2, -4); // 카메라 위치 오프셋
    public float mouseSensitivity = 3f;
    public float distance = 0.5f;
    public float rotationSmoothTime = 0.12f;

    float yaw; // 좌우 회전값
    float pitch; // 상하 회전값

    Vector3 currentRotation;
    Vector3 rotationSmoothVelocity;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -35f, 60f); // 위아래 회전 제한

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distance + offset;
    }
}

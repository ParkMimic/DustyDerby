using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // �÷��̾�
    public Vector3 offset = new Vector3(0, 2, -4); // ī�޶� ��ġ ������
    public float mouseSensitivity = 3f;
    public float distance = 0.5f;
    public float rotationSmoothTime = 0.12f;

    float yaw; // �¿� ȸ����
    float pitch; // ���� ȸ����

    Vector3 currentRotation;
    Vector3 rotationSmoothVelocity;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -35f, 60f); // ���Ʒ� ȸ�� ����

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distance + offset;
    }
}

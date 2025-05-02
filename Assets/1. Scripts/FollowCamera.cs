using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // ���� ���
    public Vector3 offset; // ������ �Ÿ�

    void Update()
    {
        if (target != null) // ����� ������ ���� ����
        {
            transform.position = target.position + offset; // ���󰡱�
        }
    }
}

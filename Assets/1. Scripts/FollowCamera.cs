using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // 따라갈 대상
    public Vector3 offset; // 대상과의 거리

    void Update()
    {
        if (target != null) // 대상이 없으면 실행 안함
        {
            transform.position = target.position + offset; // 따라가기
        }
    }
}

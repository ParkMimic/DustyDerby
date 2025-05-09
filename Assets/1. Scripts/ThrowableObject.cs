using System.Threading;
using UnityEngine;

// 던져지는 오브젝트 스크립트 (ThrowableObject.cs)
public class ThrowableObject : MonoBehaviour
{
    private bool isThrown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown == false) return; // 안 던졌으면 충돌 무시
        Slime slime = collision.gameObject.GetComponent<Slime>();
        if (slime != null)
        {
            slime.Stun();
        }

        isThrown = false; // 한번 기절시키고 초기화
    }
}


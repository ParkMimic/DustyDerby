using System.Threading;
using UnityEngine;

// 던져지는 오브젝트 스크립트 (ThrowableObject.cs)
public class ThrowableObject : MonoBehaviour
{
    public bool isThrown;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;
        Debug.Log("충돌 감지됨: " + collision.gameObject.name);

        Slime slime = collision.gameObject.GetComponent<Slime>();
        if (slime != null)
        {
            Debug.Log("슬라임 발견, 기절 시도");
            slime.Stun();
        }

        isThrown = false;
    }
}


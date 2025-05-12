using System.Threading;
using UnityEngine;

// 던져지는 오브젝트 스크립트 (ThrowableObject.cs)
public class ThrowableObject : MonoBehaviour
{
    public bool isThrown;
    public ParticleSystem hitEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;

        Slime slime = collision.gameObject.GetComponent<Slime>();
        if (slime != null)
        {
            if (collision.contacts.Length > 0)
            {
                // 충돌 처리
                Vector3 hitPoint = collision.contacts[0].point;
                Quaternion hitRotation = Quaternion.LookRotation(collision.contacts[0].normal);

                hitEffectPrefab.transform.position = hitPoint;
                hitEffectPrefab.transform.rotation = hitRotation;

                ParticleSystem effect = Instantiate(hitEffectPrefab, hitPoint, hitRotation);

                // 이펙트 실행
                effect.Play();

                Destroy(effect.gameObject, 1f);
            }

            slime.Stun();
        }

        isThrown = false;
    }
}


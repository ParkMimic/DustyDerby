using System.Threading;
using UnityEngine;

// �������� ������Ʈ ��ũ��Ʈ (ThrowableObject.cs)
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
                // �浹 ó��
                Vector3 hitPoint = collision.contacts[0].point;
                Quaternion hitRotation = Quaternion.LookRotation(collision.contacts[0].normal);

                hitEffectPrefab.transform.position = hitPoint;
                hitEffectPrefab.transform.rotation = hitRotation;

                ParticleSystem effect = Instantiate(hitEffectPrefab, hitPoint, hitRotation);

                // ����Ʈ ����
                effect.Play();

                Destroy(effect.gameObject, 1f);
            }

            slime.Stun();
        }

        isThrown = false;
    }
}


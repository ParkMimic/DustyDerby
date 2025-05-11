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
            Vector3 hitPoint = collision.contacts[0].point;
            Quaternion hitRotation = Quaternion.LookRotation(collision.contacts[0].normal);

            hitEffectPrefab.Play();

            slime.Stun();
        }

        isThrown = false;
    }
}


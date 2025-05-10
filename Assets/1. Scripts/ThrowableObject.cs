using System.Threading;
using UnityEngine;

// �������� ������Ʈ ��ũ��Ʈ (ThrowableObject.cs)
public class ThrowableObject : MonoBehaviour
{
    public bool isThrown;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;
        Debug.Log("�浹 ������: " + collision.gameObject.name);

        Slime slime = collision.gameObject.GetComponent<Slime>();
        if (slime != null)
        {
            Debug.Log("������ �߰�, ���� �õ�");
            slime.Stun();
        }

        isThrown = false;
    }
}


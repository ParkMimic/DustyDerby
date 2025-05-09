using System.Threading;
using UnityEngine;

// �������� ������Ʈ ��ũ��Ʈ (ThrowableObject.cs)
public class ThrowableObject : MonoBehaviour
{
    private bool isThrown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown == false) return; // �� �������� �浹 ����
        Slime slime = collision.gameObject.GetComponent<Slime>();
        if (slime != null)
        {
            slime.Stun();
        }

        isThrown = false; // �ѹ� ������Ű�� �ʱ�ȭ
    }
}


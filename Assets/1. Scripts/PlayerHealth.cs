using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerHealth : LivingEntity
{
    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ���κ��� IItem ������Ʈ �������� �õ�
        IItem item = other.GetComponent<IItem>();
        // IItem ������Ʈ�� �������µ��� �����ߴٸ�
        if (item != null)
        {
            // Use �ż��带 ����� ����
            item.Use(gameObject);
        }
    }
}

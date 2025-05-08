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
        // 충돌한 상대로부터 IItem 컴포넌트 가져오기 시도
        IItem item = other.GetComponent<IItem>();
        // IItem 컴포넌트를 가져오는데에 성공했다면
        if (item != null)
        {
            // Use 매서드를 사용해 실행
            item.Use(gameObject);
        }
    }
}

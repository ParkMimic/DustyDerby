using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public bool isStun { get; private set; }

    protected virtual void OnEnable()
    {
        // 기절하지 않은 상태로 시작
        isStun = false;
    }
    
    public virtual void Stun()
    {
        if (isStun != true)
        {
            isStun = true;
        }
    }
}

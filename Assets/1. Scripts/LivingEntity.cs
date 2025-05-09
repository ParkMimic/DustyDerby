using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public bool isStun { get; private set; }

    protected virtual void OnEnable()
    {
        // �������� ���� ���·� ����
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

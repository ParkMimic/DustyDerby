using UnityEngine;

public interface IDamageable
{
    void OnDamage(Vector3 hitPosition, Vector3 hitNormal);
}

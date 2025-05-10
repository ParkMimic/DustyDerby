using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour
{
    public bool isStun { get; protected set; }

    private Coroutine stunCoroutine;

    protected virtual void OnEnable()
    {
        isStun = false;
    }

    public virtual void Stun()
    {
        if (!isStun)
        {
            if (stunCoroutine != null)
            {
                StopCoroutine(stunCoroutine);
            }
            stunCoroutine = StartCoroutine(StunWait());
        }
    }

    IEnumerator StunWait()
    {
        isStun = true;
        yield return new WaitForSeconds(2f);
        isStun = false;
    }
}
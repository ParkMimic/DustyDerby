using System.Threading;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster")
        {
            Slime slime = collision.gameObject.GetComponent<Slime>();
            if (slime != null)
            {
                slime.Stun();
            }
        }
    }
}

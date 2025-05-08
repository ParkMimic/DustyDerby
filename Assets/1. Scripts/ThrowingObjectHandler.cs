using UnityEngine;
using System.Collections;

public class ThrowingObjectHandler : MonoBehaviour
{
    public Transform holdPosition; // 머리 위 위치
    public float throwForce = 10f;
    private GameObject heldObject;

    void Update()
    {
        if (heldObject != null && Input.GetMouseButtonDown(0))
        {
            ThrowObject();
            Debug.Log("던지기 시도");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (heldObject == null && other.collider.tag == "Throwable")
        {
            PickUpObject(other.gameObject);
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        obj.transform.SetParent(holdPosition);
        obj.transform.localPosition = Vector3.up * 1.5f;
    }

    void ThrowObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        heldObject.transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        // 태그 잠시 변경
        heldObject.tag = "Untagged"; // 나중에 다시 바꿀 태그
        StartCoroutine(ResetTag(heldObject, 0.5f)); // 0.5초 후 복원

        heldObject = null;
    }

    IEnumerator ResetTag(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null) obj.tag = "Throwable";
    }
}

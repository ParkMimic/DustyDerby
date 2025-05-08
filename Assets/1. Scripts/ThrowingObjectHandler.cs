using UnityEngine;
using System.Collections;

public class ThrowingObjectHandler : MonoBehaviour
{
    public Transform holdPosition; // �Ӹ� �� ��ġ
    public float throwForce = 10f;
    private GameObject heldObject;

    void Update()
    {
        if (heldObject != null && Input.GetMouseButtonDown(0))
        {
            ThrowObject();
            Debug.Log("������ �õ�");
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

        // �±� ��� ����
        heldObject.tag = "Untagged"; // ���߿� �ٽ� �ٲ� �±�
        StartCoroutine(ResetTag(heldObject, 0.5f)); // 0.5�� �� ����

        heldObject = null;
    }

    IEnumerator ResetTag(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null) obj.tag = "Throwable";
    }
}

using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    public AudioSource getCoin;
    public AudioClip GetCoin;

    public int score = 1;
    public void Use(GameObject target)
    {
        // ���� �Ŵ����� �����Ͽ� ���� �߰�
        GameManager.instance.AddScore(score);
        // ��� �Ǿ����� ���� �ı�
        Destroy(gameObject);
    }
}

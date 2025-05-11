using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    public AudioSource getCoin;
    public AudioClip GetCoin;

    public int score = 1;
    public void Use(GameObject target)
    {
        // 게임 매니저에 접근하여 점수 추가
        GameManager.instance.AddScore(score);
        // 사용 되었으니 코인 파괴
        Destroy(gameObject);
    }
}

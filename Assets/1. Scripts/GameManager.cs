using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글턴 변수에 오브젝트가 할당 되어있지 않다면
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<GameManager>();
            }
            // 싱글턴 오브젝트 반환
            return m_instance;
        }
    }

    // 싱글턴이 할당 될 instance 변수
    private static GameManager m_instance;

    private int score = 0; // 현재 게임 점수
    public bool isStun { get; private set; }

    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int newScore)
    {
        // 점수 추가
        score += newScore;

    }
}

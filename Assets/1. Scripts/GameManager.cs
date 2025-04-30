using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ������Ʈ�� �Ҵ� �Ǿ����� �ʴٸ�
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<GameManager>();
            }
            // �̱��� ������Ʈ ��ȯ
            return m_instance;
        }
    }

    // �̱����� �Ҵ� �� instance ����
    private static GameManager m_instance;

    private int score = 0; // ���� ���� ����
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
        // ���� �߰�
        score += newScore;

    }
}

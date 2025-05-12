using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<UIManager>();
            }
            return m_instance;
        }
    }

    private static UIManager m_instance; // �̱����� �Ҵ�� ����

    public Text scoreText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public GameObject winUI; // �¸� UI Ȱ��ȭ�� ���� ���� ������Ʈ

    // ���� �ؽ�Ʈ ����
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = newScore + " / 15";
    }
}

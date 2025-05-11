using JetBrains.Annotations;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int winScore;
    public GameObject winUI;

    public AudioClip coinClip;
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
    }

    public void AddScore(int newScore)
    {
        
        // ���� �߰�
        score += newScore;
        audioSource.PlayOneShot(coinClip);

        if (score >= winScore)
        {
            Win();
        }

        // ���� UI �ؽ�Ʈ ����
        UIManager.instance.UpdateScoreText(score);
    }

    void Win()
    {
        if (score >= winScore)
        {
            winUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Room_Unity");
        Time.timeScale = 1;
    }

    public void Title()
    {
        SceneManager.LoadScene("Intro");
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}

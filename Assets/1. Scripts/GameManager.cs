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

        audioSource = GetComponent<AudioSource>();
    }

    public void AddScore(int newScore)
    {
        
        // 점수 추가
        score += newScore;
        audioSource.PlayOneShot(coinClip);

        if (score >= winScore)
        {
            Win();
        }

        // 점수 UI 텍스트 갱신
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

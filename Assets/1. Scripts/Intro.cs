using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Animation animation;
    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.anyKeyDown == true)
        {
            SceneManager.LoadScene("Room_Unity");
        }
    }
}

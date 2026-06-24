using UnityEngine;

public class PauseControl : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    public bool isPaused { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPaused = true;
        TogglePause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        pauseUI.SetActive(isPaused);
    }
}

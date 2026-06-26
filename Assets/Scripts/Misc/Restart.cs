using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;

    public void LoadScene(int index)
    {
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene(index);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenePowers : MonoBehaviour
{
    float delay = 2f;
    [SerializeField] GameObject blackScreen;
    [SerializeField] AudioClip[] apogee;
    [SerializeField] AudioSource music;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int buildIndex)
    {
        if (buildIndex == 2)
        {
            StartCoroutine(BeginApogee());
        } else
        {
            SceneManager.LoadScene(buildIndex);
        }
        
    }

    public IEnumerator BeginApogee()
    {
        blackScreen.SetActive(true);
        music.Stop();
        music.clip = apogee[0];
        music.Play();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }
}

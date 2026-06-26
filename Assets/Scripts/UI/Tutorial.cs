using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] tutorial;
        int currentInx = 0;
    [SerializeField] GameObject tutText;
    //0 is next Tut
    [SerializeField] AudioClip[] sfx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            NextTut();
            SoundFXManager.instance.PlaySoundEffectClip(sfx[0], transform.position, 1f);
        }
    }

    void NextTut()
    {
        if (currentInx >= 0 && currentInx < tutorial.Length)
        {
            tutorial[currentInx].SetActive(false);
            currentInx++;

            if (currentInx < tutorial.Length)
            {
                tutorial[currentInx].SetActive(true);

            }

        }

        if (currentInx >= (tutorial.Length))
        {
            tutText.SetActive(false);
            SceneManager.LoadScene(2);
            //tutorial over
        }
    }
}

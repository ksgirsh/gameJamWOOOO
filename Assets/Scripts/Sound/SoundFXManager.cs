using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] AudioSource SFXObject;
    [SerializeField] Camera mc;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (mc == null)
        {
            mc = (GameObject.FindGameObjectWithTag("MainCamera")).GetComponent<Camera>();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySoundEffectClip(AudioClip audioClip, Vector3 spawnPoint, float volume)
    {
        //spawn gameObject
        AudioSource audioSource = Instantiate(SFXObject, spawnPoint, Quaternion.identity);

        //assign Clip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;
        //play clip
        audioSource.Play();

        // get legth of clip
        float clipLength = audioSource.clip.length;

        //destroy clip after set time
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundEffectClip(AudioClip[] audioClip, Vector3 spawnPoint, float volume)
    {

        // random index
        int rand = Random.Range(0, audioClip.Length);



        //spawn gameObject
        AudioSource audioSource = Instantiate(SFXObject, spawnPoint, Quaternion.identity);

        //assign Clip
        audioSource.clip = audioClip[rand];

        //assign volume
        audioSource.volume = volume;
        //play clip
        audioSource.Play();

        // get legth of clip
        float clipLength = audioSource.clip.length;

        //destroy clip after set time
        Destroy(audioSource.gameObject, clipLength);
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WaveController : MonoBehaviour
{
    [Header("UI")]
    //in seconds
    [SerializeField] TextMeshProUGUI timerText;
    bool ticking = false;

    [Header("Invasion")]
    bool invasion;
    int currentInvasions = 0;
    [SerializeField] int initAlienCount;
    [SerializeField] int invasionScaling;
    [SerializeField] int setTimeTillNextWave = 300;
    private int timeTillNextWave = 300;

    [Header("Aliens")]
    [SerializeField] GameObject baseAlien;
    public List<GameObject> currentlyAliveAliens;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invasion = false;
        timeTillNextWave = setTimeTillNextWave;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (timeTillNextWave > 0 && !invasion && !ticking)
        {
            StartCoroutine("Tick");

        }
        else if (!invasion && !ticking)
        {
            TriggerInvasion();
        }

        if (invasion && currentlyAliveAliens.Count == 0)
        {
            invasion = false;
            timeTillNextWave = setTimeTillNextWave;
        }
    }

    IEnumerator Tick()
    {
        ticking = true;
        yield return new WaitForSeconds(1f);
        timeTillNextWave--;
        UpdateText();
        ticking = false;
    }

    void UpdateText()
    {
        int minutes = (int)Mathf.Ceil((timeTillNextWave) / 60);
        int seconds = (timeTillNextWave % 60);

        string secondString = (seconds.ToString());
        if (seconds < 10)
        {
            secondString = string.Concat("0", secondString);
        }

        
        string write = string.Concat((minutes.ToString()), ":", (secondString));
        timerText.text = write;

    }

    void TriggerInvasion()
    {
        invasion = true;
        currentInvasions++;
        int alienCount = initAlienCount * (invasionScaling * currentInvasions);

        for (int i = 0; i < alienCount; i++)
        {
            GameObject alien = GameObject.Instantiate(baseAlien, transform.position, Quaternion.identity);
            currentlyAliveAliens.Add(alien);
        }

    }
}

using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public float health;
    public float currentHealth;
    public bool decay;

    //0 is satellite, 1 is rocket, 2 is target, 3 is alien, 4 is planet
    bool[] isOn = new bool[5];
    SelectControl select;
    FadeIn fade;
    WaveController alienControl;

    [SerializeField] float invincibleTime = 0.8f;

    bool isInvincible;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        select = player.GetComponent<SelectControl>();
        fade = gameObject.GetComponent<FadeIn>();

        float randomChange = Random.Range(-(health * 0.2f), (health * 0.4f));
        health += randomChange;
        currentHealth = health;

        if (gameObject.GetComponent<Satellite>() != null)
        {
            isOn[0] = true;
        }

        if (gameObject.GetComponent<TargetAttribute>() != null)
        {
            isOn[2] = true;


        }

        if (gameObject.GetComponent<Alien>() != null)
        {
            isOn[3] = true;
            alienControl = player.GetComponent<WaveController>();

        }
        isInvincible = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (decay)
        {
            currentHealth -= Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            
            if (isOn[0])
            {
                foreach(GameObject hook in select.lockedHooks)
                {
                    if (hook == gameObject)
                    {
                        select.EraseLockOn((select.lockedHooks.IndexOf(hook)));
                        break;
                    }
                }

                GameObject ring = (gameObject.GetComponent<Satellite>().orbitRing.gameObject);
                Destroy(ring);
            }

            if (isOn[2])
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                TargetSpawn targetManager = player.GetComponent<TargetSpawn>();
                targetManager.currentTargets--;
            }

            if (isOn[3])
            {
                alienControl.currentlyAliveAliens.Remove(gameObject);
            }

            Destroy(gameObject);
        }
    }

    public float NormalizedHealth()
    {
        return (currentHealth / health);
        
    }


    public IEnumerator TakeDamage(float damage)
    {
        //play hurt sound effect here

            

            if (isInvincible == false)
            {
                isInvincible = true;
                StartCoroutine(fade.PulseColorSpr(0.4f, gameObject, Color.red));
                currentHealth -= damage;
                yield return new WaitForSeconds(invincibleTime);
            }


            isInvincible = false;
       
        

    }
}

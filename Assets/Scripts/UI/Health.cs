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

    //0 is hook damage, 1 is hook destroy, 2 is planet damage, 3 is alien damage, 4 is alien destroy
    [Header("Sound")]
    [SerializeField] AudioClip[] sfx;

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

        if (gameObject.tag == "Planet")
        {
            isOn[4] = true;
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
                //play hook destroy sfx
                SoundFXManager.instance.PlaySoundEffectClip(sfx[1], transform.position, 1f);
                foreach (GameObject hook in select.lockedHooks)
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
                //play alien dmg sfx
                SoundFXManager.instance.PlaySoundEffectClip(sfx[4], transform.position, 1f);

                alienControl.currentlyAliveAliens.Remove(gameObject);

            }

            if (isOn[4])
            {
                ((transform.parent).gameObject.GetComponent<Planet>()).StartCoroutine((transform.parent).gameObject.GetComponent<Planet>().EarthDestroy());
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
            
        if ((currentHealth - damage) > 0)
        {
            for (int i = 0; i < isOn.Length; i++)
            {
                if (isOn[i] == true)
                {
                    switch (i)
                    {
                        case 0:
                            //play hook damage sfx
                            SoundFXManager.instance.PlaySoundEffectClip(sfx[0], transform.position, 1f);
                            break;
                        case 3:
                            //play alien damage sfx
                            SoundFXManager.instance.PlaySoundEffectClip(sfx[3], transform.position, 1f);
                            break;
                        case 4:
                            //play planet damage sfx
                            SoundFXManager.instance.PlaySoundEffectClip(sfx[2], transform.position, 1f);
                            break;
                    }
                }
            }
        }

        

    }
}

using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float currentHealth;
    public bool decay;

    bool isOnSatellite;
    SelectControl select;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = health;
        if (gameObject.GetComponent<Satellite>() != null)
        {
            isOnSatellite = true;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        select = player.GetComponent<SelectControl>();
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
            
            if (isOnSatellite)
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

            Destroy(gameObject);
        }
    }

    public float NormalizedHealth()
    {
        return (currentHealth / health);
        
    }
}

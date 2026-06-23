using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float currentHealth;
    public bool decay;

    //0 is satellite, 1 is rocket, 2 is target
    bool[] isOn = new bool[3];
    SelectControl select;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

            Destroy(gameObject);
        }
    }

    public float NormalizedHealth()
    {
        return (currentHealth / health);
        
    }
}

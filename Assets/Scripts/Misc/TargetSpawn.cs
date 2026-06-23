using UnityEngine;
using System.Collections.Generic;

public class TargetSpawn : MonoBehaviour
{
    [SerializeField] List<TargetAttribute> possibleTargets;
    [SerializeField] RocketControl houston;
    BuyControl buy;

    [SerializeField] GameObject[] unlockableTargets;

    [SerializeField] Camera mainCam;

    [SerializeField] int maxTargets = 4;
    public int currentTargets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        houston = gameObject.GetComponent<RocketControl>();
        buy = GetComponent<BuyControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (houston.meters > 100f && possibleTargets.Count == 0)
        {
            possibleTargets.Add(unlockableTargets[0].GetComponent<TargetAttribute>());
        }

        if (possibleTargets.Count == 1 && buy.thingsBought > 0)
        {
            possibleTargets.Add(unlockableTargets[1].GetComponent<TargetAttribute>());
        }

        
    }

    void FixedUpdate()
    {
        if (currentTargets < maxTargets)
        {
            Invoke("SpawnTargetCheck", 1f);
        }

    }

    float GetSphereOfInfluence()
    {
        float cachedRad = 0f;
        GameObject[] allSats = GameObject.FindGameObjectsWithTag("Satellite");

        foreach (GameObject sat in allSats)
        {
            if (cachedRad < sat.GetComponent<Satellite>().orbitRadius)
            {
                cachedRad = sat.GetComponent<Satellite>().orbitRadius;
            }

        }


        return cachedRad;
    }

    void SpawnTargetCheck()
    {
        for (int i = 0; i < possibleTargets.Count; i++)
        {
            int randomChance = Random.Range(0, possibleTargets[i].spawnChance);
            if (randomChance == 0)
            {

                Vector3 randomRotV = new Vector3(0f, 0f, Random.Range(-180f, 180f));
                Quaternion randomRot = Quaternion.Euler(randomRotV);
                GameObject newTarget = GameObject.Instantiate(unlockableTargets[i], RandomPointInRing(Vector2.one, (GetSphereOfInfluence() + 1f), (mainCam.orthographicSize + 10f)), randomRot);
                currentTargets++;
            }
        }
    }

    //credit to Emolk on the unity forums for saving me mental energy and being lovely
    public Vector2 RandomPointInRing(Vector2 origin, float minRadius, float maxRadius)
    {

        var randomDirection = (Random.insideUnitCircle).normalized;

        var randomDistance = Random.Range(minRadius, maxRadius);
        

        Vector2 point = origin + (randomDirection * randomDistance);
        Debug.Log(point);
        return point;
    }
}

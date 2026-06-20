using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RocketControl : MonoBehaviour
{
    [SerializeField] SelectControl select;
    [SerializeField] GameObject rocketPrefab;

    [SerializeField] List<GameObject> rockets;

    public GameObject planet;
    private Planet plntScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        select = gameObject.GetComponent<SelectControl>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && planet != null)
        {
            plntScript = planet.GetComponent<Planet>();

            SpawnRocket(plntScript.MouseToSurfaceRay(planet.transform), plntScript.MouseToSurfaceNormal(planet.transform));
        }
    }

    public void SpawnRocket(Vector2 position, Vector2 normal)
    {
        

        float normalAngle = (Mathf.Atan2(normal.y, normal.x)) * Mathf.Rad2Deg;
        Quaternion normalQuaternion = Quaternion.Euler(0f, 0f, normalAngle - 90f);
        GameObject rocketInstance = GameObject.Instantiate(rocketPrefab, position, normalQuaternion);

        rockets.Add(rocketInstance);
        SetRocketTarget(rocketInstance);
        rocketInstance.GetComponent<Rocket>().normalVector = normal;


    }

    void SetRocketTarget(GameObject rocket)
    {
        if (select.lockedHooks.Count > 0)
        {
            rocket.GetComponent<Rocket>().targetHook = select.lockedHooks[0];
            select.EraseLockOn(0);

        } else
        {
            rocket.GetComponent<Rocket>().targetHook = select.nearestHook;
        }

        
    }
}

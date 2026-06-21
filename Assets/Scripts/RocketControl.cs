using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;

public class RocketControl : MonoBehaviour
{
    [SerializeField] SelectControl select;
    public GameObject rocketPrefab;

    public List<GameObject> rockets;

    //i dont like having this many public variables but it DOES make sense
    [HideInInspector] public List<GameObject> nonAutoRockets;

    [SerializeField] TextMeshProUGUI metersText;

    public GameObject planet;
    private Planet plntScript;

    public float savedDistance;

    public float meters;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        select = gameObject.GetComponent<SelectControl>();
        savedDistance = 5;

    }

    // Update is called once per frame
    void Update()
    {
        if (rocketPrefab != null)
        {
            if (Input.GetButtonDown("Fire1") && planet != null && meters >= rocketPrefab.GetComponent<Rocket>().rocketPrice)
            {
                plntScript = planet.GetComponent<Planet>();

                SpawnRocket(plntScript.MouseToSurfaceRay(planet.transform), plntScript.MouseToSurfaceNormal(planet.transform));

            }
        }


        ProcessTotalDistance();
    }

    public void SpawnRocket(Vector2 position, Vector2 normal)
    {
        
        if (select.lockedHooks.Count == 0 && (select.nearestHook.GetComponent<Satellite>().loadedRockets.Count >= select.nearestHook.GetComponent<Satellite>().maxRockets))
        {
            return;
        }

        float normalAngle = (Mathf.Atan2(normal.y, normal.x)) * Mathf.Rad2Deg;
        Quaternion normalQuaternion = Quaternion.Euler(0f, 0f, normalAngle - 90f);
        GameObject rocketInstance = GameObject.Instantiate(rocketPrefab, position, normalQuaternion);

        rockets.Add(rocketInstance);
        SetRocketTarget(rocketInstance);
        rocketInstance.GetComponent<Rocket>().normalVector = normal;
        rocketInstance.GetComponent<Rocket>().houston = this;
        savedDistance -= (rocketInstance.GetComponent<Rocket>().rocketPrice);



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

    void ProcessTotalDistance()
    {
        float totalDistance = 0f;
        foreach (GameObject rock in rockets)
        {
            totalDistance += (int)rock.GetComponent<Rocket>().distanceTravelled;
        }
        meters = totalDistance + savedDistance;
        string txt = (meters).ToString() + " km";
        metersText.text = txt;
    }

}

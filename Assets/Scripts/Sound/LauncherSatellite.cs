using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LauncherSatellite : Satellite
{
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] Satellite targetHook;
    private Rocket rockProp;

    private float hookRad;
    private float hookAngSpeed;
    private float thisAngSpeed;
    private float trueDistance;
    RocketControl houston;
    bool launching = false;
    [SerializeField] float cooldown = 4f;

    [SerializeField] AudioClip[] sfx;
    float rocketPrice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        SelectNearestSatellite();
        base.Start();
        

        hookRad = targetHook.orbitRadius;
        hookAngSpeed = targetHook.orbitVelocity / hookRad;
        thisAngSpeed = orbitVelocity / orbitRadius;
        trueDistance = Mathf.Abs(hookRad - orbitRadius);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        rockProp = rocketPrefab.GetComponent<Rocket>();
        rocketPrice = rockProp.rocketPrice;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (houston != null)
        {
            if (!launching && (houston.meters - rocketPrice) > 0)
            {
                StartCoroutine(LaunchRocket());
            }
        }

        if (targetHook == null)
        {
            SelectNearestSatellite();
        }

    }

    void SelectNearestSatellite()
    {
        GameObject[] allSatObjs = GameObject.FindGameObjectsWithTag("Satellite");
        List<Satellite> allSats = new List<Satellite> { };

        foreach (GameObject obj in allSatObjs)
        {
            allSats.Add(obj.GetComponent<Satellite>());
        }



        float cachedDist = 999;
        foreach (Satellite sat in allSats)
        {
            if (sat.rocketTarget == true)
            {
                float dist = (sat.transform.position - transform.position).magnitude;
                Debug.Log(dist);
                if (cachedDist > dist)
                {
                    cachedDist = dist;
                    targetHook = sat;
                }
            }
        }

    }

    Vector3 CalcTargetPos()
    {
        //the time the rocket will travel to its end point will be equal to the cooldown, or the launcher's fire rate.

        //calculate the angle of the targethook's turn during the cooldown interval
        float angleTurn = cooldown * (hookAngSpeed);

        Vector3 hookPos = targetHook.transform.position;
        float trueAngle = ((Mathf.Atan2(hookPos.y, hookPos.x) + angleTurn));

        Debug.Log((angleTurn) * Mathf.Rad2Deg  + " Degrees");
        Vector2 launchDir = new Vector2(Mathf.Cos(trueAngle), Mathf.Sin(trueAngle));
        Debug.Log(launchDir);


        return (launchDir * hookRad);

    }

    IEnumerator LaunchRocket()
    {
        launching = true;

        
        Vector2 dir = ((CalcTargetPos()) - transform.position).normalized;
        SpawnRocket(transform.position, dir);
        yield return new WaitForSeconds(cooldown);
        launching = false;
    }

    //3 upgrades for Basic Skyhook: Spin Faster, Move Faster, More Health (which wont do anything)
    public override void UpgradeHook(int index)
    {
        switch (index)
        {
            default:
                Debug.Log("Upgrade index outside of registered options");
                break;

            case 0:

                if (upgrades[index].currentUpgrades < upgrades[index].maxUpgrades)
                {
                    cooldown -= (0.4f * (upgrades[index].currentUpgrades + 1));
                }
                break;

            case 1:
                if (upgrades[index].currentUpgrades < upgrades[index].maxUpgrades)
                {
                    gameObject.GetComponent<Health>().health += (90f * (upgrades[index].currentUpgrades + 1));
                    gameObject.GetComponent<Health>().currentHealth = gameObject.GetComponent<Health>().health;
                }
                break;


        }

        upgrades[index].currentUpgrades++;
    }

    //i wish i could call from houston but weird things happen if i try that :(
    public void SpawnRocket(Vector2 position, Vector2 dir)
    {

        if ((targetHook.loadedRockets.Count >= targetHook.maxRockets))
        {
            return;

        }



        float normalAngle = (Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;
        Quaternion normalQuaternion = Quaternion.Euler(0f, 0f, normalAngle);
        GameObject rocketInstance = GameObject.Instantiate(rocketPrefab, position, normalQuaternion);

        houston.rockets.Add(rocketInstance);
        Rocket rocketScript = rocketInstance.GetComponent<Rocket>();
        rocketScript.targetHook = targetHook.gameObject;
        rocketScript.normalVector = dir;
        rocketScript.houston = houston;
        rocketScript.waitOnInit = false;
        houston.savedDistance -= (rocketScript.rocketPrice);

        //magnitude of distance between the rocket and the target
        Vector2 targetPos = CalcTargetPos();

        rocketScript.trueDistance = (position - (targetPos)).magnitude;
        rocketScript.rocketSpeed = (((position - (targetPos)).magnitude) / cooldown);
        rocketScript.targetPosition = targetPos;

        //assign target directly







    }
}

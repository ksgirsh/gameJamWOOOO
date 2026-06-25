using UnityEngine;
using System.Collections;

public class AsteroidTarget : TargetAttribute
{
    Camera mc;
    float shrinkF = 0.8f;
    Rigidbody2D rb;
    [SerializeField] float asteroidMoveSpeed;
    public bool beingMined = false;
    float maxRange;
    Health health;
    RocketControl houston;
    SelectControl control;
    int metalAmt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        mc = camObj.GetComponent<Camera>();
        base.Start();

        transform.position = CalcSpawnPos();

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(-transform.right * (asteroidMoveSpeed + Random.Range(-1f, 1f)), ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-2f, 2f), ForceMode2D.Impulse);
        

        maxRange = camObj.GetComponent<CameraM>().maxSize;
        health = gameObject.GetComponent<Health>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        control = player.GetComponent<SelectControl>();

        metalAmt = 100 + (Random.Range(-50, 200));

    }

    // Update is called once per frame
    void Update()
    {
        if ((Mathf.Abs(transform.position.x - (transform.position.x * 0.42f)) > maxRange) && (beingMined == false))
        {
            TriggerDestroy();

        } else if ((Mathf.Abs(transform.position.y) > (maxRange)) && (beingMined == false))
        {
            //Trigger Destroy
            TriggerDestroy();
        }

        if (beingMined && health != null)
        {
            //triple decay rate
            health.currentHealth -= (Time.deltaTime * 2f);
        }

    }

    bool IsWithinCamera()
    {
        Vector2 camDim = new Vector2(mc.pixelWidth, mc.pixelHeight);


        float aspect = (camDim.x / camDim.y);
        float wHeight = mc.orthographicSize * 2;
        float wWidth = wHeight * aspect;

        Vector2 widthRange = new Vector2((mc.transform.position.x - wWidth), (mc.transform.position.x + wWidth));
        Vector2 heightRange = new Vector2((mc.transform.position.y - wHeight), (mc.transform.position.y + wHeight));

        if (transform.position.x >= widthRange.y && transform.position.x <= widthRange.x)
        {
            if (transform.position.y >= heightRange.y && transform.position.y <= heightRange.x)
            {
                return true;

            } else
            {
                return false;
            }


        } else
        {
            return false;
        }

    }

    Vector2 CalcSpawnPos()
    {
        Vector2 camDim = new Vector2(mc.pixelWidth, mc.pixelHeight);


        float aspect = (camDim.x / camDim.y);
        float wHeight = mc.orthographicSize * 2;
        float wWidth = wHeight * aspect;

        //Debug.Log("WIDTH: " + wWidth + "  HEIGHT: " + wHeight);

        Vector2 wDim = new Vector2(wWidth, wHeight);

        Vector2 point = mc.transform.position + (Vector3)(wDim / 2);
        // Debug.Log(point);

        Vector2 spawnPos = new Vector2((point.x + Random.Range(2, 8)), Random.Range(point.y * shrinkF, (point.y - wHeight) * shrinkF));

        float maxRad = SphereOfInfluence();

        if (spawnPos.y < (maxRad + 2f) && spawnPos.y > -(maxRad + 2f))
        {
            int coinFlip = Random.Range(0, 2);
            if (coinFlip == 0)
            {
                spawnPos.y = (maxRad + 2f);
            } else
            {
                spawnPos.y = -(maxRad + 2f);
            }
        }

        return spawnPos;
        //x will be point.x + some margin
        //y will be random between point.y * shrinkF and (point.y - wHeight) * shrinkF
    }

    float SphereOfInfluence()
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


    public void TakeDamage(float damage)
    {
        if (((health.currentHealth - damage) <= 0) && houston != null)
        {
            houston.savedMetal += metalAmt;
        } else
        {
            health.currentHealth -= damage;
            houston.savedMetal += (int)Mathf.Round((metalAmt / (Random.Range(10f, 90f))));
        }
    }

    void OnMouseEnter()
    {
        control.SelectTrigger(gameObject);
    }

    void OnMouseExit()
    {
        control.EraseTrigger();
    }

}

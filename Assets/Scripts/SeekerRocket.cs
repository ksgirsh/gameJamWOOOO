using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class SeekerRocket : Rocket
{
    public float seekCheckRadius;
    [SerializeField] protected LayerMask seekLayers;
    [SerializeField] float turnStrength;
    [SerializeField] bool seeking = false;
    protected bool hitTarget = false;
    protected Transform seekTarget;
    Vector2 cachedVel = Vector2.zero;
    float timeSeeking;
    bool seekInfinite = false;

    [Header("Offense")]
    [SerializeField] float rocketDamage;

    void SeekCheck()
    {
        Collider2D[] seeks = Physics2D.OverlapCircleAll(transform.position, seekCheckRadius, seekLayers);

        if (seeks.Length > 0)
        {
            seeking = true;
            seekTarget = seeks[0].gameObject.transform;

            //rb.linearVelocity = Vector2.zero;
        }

    }

    protected override void Update()
    {
        base.Update();
        if (seeking == false && hitTarget == false)
        {
            SeekCheck();
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if ((seeking == true) && !hitTarget && tracking)
        {
            Seek(seekTarget);
            Debug.Log(seekTarget);
            timeSeeking += Time.fixedDeltaTime;

        }
    }

    void Seek(Transform target)
    {
        if (target != null)
        {
            Vector2 dist = target.position - transform.position;
            if (cachedVel == Vector2.zero)
            {
                cachedVel = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
            }

            rb.AddForce(dist.normalized * ((Time.fixedDeltaTime) * (cachedVel.magnitude) + (Mathf.Pow(2f, timeSeeking))), ForceMode2D.Impulse);



            Quaternion setRotate = Quaternion.Euler(transform.rotation.x, transform.rotation.y, ((Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x)) * Mathf.Rad2Deg));
            transform.rotation = setRotate;


            if (dist.magnitude < 0.8f)
            {
                hitTarget = true;
                rb.linearVelocity *= (1 / (rb.linearVelocity.magnitude * 0.3f));
            }

        } else
        {
            hitTarget = true;
            rb.linearVelocity *= (1 / (rb.linearVelocity.magnitude * 0.3f));
            if (seekInfinite)
            {
                seeking = false;
                hitTarget = false;
            }
        }

        

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, seekCheckRadius);
    }

    public override void UpgradeRocket(int index)
    {
        switch (index)
        {
            case (0):
                seekCheckRadius += (1f * upgrades[0].currentUpgrades);
                break;
            case (1):
                seekInfinite = true;
                break;
            case (2):
                rocketDamage += 1000f;
                break;
            default:
                Debug.Log("Supplied index is outside of those given by the upgrades list.");
                break;

        }
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Target")
        {
            TargetAttribute check = coll.gameObject.GetComponent<TargetAttribute>();

            if (check.identifier == "Meter Target")
            {
                houston.savedDistance += Mathf.Round(coll.gameObject.GetComponent<MeterTarget>().targetPoints);
                coll.gameObject.GetComponent<MeterTarget>().TargetHit();

            }

            if (check.identifier == "Asteroid")
            {
                coll.gameObject.GetComponent<AsteroidTarget>().TakeDamage(rocketDamage);
                StartCoroutine(EraseRocket(0.05f));

            }
        }

        if (coll.gameObject.tag == "Alien")
        {
            Health alieHealth = coll.gameObject.GetComponent<Health>();
            StartCoroutine(alieHealth.TakeDamage(rocketDamage));
        }

    }

}

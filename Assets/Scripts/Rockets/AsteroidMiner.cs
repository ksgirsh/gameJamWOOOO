using UnityEngine;
using System.Collections;

public class AsteroidMiner : SeekerRocket
{

    protected bool mining;
    public float mineTime = 1.3f;
    int mineAmt = 1;
    bool landed = false;

    bool lpIsPlaying;

    protected override void Update()
    {
        base.Update();
        if (hitTarget && tracking == true)
        {
            LandOnAsteroid(seekTarget);
        } else if (landed == true)
        {
            if (!mining)
            {
                StartCoroutine(Mine(mineTime, mineAmt));
            }

            StartCoroutine(LoopSFX(3));
        }

    }

    void LandOnAsteroid(Transform asteroid)
    {
        Vector2 distVector = asteroid.position - transform.position;
        // Debug.Log(-(distVector.normalized));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (distVector.normalized), Mathf.Infinity, seekLayers);

        if (hit != null)
        {         
            float normalAngle = (Mathf.Atan2(hit.normal.y, hit.normal.x)) * Mathf.Rad2Deg;
            Quaternion normalQuaternion = Quaternion.Euler(0f, 0f, normalAngle);
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            Vector2 newDist = ((Vector2)asteroid.position - hit.point);
            transform.SetParent(asteroid);
            transform.localPosition = newDist;
            //heal to full
            heal.currentHealth = heal.health;
            tracking = false;
            landed = true;
            tr.emitting = false;
            asteroid.gameObject.GetComponent<AsteroidTarget>().beingMined = true;




        }

    }

    protected virtual IEnumerator Mine(float initDuration, int mineAmt)
    {
        mining = true;
        yield return new WaitForSeconds(initDuration);
        houston.savedMetal += mineAmt;
        SoundFXManager.instance.PlaySoundEffectClip(sfx[4], transform.position, 1f);
        mining = false;
    }

    protected override IEnumerator EraseRocket(float initDelay)
    {
        yield return new WaitForSeconds(initDelay);

        houston.rockets.Remove(gameObject);
        houston.savedDistance += ((int)distanceTravelled);
        if (seekTarget != null)
        {
            seekTarget.gameObject.GetComponent<AsteroidTarget>().beingMined = false;
        }

        Destroy(gameObject);
    }

    public override void UpgradeRocket(int index)
    {
        switch (index)
        {
            case (0):
                heal.health += (4f * upgrades[0].currentUpgrades);
                heal.GetComponent<Health>().currentHealth = heal.health;
                break;
            case (1):
                mineTime -= (0.3f * upgrades[0].currentUpgrades);
                break;
            case (2):
                mineAmt += 1;
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
        }

    }

    void LoopAudio()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.loop = true;
    }

    protected virtual IEnumerator LoopSFX(int i)
    {
        if (!lpIsPlaying)
        {
            lpIsPlaying = true;
            SoundFXManager.instance.PlaySoundEffectClip(sfx[i], transform.position, 1f);
            yield return new WaitForSeconds(sfx[i].length);
            lpIsPlaying = false;
        }

    }

}

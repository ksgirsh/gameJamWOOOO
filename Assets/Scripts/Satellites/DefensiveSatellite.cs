using UnityEngine;
using System.Collections;
public class DefensiveSatellite : Satellite
{
    [field:SerializeField] protected GameObject projectile;
    [field: SerializeField] protected float fireForce = 8f;

    [field:SerializeField] public float fireRange { get; private set; }
    [field: SerializeField] protected LayerMask alienLayers;

    [field: SerializeField] protected float attackDamage = 25f;
    [field: SerializeField] protected float attackRate = 1f;
    protected bool firing = false;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SeekAliens();
    }

    protected virtual void SeekAliens()
    {
        Collider2D[] seek = Physics2D.OverlapCircleAll(hookPoint.position, fireRange, alienLayers);
        if (seek.Length > 0)
        {
            GameObject targetAlien = seek[0].gameObject;

            if (firing == false)
            {
                StartCoroutine(FireAtAlien(targetAlien.transform));
            }



        }


    }

    protected virtual IEnumerator FireAtAlien(Transform trans)
    {
        firing = true;
        yield return new WaitForSeconds(attackRate);
        if (trans != null)
        {
            rb.angularVelocity = 0f;

            Vector2 dir = (trans.position - transform.position).normalized;

            float angle = (Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;
            Quaternion quatRotate = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = quatRotate;

            GameObject proh = GameObject.Instantiate(projectile, transform.position, quatRotate);
            proh.GetComponent<Rigidbody2D>().AddForce(dir * fireForce, ForceMode2D.Impulse);
            proh.GetComponent<Bullet>().attackDamage = attackDamage;
            Destroy(proh, 2f);

        }

        firing = false;
        rb.AddTorque(rotateVelocity, ForceMode2D.Impulse);

    }

    //increase fire rate, increase range, repair
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
                    attackRate -= (0.2f * (upgrades[index].currentUpgrades + 1));
                }
                break;

            case 1:

                if (upgrades[index].currentUpgrades < upgrades[index].maxUpgrades)
                {
                    fireRange += (1f * (upgrades[index].currentUpgrades + 1));
                }
                break;

            case 2:
                if (upgrades[index].currentUpgrades < upgrades[index].maxUpgrades)
                {
                    gameObject.GetComponent<Health>().health += (90f * (upgrades[index].currentUpgrades + 1));
                    gameObject.GetComponent<Health>().currentHealth = gameObject.GetComponent<Health>().health;
                }
                break;


        }

        upgrades[index].currentUpgrades++;
    }


}

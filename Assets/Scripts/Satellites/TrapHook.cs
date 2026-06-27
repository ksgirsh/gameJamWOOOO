using UnityEngine;
using System.Collections;
public class TrapHook : DefensiveSatellite
{
    [SerializeField] float trapHealth;

    protected override void SeekAliens()
    {
        if (firing == false)
        {
            StartCoroutine(FireTrap());
        }

    }

    protected virtual IEnumerator FireTrap()
    {
        firing = true;

        Vector2 dir = (Random.insideUnitCircle).normalized;

        GameObject proh = GameObject.Instantiate(projectile, transform.position, Quaternion.identity);
        Debug.Log(proh.name);

        proh.GetComponent<Rigidbody2D>().AddForce(dir * fireForce, ForceMode2D.Impulse);
        proh.GetComponent<Rigidbody2D>().AddTorque(2f, ForceMode2D.Impulse);
        proh.GetComponent<Bullet>().attackDamage = attackDamage;
        proh.GetComponent<Health>().health = trapHealth;
        //Destroy(proh, 40f);

        yield return new WaitForSeconds(attackRate);
        firing = false;


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
                    trapHealth += (40f * (upgrades[index].currentUpgrades + 1));
                }
                break;

            case 2:
                if (upgrades[index].currentUpgrades < upgrades[index].maxUpgrades)
                {
                    attackDamage += (5f * (upgrades[index].currentUpgrades + 1));
                }
                break;


        }

        upgrades[index].currentUpgrades++;
    }


}

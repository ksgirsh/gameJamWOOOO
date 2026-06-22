using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class AimHook : Satellite
{
    public bool autoUpgrade { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        autoUpgrade = false;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void UpgradeHook(int index)
    {
        switch (index)
        {
            default:
                Debug.Log("Upgrade index outside of registered options");
                break;
            case 0:
                autoUpgrade = true;

                break;
            case 1:

                gameObject.GetComponent<Health>().health += (90f * (upgrades[index].currentUpgrades + 1));
                gameObject.GetComponent<Health>().currentHealth = gameObject.GetComponent<Health>().health;

                break;
            

        }
        upgrades[index].currentUpgrades++;
    }
}

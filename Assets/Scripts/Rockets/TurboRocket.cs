using UnityEngine;

public class TurboRocket : Rocket
{
    public override void UpgradeRocket(int index)
    {
        switch (index)
        {
            case (0):
                heal.health += (4f * upgrades[0].currentUpgrades);
                heal.GetComponent<Health>().currentHealth = heal.health;
                break;
            case (1):
                if (upgrades[1].currentUpgrades > 0)
                {
                    rocketSpeed += 2f;
                }
                break;
            default:
                Debug.Log("Supplied index is outside of those given by the upgrades list.");
                break;

        }
    }
}

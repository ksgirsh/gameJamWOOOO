using UnityEngine;

public class MedHook : Satellite
{
    [SerializeField] Transform medAura;
    [SerializeField] float medRange;
    [SerializeField] float healRate;
    [SerializeField] LayerMask healLayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        medAura.transform.localScale *= medRange;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CheckForHealable();
    }

    protected void Heal(Health heal)
    {
        heal.currentHealth += (healRate * Time.deltaTime);
    }

    protected void CheckForHealable()
    {
        Collider2D[] healables = Physics2D.OverlapCircleAll(transform.position, medRange, healLayers);
        if (healables.Length > 0)
        {
            for (int i = 0; i < healables.Length; i++)
            {
                Heal(healables[i].gameObject.GetComponent<Health>());
            }

        }
    }

    //3 upgrades for Basic Skyhook: Spin Faster, Move Faster, More Health (which wont do anything)
    public virtual void UpgradeHook(int index)
    {
        switch (index)
        {
            default:
                Debug.Log("Upgrade index outside of registered options");
                break;
            case 0:

                if (upgrades[index].currentUpgrades < upgrades[index].maxUpgrades)
                {
                    healRate *= 1.25f;
                }
                break;

            case 1:

                if (upgrades[index].currentUpgrades < upgrades[index].maxUpgrades)
                {
                    medRange += 1f;
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

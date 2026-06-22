using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HookUpgradeHandler : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    //public List<Satellite> satellitesToUpgrade;
    public Satellite satelliteToUpgrade;
    [SerializeField] RocketControl houston;

    [SerializeField] Image healthMat;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();

        dropdown = gameObject.GetComponent<TMP_Dropdown>();

        dropdown.onValueChanged.AddListener(value => {
            Debug.Log("Pressed");
            dropdown.SetValueWithoutNotify(-1);
        });

        dropdown.SetValueWithoutNotify(-1);
        
    }

    // Update is called once per frame
    void Update()
    {
        healthMat.material.SetFloat("_Health", (satelliteToUpgrade.gameObject.GetComponent<Health>().NormalizedHealth()));

    }

    //This function is called in Select Control (FYI). Me commenting down where this function is called is probably a sign of bad code. But uhh. i only have 5 days okay cut me a break
    public void ChangeUpgrades(List<TMP_Dropdown.OptionData> options, List<int> prices, List<int> remUpgr)
    {
        
        dropdown.options.Clear();

        //instancing the options prevents modifying the root satellite "options" variable, which is nice :)
        
        //NEVERMIND that was like an hour of my life i'll never get back. fuck

        for (int i = 0; i < options.Count; i++)
        {

            //future change: remaining upgrade should be equal to the MINIMUM amount of upgrades among the selected list
            string addendum = string.Concat(" - ", prices[i].ToString(), "km (x", remUpgr[i].ToString(), ") ");
            string predendum = options[i].text;

            options[i].text = string.Concat(predendum, addendum);
        }


        dropdown.AddOptions(options);

        dropdown.RefreshShownValue();

        
        dropdown.SetValueWithoutNotify(-1);

    }

    public void GetDropdownValue()
    {
        int pickedEntryIndex = dropdown.value;

        if ((houston.meters - satelliteToUpgrade.upgrades[pickedEntryIndex].cost) >= 0)
        {
            satelliteToUpgrade.UpgradeHook(pickedEntryIndex);
            houston.savedDistance -= satelliteToUpgrade.upgrades[pickedEntryIndex].cost;
            satelliteToUpgrade.ResetOptionsText(1, pickedEntryIndex);
            ChangeUpgrades(satelliteToUpgrade.upgradeDropdownDisplay, satelliteToUpgrade.GetListOfPrices(), satelliteToUpgrade.GetListOfRemainingUpgrades());

        } else
        {
            Debug.Log("Not Enough Money, Come back when you're a little-- mmmmmm RICHER!");
        }


        dropdown.SetValueWithoutNotify(-1);

    }


}

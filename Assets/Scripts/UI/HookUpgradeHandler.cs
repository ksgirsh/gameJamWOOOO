using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class HookUpgradeHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Dropdown dropdown;
    //public List<Satellite> satellitesToUpgrade;
    public Satellite satelliteToUpgrade;
    [SerializeField] RocketControl houston;
    [SerializeField] SelectControl select;

    [SerializeField] TextMeshProUGUI primaryText;


    public List<Satellite> satellitesToUpgrade;
    bool canUpgrade = true;


    //0 is select, 1 is purchase
    [SerializeField] AudioClip[] sfx;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        select = player.GetComponent<SelectControl>();

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
       if (satellitesToUpgrade.Count > 1)
       {
          string write = string.Concat("Upgrade Hooks", " x", satellitesToUpgrade.Count);
          primaryText.text = write;
       } else
       {
          primaryText.text = "Upgrade Hook";
       }

        if (dropdown.IsExpanded)
        {
            UpdateText();
        }
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

            //-- i represents the upgrade. get all of the remaining upgrades for THIS upgrade
            List<int> remUpgradesThisUpg = new List<int>{ };
            foreach (Satellite sat in satellitesToUpgrade)
            {
                if (i >= sat.upgrades.Count)
                {
                    break;

                }

                int thisRemainingUpgrade = (sat.upgrades[i].maxUpgrades - sat.upgrades[i].currentUpgrades);
                remUpgradesThisUpg.Add(thisRemainingUpgrade);
            }

            int minimumRemainingUpgrade = remUpgradesThisUpg[0];
            for (int k = 0; k < remUpgradesThisUpg.Count; k++)
            {
                if (minimumRemainingUpgrade > remUpgradesThisUpg[k])
                {
                    minimumRemainingUpgrade = remUpgradesThisUpg[k];
                }
            }


            string addendum = string.Concat(" - ", (prices[i]).ToString());
            //if there is already an addendum, remove it
            string predendum = ((options[i].text).Replace(addendum, ""));

            options[i].text = string.Concat(predendum, addendum);

        }


        dropdown.AddOptions(options);

        dropdown.RefreshShownValue();

        
        dropdown.SetValueWithoutNotify(-1);

    }

    public void GetDropdownValue()
    {
       
            int pickedEntryIndex = dropdown.value;
            //cost of upgrading multiple satellites
            int totalCost = ((satellitesToUpgrade[0].upgrades[pickedEntryIndex].cost) * satellitesToUpgrade.Count);
            
            if ((houston.meters - totalCost) >= 0)
            {
                foreach (Satellite sat in satellitesToUpgrade)
                {
                    if (sat.upgrades[pickedEntryIndex].currentUpgrades < sat.upgrades[pickedEntryIndex].maxUpgrades)
                    {
                        sat.UpgradeHook(pickedEntryIndex);

                        AudioClip purchase = sfx[1];
                        SoundFXManager.instance.PlaySoundEffectClip(purchase, Vector2.zero, 1f);
                } else
                    {
                        //refund purchase
                        houston.savedDistance += sat.upgrades[pickedEntryIndex].cost;
                    }      
                }
                houston.savedDistance -= totalCost;
                ChangeUpgrades(satelliteToUpgrade.upgradeDropdownDisplay, satelliteToUpgrade.GetListOfPrices(), satelliteToUpgrade.GetListOfRemainingUpgrades());

            }
            else
            {
                Debug.Log("Not Enough Money, Come back when you're a little-- mmmmmm RICHER!");
            }


        dropdown.SetValueWithoutNotify(-1);


    }

    public void LockedHooksToList()
    {
        satellitesToUpgrade.Clear();

        foreach (GameObject hook in select.lockedHooks)
        {
            satellitesToUpgrade.Add(hook.GetComponent<Satellite>());
        }

        string cachedIdent = "";

        for (int i = 0; i < satellitesToUpgrade.Count; i++)
        {
            Satellite satID = satellitesToUpgrade[i];

            if (i == 0)
            {
                cachedIdent = satID.identifier;
            }

            if (cachedIdent == satID.identifier)
            {
                continue;

            }
            else
            {

                //Debug.Log("Mismatch in selected hooks");
                satellitesToUpgrade.RemoveAt(i);
                dropdown.interactable = false;
                //canUpgrade = false;
                return;
            }
        }

       // canUpgrade = true;
        dropdown.interactable = true;
        //Debug.Log("All hooks of the same type");
    }


    void UpdateText()
    {
        List<GameObject> remText = new List<GameObject> { };
        ComponentSort[] allRemText = dropdown.gameObject.GetComponentsInChildren<ComponentSort>();
        foreach (ComponentSort sort in allRemText)
        {
            remText.Add(sort.gameObject);
        }

        for (int i = 0; i < remText.Count; i++)
        {
            //-- i represents the upgrade. get all of the remaining upgrades for THIS upgrade
            List<int> remUpgradesThisUpg = new List<int> { };
            foreach (Satellite sat in satellitesToUpgrade)
            {
                if (i >= sat.upgrades.Count)
                {
                    break;

                }

                int thisRemainingUpgrade = (sat.upgrades[i].maxUpgrades - sat.upgrades[i].currentUpgrades);
                remUpgradesThisUpg.Add(thisRemainingUpgrade);
            }

            int minimumRemainingUpgrade = remUpgradesThisUpg[0];
            for (int k = 0; k < remUpgradesThisUpg.Count; k++)
            {
                if (minimumRemainingUpgrade > remUpgradesThisUpg[k])
                {
                    minimumRemainingUpgrade = remUpgradesThisUpg[k];
                }
            }

            remText[i].GetComponent<TextMeshProUGUI>().text = string.Concat(" x" + minimumRemainingUpgrade.ToString());


        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //left click
        if (eventData.pointerId == -1)
        {
            AudioClip select = sfx[0];
            SoundFXManager.instance.PlaySoundEffectClip(select, Vector2.zero, 1f);
        }

    }
}

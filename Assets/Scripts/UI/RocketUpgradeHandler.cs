using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class RocketUpgradeHandler : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] RocketControl houston;
    [SerializeField] SelectControl select;
    [SerializeField] RocketShop shop;

    public RocketMenuPanel subjectPanel;

     
    public Rocket subject;

   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        select = player.GetComponent<SelectControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dropdown.IsExpanded)
        {
            UpdateText();
        }
    }

    //This function is called in Select Control (FYI). Me commenting down where this function is called is probably a sign of bad code. But uhh. i only have 5 days okay cut me a break
    public void ChangeUpgrades(List<TMP_Dropdown.OptionData> options)
    {
        dropdown.options.Clear();

        for (int i = 0; i < options.Count; i++)
        {
            int remUpgr = (subjectPanel.upgrades[i].maxUpgrades - subjectPanel.upgrades[i].currentUpgrades);
            string addendum = string.Concat(" - ", (subject.upgrades[i].cost).ToString());
            //if there is already an addendum, remove it
            string predendum = ((options[i].text).Replace(addendum, ""));

            options[i].text = string.Concat(predendum, addendum);


            


        }


        dropdown.AddOptions(options);

        
        dropdown.RefreshShownValue();


        dropdown.SetValueWithoutNotify(-1);
        UpdateText();

    }

    public void GetDropdownValue()
    {
        int pickedEntryIndex = dropdown.value;
        if ((houston.meters - subject.upgrades[pickedEntryIndex].cost) >= 0)
        {
            //upgrades rocket PANEL. the panel then applies changes to the rocket when said rocket is instantiated.
            subjectPanel.UpgradePanel(pickedEntryIndex);
            houston.savedDistance -= subject.upgrades[pickedEntryIndex].cost;
            subjectPanel.ResetTextUponBuy();
        }

        dropdown.SetValueWithoutNotify(-1);


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
            int remainingUpgr = subjectPanel.upgrades[i].maxUpgrades - subjectPanel.upgrades[i].currentUpgrades;
            remText[i].GetComponent<TextMeshProUGUI>().text = string.Concat(" x" + remainingUpgr.ToString());


        }

    }
}

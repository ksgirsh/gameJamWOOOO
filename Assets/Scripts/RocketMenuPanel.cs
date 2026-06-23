using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RocketMenuPanel : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public Image icon;
    [HideInInspector] public TextMeshProUGUI title;
    [HideInInspector] public TextMeshProUGUI price;
    [HideInInspector] public GameObject item;

    [System.Serializable]
    public class Upgrade
    {
        public int cost;
        public int maxUpgrades;
        public int currentUpgrades;
        public int magnitude;
    }
    public List<Upgrade> upgrades;



    private RocketControl houston;
    public bool isActive = false;
    public bool unlocked = false;
    [HideInInspector] public int unlockPrice;
    [HideInInspector] public int consumePrice;
    [SerializeField] Image bgColor;
    [HideInInspector] public RocketShop shop;

    [SerializeField] Color activeColor;
    [SerializeField] Color lockedColor;
    [SerializeField] Color inactiveColor;
    [SerializeField] Color selectedColor;

    public bool selectedForUpgrade = false;
    GameObject player;

    private RocketUpgradeHandler rockUpgr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        bgColor = gameObject.GetComponent<Image>();
        rockUpgr = UISingleton.instance.rockUpgradeDrop.GetComponent<RocketUpgradeHandler>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitUnlockLogic()
    {
        if (unlocked == false)
        {
            price.text = unlockPrice.ToString();
            bgColor.color = lockedColor;



        }
        else
        {
            price.text = consumePrice.ToString();
            bgColor.color = inactiveColor;
            Start();
            ToggleActiveRocket();

        }
    }

    public void ToggleActiveRocket()
    {

        if (unlocked == false && ((houston.savedDistance - unlockPrice) >= 0))
        {
            houston.savedDistance -= unlockPrice;
            price.text = consumePrice.ToString();
            unlocked = true;
            bgColor.color = inactiveColor;

        }
        else if (unlocked == true)
        {
            isActive = !isActive;
            if (isActive)
            {
                if (shop.activePanel != null && shop.activePanel != this)
                {
                    //already an active rocket being used
                    shop.activePanel.DeactivatePanel();
                    shop.activePanel = this;
                }
                else
                {
                    shop.activePanel = this;
                }


                item.GetComponent<Rocket>().rocketPrice = consumePrice;
                houston.rocketPrefab = item;
                bgColor.color = activeColor;

            }
            else
            {
                DeactivatePanel();
            }

        }





    }

    public void DeactivatePanel()
    {
        houston.rocketPrefab = null;
        bgColor.color = inactiveColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //left click
        if (eventData.pointerId == -1)
        {
            ToggleActiveRocket();
        }

        //right click
        if (eventData.pointerId == -2 && unlocked == true)
        {

            selectedForUpgrade = !selectedForUpgrade;
            if (selectedForUpgrade)
            {
                Rocket rockProperties = item.GetComponent<Rocket>();

                if (shop.selectedPanel != null)
                {
                    shop.selectedPanel.Deselect();
                    shop.selectedPanel = this;
                    rockUpgr.subject = rockProperties;
                    rockUpgr.subjectPanel = this;
                }
                else
                {
                    shop.selectedPanel = this;
                    rockUpgr.subjectPanel = this;
                    rockUpgr.subject = rockProperties;
                }

                rockUpgr.ChangeUpgrades(rockProperties.upgradeDropdownDisplay);
                UISingleton.instance.ToggleDropdown(3);
                bgColor.color = selectedColor;


            }
            else
            {
                Deselect();

            }
        }


    }

    public void Deselect()
    {
        selectedForUpgrade = false;
        shop.selectedPanel = null;
        if (isActive)
        {
            bgColor.color = activeColor;
        }
        else
        {
            bgColor.color = inactiveColor;
        }

        //check if any skyhooks are selected, if so show skyhook upgrade dropdown, otherwise enable shop
        if (player.GetComponent<SelectControl>().lockedHooks.Count > 1)
        {
            UISingleton.instance.ToggleDropdown(2);

        }
        else
        {
            UISingleton.instance.ToggleDropdown(1);
        }
    }

    public void AddBlankUpgrades(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            upgrades.Add(new Upgrade { });
        }
    }

    public void UpgradePanel(int index)
    {
        //store upgrade data on panel, apply to rocket on instantiate
        upgrades[index].currentUpgrades++;
    }


    public void ResetTextUponBuy()
    {
        Rocket rockProperties = item.GetComponent<Rocket>();
        for (int i = 0; i < rockProperties.upgradeDropdownDisplay.Count; i++)
        {
            int remUpgr = (upgrades[i].maxUpgrades - upgrades[i].currentUpgrades);
            string addendum = string.Concat(" - ", (rockProperties.upgrades[i].cost).ToString(), "km (x", (remUpgr + 1).ToString(), ") ");
            //if there is already an addendum, remove it
            string predendum = ((rockProperties.upgradeDropdownDisplay[i].text).Replace(addendum, ""));

            rockProperties.upgradeDropdownDisplay[i].text = predendum;
        }

    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RocketShop : MonoBehaviour
{
    [System.Serializable]
    class RocketBuyable
    {
        public Sprite icon;
        public string title;
        public int price;
        public GameObject item;
        public bool unlocked = false;
        public int unlockPrice;
    }

    [SerializeField] List<RocketBuyable> rocketPurchasables;
    [SerializeField] GameObject rocketUIPanel;
    [SerializeField] Transform location;
    [SerializeField] float horizontalSpacing;

    public RocketMenuPanel activePanel;
    public RocketMenuPanel selectedPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateRocketShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateRocketShop()
    {
        for (int i = 0; i < rocketPurchasables.Count; i++)
        {
            Vector3 position = (new Vector3(horizontalSpacing * i, 0, 0)) + location.position;

            GameObject panel = GameObject.Instantiate(rocketUIPanel, position, Quaternion.identity, location);

            //store values in panel
            RocketMenuPanel panelProperties = panel.GetComponent<RocketMenuPanel>();
            panelProperties.icon.sprite = rocketPurchasables[i].icon;
            panelProperties.title.text = rocketPurchasables[i].title;
            
            panelProperties.item = rocketPurchasables[i].item;
            panelProperties.unlocked = rocketPurchasables[i].unlocked;

            panelProperties.unlockPrice = rocketPurchasables[i].unlockPrice;
            panelProperties.consumePrice = rocketPurchasables[i].price;
            panelProperties.shop = this;

            //store upgrade possibilities in panel

            Rocket thisRocket = rocketPurchasables[i].item.GetComponent<Rocket>();

            panelProperties.AddBlankUpgrades(thisRocket.upgrades.Count);
            
            for (int j = 0; j < thisRocket.upgrades.Count; j++)
            {
                //clones upgrades list from the rocket prefab to the panel
                panelProperties.upgrades[j].cost = thisRocket.upgrades[j].cost;
                panelProperties.upgrades[j].maxUpgrades = thisRocket.upgrades[j].maxUpgrades;
                panelProperties.upgrades[j].currentUpgrades = thisRocket.upgrades[j].currentUpgrades;
                panelProperties.upgrades[j].magnitude = thisRocket.upgrades[j].magnitude;
            }

            panelProperties.InitUnlockLogic();
        }
    }
}

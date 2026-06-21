using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ShopValues : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    [System.Serializable]

    class Purchasable
    {
        public GameObject obj;
        public int price;
        public string name;
    }

    [SerializeField] List<Purchasable> shoppables;

    [SerializeField] BuyControl buy;
    [SerializeField] RocketControl houston;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;
        
        RefreshShop();
        dropdown.captionText.text = "Astroshop";
    }

    public void GetDropdownValue()
    {
        
        int pickedEntryIndex = dropdown.value;
        string selectedOption = (dropdown.options[pickedEntryIndex]).text;

        if ((houston.meters - (shoppables[pickedEntryIndex].price)) >= 0)
        {
            buy.selectedBuy = shoppables[pickedEntryIndex].obj;
            buy.TriggerBuy((float)(shoppables[pickedEntryIndex].price));
        } else
        {
            Debug.Log("not enough money");
        }



        dropdown.captionText.text = "Astroshop";

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Add New Location")]
    void AddNewLocation(string name, int price)
    {
        string display = string.Concat(name, " - ", (price.ToString()));
        dropdown.options.Add(new TMP_Dropdown.OptionData(display, null, Color.white));

        dropdown.RefreshShownValue();
        

        //dropdown.AddOptions(List<TMP_Dropdown.OptionData>) adds multiple options at once.
    }

    void RemoveLocation(int index)
    {
        dropdown.options.RemoveAt(index);
        dropdown.RefreshShownValue();

    }

    void RefreshShop()
    {
        foreach (Purchasable item in shoppables)
        {
            
            AddNewLocation(item.name, item.price);

        }
    }
}

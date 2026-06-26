using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ShopValues : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Dropdown dropdown;


    //0 is select, 1 is purchase, 2 is deny
    [SerializeField] AudioClip[] sfx;



    [System.Serializable]
    class Purchasable
    {
        public GameObject obj;
        public int price;
        public string name;
        public int metalPrice;

        public string tooltipHeader;
        public string tooltipContent;
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

        dropdown.onValueChanged.AddListener(value => {
            Debug.Log("Pressed");
            dropdown.SetValueWithoutNotify(-1);
        });

        dropdown.SetValueWithoutNotify(-1);


    }

    public void GetDropdownValue()
    {

        
        


        int pickedEntryIndex = dropdown.value;
        string selectedOption = (dropdown.options[pickedEntryIndex]).text;


        if (((houston.meters - (shoppables[pickedEntryIndex].price)) >= 0) && buy.buying == false && ((houston.savedMetal - (shoppables[pickedEntryIndex].metalPrice)) >= 0))
        {
            buy.selectedBuy = shoppables[pickedEntryIndex].obj;
            buy.TriggerBuy((float)(shoppables[pickedEntryIndex].price), shoppables[pickedEntryIndex].metalPrice);

            AudioClip purchase = sfx[1];
            SoundFXManager.instance.PlaySoundEffectClip(purchase, Vector2.zero, 1f);

        } else
        {
            SoundFXManager.instance.PlaySoundEffectClip(sfx[2], transform.position, 1f);

        }



        dropdown.captionText.text = "Astroshop";

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Add New Location")]
    void AddNewLocation(string name, int price, string suffix, string color)
    {
        string display = string.Concat(name, " - " + color, (price.ToString()) + " " + suffix + "</color>");
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
            if (item.price == 0 && item.metalPrice > 0)
            {
                //metal color
                AddNewLocation(item.name, item.metalPrice, "t", "<color=#B98B8B>");
                return;
            }
            //meters color
            AddNewLocation(item.name, item.price, "km", "<color=#FFF000>");

        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //left click
        if (eventData.pointerId == -1)
        {
            AudioClip select = sfx[0];
            SoundFXManager.instance.PlaySoundEffectClip(select, Vector2.zero, 1f);



            TooltipTrigger[] info = dropdown.gameObject.GetComponentsInChildren<TooltipTrigger>();
            for (int i = 0; i < info.Length; i++)
            {
                info[i].header = shoppables[i].tooltipHeader;
                info[i].content = shoppables[i].tooltipContent;
            }
        }

    }
}

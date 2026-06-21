using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RocketMenuPanel : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public Image icon;
    [HideInInspector] public TextMeshProUGUI title;
    [HideInInspector] public TextMeshProUGUI price;
    [HideInInspector] public GameObject item;


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

    bool selectedForUpgrade = false;
    GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        bgColor = gameObject.GetComponent<Image>();


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



        } else
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

        } else if (unlocked == true)
        {
            isActive = !isActive;
            if (isActive)
            {
                if (shop.activePanel != null && shop.activePanel != this)
                {
                    //already an active rocket being used
                    shop.activePanel.DeactivatePanel();
                    shop.activePanel = this;
                } else
                {
                    shop.activePanel = this;
                }


                item.GetComponent<Rocket>().rocketPrice = consumePrice;
                houston.rocketPrefab = item;
                bgColor.color = activeColor;

            } else
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
                if (shop.selectedPanel != null)
                {
                    shop.selectedPanel.Deselect();
                    shop.selectedPanel = this;
                    
                } else
                {
                    shop.selectedPanel = this;
                }

                bgColor.color = selectedColor;
                UISingleton.instance.ToggleDropdown(3);

            } else
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
}

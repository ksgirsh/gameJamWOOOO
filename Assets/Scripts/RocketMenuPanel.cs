using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RocketMenuPanel : MonoBehaviour
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();
        bgColor = gameObject.GetComponent<Image>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitUnlockLogic()
    {
        Debug.Log("ran logic");
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

        Debug.Log("pressed");

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
}

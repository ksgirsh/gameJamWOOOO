using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Satellite : MonoBehaviour
{
    
    [SerializeField] GameObject parent;

    public float orbitRadius;
    public float orbitVelocity;
    public float rotateVelocity;
    private float orbitAngle;
    public Transform hookPoint;

    [SerializeField] Transform orbitRing;


    private Rigidbody2D rb;

    protected SelectControl control;

    public List<GameObject> loadedRockets;
    public int maxRockets;

    public bool auto = true;

    

    [System.Serializable]
    public class Upgrade
    {
        public int cost;
        public int maxUpgrades;
        public int currentUpgrades;
    }
    public List<Upgrade> upgrades;

    public List<TMP_Dropdown.OptionData> upgradeDropdownDisplay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if (parent == null)
        {
            parent = GameObject.FindGameObjectWithTag("Planet");
        }
        rb = gameObject.GetComponent<Rigidbody2D>();
        orbitRadius += parent.transform.localScale.x;
        Vector2 planetPos = parent.transform.position;
        transform.position = new Vector2(planetPos.x + orbitRadius, planetPos.y);

        if (orbitRing != null)
        {
            orbitRing.localScale = (Vector3.one * (orbitRadius) * 2);
            orbitRing.position = parent.transform.position;
            orbitRing.SetParent(null);
        }

        GameObject manager = GameObject.FindGameObjectWithTag("Player");
        control = manager.GetComponent<SelectControl>();
        rb.AddTorque(rotateVelocity, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        StartCoroutine(Orbit());
    }

    IEnumerator Orbit()
    {

        orbitAngle += Time.deltaTime * (orbitVelocity / orbitRadius);
        transform.position = new Vector2(Mathf.Cos(orbitAngle) * orbitRadius, Mathf.Sin(orbitAngle) * orbitRadius);
        yield return null;
    }

    Vector2 TangentToParent()
    {
        Vector2 distance = transform.position - parent.transform.position;   

        //rotate this 90 degrees using the satellite as a pivot point
        float satelliteToParentAngle = Mathf.Atan2(distance.y, distance.x);


        //should be rotated 90 degrees
        Vector2 rotatedPoint = new Vector2(Mathf.Sin(satelliteToParentAngle) * -1, Mathf.Cos(satelliteToParentAngle));

        Vector2 tangentDir = rotatedPoint.normalized;
        
        return tangentDir;



    }


    public void LoadRocket(GameObject obj)
    {
        loadedRockets.Add(obj);
    }

    public void UnloadRocket(GameObject obj)
    {
        loadedRockets.Remove(obj);
    }

    public void MouseSenseLogic(bool mouseEnter)
    {
        
        if (mouseEnter)
        {
            control.SelectTrigger(gameObject);

        } else
        {
            control.EraseTrigger();
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
                rb.angularVelocity = 0f;
                rotateVelocity += 1f;
                rb.AddTorque(rotateVelocity, ForceMode2D.Impulse);

                break;
            case 1:
                orbitVelocity += 2f;

                break;
            case 2:
                Debug.Log("Repaired Hook");
                break;


        }

        upgrades[index].currentUpgrades++;
    }

    public List<int> GetListOfPrices()
    {
        List<int> priceList = new List<int> { };
        foreach (Upgrade upgr in upgrades)
        {
            priceList.Add(upgr.cost);
        }
        return priceList;
    }

    public List<int> GetListOfRemainingUpgrades(int lastUpgrade = 0)
    {
        List<int> remList = new List<int> { };

        foreach (Upgrade upgr in upgrades)
        {
            remList.Add(((upgr.maxUpgrades) - (upgr.currentUpgrades)));
        }
        

        return remList;
    }

    public void ResetOptionsText(int justBought = 0, int boughtIndex = 0)
    {
        List<int> prices = GetListOfPrices();
        List<int> remUpgr = GetListOfRemainingUpgrades();

        for (int i = 0; i < upgradeDropdownDisplay.Count; i++)
        {

            //future change: remaining upgrade should be equal to the MINIMUM amount of upgrades among the selected list
            string addendum = string.Concat(" - ", prices[i].ToString(), "km (x", (remUpgr[i]).ToString(), ") ");

            if (i == boughtIndex)
            {
                addendum = string.Concat(" - ", prices[i].ToString(), "km (x", (remUpgr[i] + justBought).ToString(), ") ");
            }

            upgradeDropdownDisplay[i].text = (upgradeDropdownDisplay[i].text).Replace(addendum, "");
        }
    }

}

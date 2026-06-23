using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UISingleton : MonoBehaviour
{
    public static UISingleton instance;
    [field: SerializeField] public GameObject shopDrop { get; private set; }
    [field: SerializeField] public GameObject skyUpgradeDrop { get; private set; }
    [field: SerializeField] public GameObject rockUpgradeDrop { get; private set; }
    [field: SerializeField] public GameObject skyHealth { get; private set; }
    private HookHealthDisplay hookHeal;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hookHeal = skyHealth.GetComponent<HookHealthDisplay>();
        ToggleDropdown(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDropdown(int dropdown)
    {
        if (dropdown == 1)
        {
            skyUpgradeDrop.SetActive(false);
            rockUpgradeDrop.SetActive(false);
            shopDrop.SetActive(true);
        }

        if (dropdown == 2)
        {
            skyUpgradeDrop.SetActive(true);
            rockUpgradeDrop.SetActive(false);
            shopDrop.SetActive(false);

        }

        if (dropdown == 3)
        {
            skyUpgradeDrop.SetActive(false);
            rockUpgradeDrop.SetActive(true);
            shopDrop.SetActive(false);
        }

        if (dropdown > 3 || dropdown < 1)
        {
            Debug.Log("Dropdown index is out of Range, fix code so it toggles the right thing");
        }
    }

}

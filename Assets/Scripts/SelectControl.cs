using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;


public class SelectControl : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Camera mainCamera;

    [SerializeField] Transform hookCam;
    [SerializeField] GameObject panel;

    [SerializeField] GameObject selectEffect;
    [SerializeField] GameObject lockEffect;

    private GameObject currentSelObj;


    public GameObject selectedHook;
    public List<GameObject> lockedHooks;
    [SerializeField] List<GameObject> currentLockEffects;

    [HideInInspector] public GameObject nearestHook;

    [SerializeField] RocketShop rockShop;
    protected HookUpgradeHandler hookUpgrader;

    [SerializeField] GameObject dragSelection;


    [Header("Sound")]
    //0 is locking
    [SerializeField] AudioClip[] sfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.SetActive(false);


        //get a list of all satellites
        // find one with the smallest radius
        // add that one as "nearest hook"
        GameObject[] allSatellites = GameObject.FindGameObjectsWithTag("Satellite");
        float cachedRad = 999f;
        GameObject cachedSatellite = null;

        foreach (GameObject satellite in allSatellites)
        {
            if (satellite.GetComponent<Satellite>().orbitRadius < cachedRad)
            {
                cachedRad = satellite.GetComponent<Satellite>().orbitRadius;
                cachedSatellite = satellite;
            }
        }

        nearestHook = cachedSatellite;
        hookUpgrader = UISingleton.instance.skyUpgradeDrop.GetComponent<HookUpgradeHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentSelObj != null && Input.GetButtonDown("Fire1"))
        {
            LockSelect();
        } else if (Input.GetButtonDown("Fire1"))
        {

        }

        if (Input.GetButtonDown("Fire2") && lockedHooks.Count > 0)
        {
            //erases most recent lock on trigger
            EraseLockOn(lockedHooks.Count - 1);
        }

        if (lockedHooks.Count > 0)
        {
            //replace shop dropdown with skyhook dropdown
            UISingleton.instance.ToggleDropdown(2);

        }
    }

    public void SelectTrigger(GameObject hook)
    {
        
        //not really for hooks, also for aliens and probably asteroids

        //check to make sure you cant select locked hooks
        foreach (GameObject lHook in lockedHooks)
        {
            if (lHook == hook)
            {
                return;
            }
        }

        
        //selection magic
        Transform hookTrans = hook.transform;
        currentSelObj = GameObject.Instantiate(selectEffect, hook.transform.position, transform.rotation, hookTrans);
        currentSelObj.transform.localPosition = Vector3.zero;

        if (hook.gameObject.tag == "Satellite")
        {
            selectedHook = hook;
        }


        hookCam.GetComponent<AttachToObject>().target = hookTrans;
        panel.SetActive(true);

        //holy reference batman
        UISingleton.instance.skyHealth.SetActive(true);
        UISingleton.instance.skyHealth.GetComponent<HookHealthDisplay>().satHealth = hook.GetComponent<Health>();
    }

    public void EraseTrigger()
    {
        Destroy(currentSelObj);

        selectedHook = null;

        if (lockedHooks.Count == 0)
        {
            hookCam.SetParent(null);

            panel.SetActive(false);
            UISingleton.instance.skyHealth.SetActive(false);
        }

    }

    void LockSelect()
    {
        //you should not be able to lock onto already locked hooks, because you have to be selecting a hook to lock onto it. Selecting already queries locked hooks, so we're fine.

        if (selectedHook != null)
        {
            GameObject lockObj = GameObject.Instantiate(lockEffect, currentSelObj.transform.position, Quaternion.identity, currentSelObj.transform.parent);
            lockObj.transform.localPosition = Vector3.zero;
            Destroy(currentSelObj);



            lockedHooks.Add(selectedHook);
            currentLockEffects.Add(lockObj);

            int mostRecent = (lockedHooks.Count - 1);
            //Update Hook Upgrade Options with most recently locked Hook
            Satellite sat = lockedHooks[mostRecent].GetComponent<Satellite>();
            hookUpgrader.satelliteToUpgrade = sat;

            hookUpgrader.LockedHooksToList();
            if (hookUpgrader.GetComponent<TMP_Dropdown>().interactable)
            {
                hookUpgrader.ChangeUpgrades((sat.upgradeDropdownDisplay), sat.GetListOfPrices(), sat.GetListOfRemainingUpgrades());
            }


            hookCam.GetComponent<AttachToObject>().target = lockedHooks[mostRecent].transform;

            panel.SetActive(true);
            UISingleton.instance.skyHealth.SetActive(true);
            UISingleton.instance.skyHealth.GetComponent<HookHealthDisplay>().satHealth = lockedHooks[mostRecent].GetComponent<Health>();

            selectedHook = null;

            //placed hook sfx
            AudioClip lockk = sfx[0];
            SoundFXManager.instance.PlaySoundEffectClip(lockk, Vector2.zero, 1f);
        }
        
    }

    public void EraseLockOn(int index)
    {
        Destroy(currentLockEffects[index]);

        lockedHooks.Remove(lockedHooks[index]);
        currentLockEffects.Remove(currentLockEffects[index]);

        if (lockedHooks.Count == 0)
        {
            

            //in the future, check if rockets are selected, if so then enable that dropdown. otherwise enable shop drop
            if (rockShop != null)
            {
                if (rockShop.selectedPanel != null)
                {
                    UISingleton.instance.ToggleDropdown(3);
                    UISingleton.instance.skyHealth.SetActive(false);

                } else
                {
                    UISingleton.instance.ToggleDropdown(1);
                    UISingleton.instance.skyHealth.SetActive(false);
                }
            }

            hookCam.SetParent(null);

            panel.SetActive(false);
            UISingleton.instance.skyHealth.SetActive(false);

        } else
        {
            Satellite sat = lockedHooks[0].GetComponent<Satellite>();
            hookUpgrader.satelliteToUpgrade = sat;
            hookUpgrader.LockedHooksToList();
            hookUpgrader.ChangeUpgrades((sat.upgradeDropdownDisplay), sat.GetListOfPrices(), sat.GetListOfRemainingUpgrades());
            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //left click
        if (eventData.pointerId == -1)
        {
            
        }

    }


}

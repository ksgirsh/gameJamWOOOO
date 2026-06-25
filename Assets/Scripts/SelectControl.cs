using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class SelectControl : MonoBehaviour
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

    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform dragSelection;
    private Vector2 originalDragPos;
    private bool isDragging;
    [HideInInspector] public List<GameObject> currentSelectEffects;
    [HideInInspector] public List<GameObject> selectHookList;

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
        dragSelection.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (currentSelObj != null && Input.GetButtonDown("Fire1"))
        {
            LockSelect();

        } else if (Input.GetButtonDown("Fire1"))
        {
            //create selectbox
            CreateSelection();

        }

        if (dragSelection.gameObject.activeSelf)
        {
            UpdateSelection();
        }


        if (Input.GetButtonUp("Fire1"))
        {
            EraseSelection();
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

    void CreateSelection()
    {
        dragSelection.gameObject.SetActive(true);
        originalDragPos = Input.mousePosition;
       
    }

    void UpdateSelection()
    {
        float scaleFac = canvas.scaleFactor;
        Vector2 newPos = (Input.mousePosition);
        Vector2 dist = (originalDragPos - newPos);
        Vector2 sizeChange = new Vector2(Mathf.Abs(dist.x / scaleFac), Mathf.Abs(dist.y / scaleFac));
        
        dragSelection.sizeDelta = sizeChange;
        dragSelection.anchoredPosition = ((originalDragPos / scaleFac) + newPos) / 2;
        SelectWithBox();
        CheckSelectedObjects();
    }

    void EraseSelection()
    {

        dragSelection.gameObject.SetActive(false);
        originalDragPos = Vector2.zero;
        isDragging = false;

        if (currentSelectEffects.Count > 1)
        {
            LockWithBox();
        }

    }

    void SelectWithBox()
    {
        //for each hook
        GameObject[] allSats = GameObject.FindGameObjectsWithTag("Satellite");

        
        //left right bottom top

        float[] directions = {0, 0, 0, 0};
        directions[0] = dragSelection.anchoredPosition.x - (dragSelection.sizeDelta.x / 2);
        directions[1] = dragSelection.anchoredPosition.x + (dragSelection.sizeDelta.x / 2);
        directions[2] = dragSelection.anchoredPosition.y - (dragSelection.sizeDelta.y / 2);
        directions[3] = dragSelection.anchoredPosition.y + (dragSelection.sizeDelta.y / 2);

        for (int i = 0; i < allSats.Length; i++)
        {
            GameObject sat = allSats[i];

            Vector3 screenPos = mainCamera.WorldToScreenPoint(sat.transform.position);
            if (((screenPos.x > directions[0]) && (screenPos.x < directions[1])) && ((screenPos.y > directions[2]) && (screenPos.y < directions[3])))
            {
                for (int j = 0; j < lockedHooks.Count; j++)
                {
                    if (sat == lockedHooks[j])
                    {
                        return;

                    }

                }

                if (currentSelectEffects.Count <= i)
                {
                        //selection magic
                        Transform hookTrans = sat.transform;
                        GameObject sel = GameObject.Instantiate(selectEffect, sat.transform.position, transform.rotation, hookTrans);
                        sel.transform.localPosition = Vector3.zero;
                        currentSelectEffects.Add(sel);
                        selectHookList.Add(sat);

                }



            }
        }

    }

    void CheckSelectedObjects()
    {
        float[] directions = { 0, 0, 0, 0 };
        directions[0] = dragSelection.anchoredPosition.x - (dragSelection.sizeDelta.x / 2);
        directions[1] = dragSelection.anchoredPosition.x + (dragSelection.sizeDelta.x / 2);
        directions[2] = dragSelection.anchoredPosition.y - (dragSelection.sizeDelta.y / 2);
        directions[3] = dragSelection.anchoredPosition.y + (dragSelection.sizeDelta.y / 2);

        for (int i = 0; i < selectHookList.Count; i++)
        {
            GameObject sat = selectHookList[i];

            Vector3 screenPos = mainCamera.WorldToScreenPoint(sat.transform.position);
            if (((screenPos.x > directions[0]) && (screenPos.x < directions[1])) && ((screenPos.y > directions[2]) && (screenPos.y < directions[3])))
            {
                return;

            } else
            {

                Destroy(currentSelectEffects[i]);
                currentSelectEffects.Remove(currentSelectEffects[i]);
                selectHookList.Remove(selectHookList[i]);
            }
        }
    }

    void LockWithBox()
    {
        for (int i = 0; i < currentSelectEffects.Count; i++)
        {
            GameObject lockObj = GameObject.Instantiate(lockEffect, currentSelectEffects[i].transform.position, Quaternion.identity, currentSelectEffects[i].transform.parent);
            lockObj.transform.localPosition = Vector3.zero;
            

            if (selectHookList[i] != null)
            {
                lockedHooks.Add(selectHookList[i]);
                Destroy(currentSelectEffects[i]);
            }


            currentLockEffects.Add(lockObj);


        }
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


        //placed hook sfx
        AudioClip lockk = sfx[0];
        SoundFXManager.instance.PlaySoundEffectClip(lockk, Vector2.zero, 1f);

        selectHookList.Clear();
        currentSelectEffects.Clear();
    }
}

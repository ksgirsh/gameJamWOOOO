using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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
                Debug.Log(cachedRad);
                cachedSatellite = satellite;
            }
        }

        nearestHook = cachedSatellite;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSelObj != null && Input.GetButtonDown("Fire1"))
        {
            LockSelect();
        }
    }

    public void SelectTrigger(GameObject hook)
    {
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

        selectedHook = hook;

        hookCam.GetComponent<AttachToObject>().target = hookTrans;
        panel.SetActive(true);
    }

    public void EraseTrigger()
    {
        Destroy(currentSelObj);

        selectedHook = null;

        hookCam.SetParent(null);

        panel.SetActive(false);

    }

    void LockSelect()
    {
        //you should not be able to lock onto already locked hooks, because you have to be selecting a hook to lock onto it. Selecting already queries locked hooks, so we're fine.

        GameObject lockObj = GameObject.Instantiate(lockEffect, currentSelObj.transform.position, Quaternion.identity, currentSelObj.transform.parent);
        lockObj.transform.localPosition = Vector3.zero;
        Destroy(currentSelObj);

        lockedHooks.Add(selectedHook);
        currentLockEffects.Add(lockObj);

        selectedHook = null;
    }

    public void EraseLockOn(int index)
    {
        Destroy(currentLockEffects[index]);
        lockedHooks.Remove(lockedHooks[index]);
        currentLockEffects.Remove(currentLockEffects[index]);
    }

    
}

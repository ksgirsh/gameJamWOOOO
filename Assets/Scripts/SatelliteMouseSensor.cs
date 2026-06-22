using UnityEngine;

public class SatelliteMouseSensor : MonoBehaviour
{
    [SerializeField] GameObject satellite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        satellite = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        
        satellite.GetComponent<Satellite>().MouseSenseLogic(true);
    }

    void OnMouseExit()
    {
        
        satellite.GetComponent<Satellite>().MouseSenseLogic(false);
    }

}

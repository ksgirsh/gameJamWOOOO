using UnityEngine;

public class PlanetMouseSensor : MonoBehaviour
{
    SelectControl control;
    [SerializeField] GameObject planet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        control = player.GetComponent<SelectControl>();
    }

    void OnMouseEnter()
    {
        control.SelectTrigger(planet);
        Debug.Log("entered");
    }

    void OnMouseExit()
    {
        control.EraseTrigger();
    }

}

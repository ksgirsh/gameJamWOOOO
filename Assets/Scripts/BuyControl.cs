using UnityEngine;

public class BuyControl : MonoBehaviour
{
    public GameObject selectedBuy;
    public Camera mainCam;

    [SerializeField] RocketControl houston;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = (GameObject.FindGameObjectWithTag("MainCamera")).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && (houston.meters - (selectedBuy.GetComponent<Buyable>().buyPrice)) >= 0)
        {
            TriggerBuy();
            houston.savedDistance -= (selectedBuy.GetComponent<Buyable>().buyPrice);


        }
    }
    
    void TriggerBuy()
    {
        
        GameObject prev = GameObject.Instantiate((selectedBuy.GetComponent<Buyable>().preview), WorldMousePos(), Quaternion.identity);
    }

    Vector3 WorldMousePos(float z = 0)
    {
        Vector3 mouseVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mouseVector.x, mouseVector.y, 0);

    }
}

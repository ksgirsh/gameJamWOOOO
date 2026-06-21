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
        if (Input.GetKeyDown(KeyCode.P))
        {
            TriggerBuy(100);
            


        }
    }
    
    public void TriggerBuy(float price)
    {
        
        GameObject prev = GameObject.Instantiate((selectedBuy.GetComponent<Buyable>().preview), WorldMousePos(), Quaternion.identity);

        //this logic feels weirdly circular and confusing. Theres a cleaner way to do this that hasnt gotten through my thick skull
        prev.GetComponent<Preview>().realVersion = selectedBuy;
        prev.GetComponent<Preview>().cost = price;

    }

    Vector3 WorldMousePos(float z = 0)
    {
        Vector3 mouseVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mouseVector.x, mouseVector.y, 0);

    }
}

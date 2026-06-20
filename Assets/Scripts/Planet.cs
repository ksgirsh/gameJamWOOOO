using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    private SelectControl control;

    [SerializeField] GameObject rocketHoverEffect;

    private GameObject currentRocketObj;

    [SerializeField] LayerMask surfaceLayers;

    public RocketControl rocket;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        rocket = player.GetComponent<RocketControl>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        currentRocketObj = GameObject.Instantiate(rocketHoverEffect, MouseToSurfaceRay(this.transform), Quaternion.identity, this.transform);
        rocket.planet = gameObject;
    }

    void OnMouseOver()
    {

        Vector2 rayPoint = MouseToSurfaceRay(this.transform);
        currentRocketObj.transform.position = new Vector3(rayPoint.x, rayPoint.y, 0f);
       // Debug.Log(rayPoint);

    }
    void OnMouseExit()
    {
        Destroy(currentRocketObj);
        rocket.planet = null;
    }

    public Vector2 MouseToSurfaceRay(Transform planetPos)
    {
        //cast ray from mouse position directed towards planet

        Vector2 distVector = (mainCamera.ScreenToWorldPoint(Input.mousePosition)) - planetPos.position;
       // Debug.Log(-(distVector.normalized));
        RaycastHit2D hit = Physics2D.Raycast((mainCamera.ScreenToWorldPoint(Input.mousePosition)), -(distVector.normalized), Mathf.Infinity, surfaceLayers);

        if (hit != null)
        {
            return hit.point;
        } else
        {
            return Vector2.zero;
        }

    }

    public Vector2 MouseToSurfaceNormal(Transform planetPos)
    {
        //cast ray from mouse position directed towards planet

        Vector2 distVector = (mainCamera.ScreenToWorldPoint(Input.mousePosition)) - planetPos.position;
       // Debug.Log(-(distVector.normalized));
        RaycastHit2D hit = Physics2D.Raycast((mainCamera.ScreenToWorldPoint(Input.mousePosition)), -(distVector.normalized), Mathf.Infinity, surfaceLayers);

        if (hit != null)
        {
            return hit.normal;
        }
        else
        {
            return Vector2.zero;
        }

    }

}

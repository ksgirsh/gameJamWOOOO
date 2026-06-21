using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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

    private SelectControl control;

    public List<GameObject> loadedRockets;
    public int maxRockets;

    public bool auto = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
    void Update()
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

    void OnMouseEnter()
    {
        control.SelectTrigger(gameObject);
    }

    void OnMouseExit()
    {
        control.EraseTrigger();
    }
}

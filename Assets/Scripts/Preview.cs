using UnityEngine;

public class Preview : MonoBehaviour
{
    public GameObject realVersion;
    public Camera mainCam;
    [SerializeField] string identifier;

    [HideInInspector] public float cost;
    private RocketControl houston;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = (GameObject.FindGameObjectWithTag("MainCamera")).GetComponent<Camera>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        houston = player.GetComponent<RocketControl>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = WorldMousePos();
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject real = GameObject.Instantiate(realVersion, transform.position, Quaternion.identity);

            if (identifier == "Satellite")
            {
                //reminder to remove the "parent addendum" later
                GameObject parent = GameObject.FindGameObjectWithTag("Planet");
                real.GetComponent<Satellite>().orbitRadius = (Mathf.Abs(transform.position.x) - parent.transform.localScale.x);

                
            }

            houston.savedDistance -= cost;
            Destroy(gameObject);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            //cancel purchase
            Destroy(gameObject);
        }
    }

    Vector3 WorldMousePos()
    {
        Vector3 mouseVector = mainCam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mouseVector.x, mouseVector.y, 0);
    }
}

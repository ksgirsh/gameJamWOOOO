using UnityEngine;

public class AttachToObject : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public float zOffset = -10.0f;
    [SerializeField] float xOffset;

    [SerializeField] float yOffset;

    void Start()
    {
        /*
        Transform trans = GetComponent<Transform>();
        Vector3 pos = trans.position;
        pos.z += zOffset;
        pos.x += xOffset * trans.localScale.x;
        this.transform.position = pos;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 pos = target.position;
            pos.z += zOffset;
            pos.x += xOffset;
            pos.y += yOffset;
            this.transform.position = pos;
        }

    }



}

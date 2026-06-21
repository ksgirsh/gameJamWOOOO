using UnityEngine;

public class MeterTarget : TargetAttribute
{
    public float targetPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        identifier = "Meter Target";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TargetHit()
    {
        ps.Play();
        ps.gameObject.transform.SetParent(null);
        Destroy(ps.gameObject, 1f);
        Destroy(gameObject);
    }
}

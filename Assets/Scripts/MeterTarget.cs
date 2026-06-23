using UnityEngine;

public class MeterTarget : TargetAttribute
{
    public float targetPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        identifier = "Meter Target";
        targetPoints += Random.Range(-100f, 100f);
        //base.Start();

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
        TriggerDestroy();
    }
}

using UnityEngine;

public class TargetAttribute : MonoBehaviour
{
    public string identifier;
    public ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

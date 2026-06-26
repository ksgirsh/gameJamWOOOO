using UnityEngine;

public class TargetAttribute : MonoBehaviour
{
    public string identifier;
    public ParticleSystem ps;
    public int spawnChance;

    public AudioClip[] sfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        ps.Stop();
        SoundFXManager.instance.PlaySoundEffectClip(sfx[0], transform.position, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void TriggerDestroy()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        TargetSpawn targetManager = player.GetComponent<TargetSpawn>();
        targetManager.currentTargets--;
        Destroy(gameObject);
    }
}

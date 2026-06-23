using UnityEngine;
using UnityEngine.UI;

public class HookHealthDisplay : MonoBehaviour
{
    [SerializeField] Image healthMat;
    public Health satHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (satHealth != null)
        {
            healthMat.material.SetFloat("_Health", (satHealth.NormalizedHealth()));
        }


    }

}

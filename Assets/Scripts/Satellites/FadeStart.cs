using UnityEngine;
using UnityEngine.UI;

public class FadeStart : MonoBehaviour
{
    FadeIn fade;
    Image img;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fade = gameObject.GetComponent<FadeIn>();
        img = gameObject.GetComponent<Image>();
        StartCoroutine(fade.FadeInImg(1f, img, 1));
    }

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PulseColorSpr(float pulseDur, GameObject gameObj, Color targetColor)
    {
        Color initColor = Color.black;
        if (gameObj.GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer rend = gameObj.GetComponent<SpriteRenderer>();
            initColor = rend.color;
        }
        else if ((gameObj.GetComponent<Image>() != null))
        {
            Image rend = gameObj.GetComponent<Image>();
            initColor = rend.color;

        }

        for (float t = 0f; t < pulseDur; t += Time.deltaTime)
        {
            float normalizedTime = t / pulseDur;

            Color lerpColor = Color.Lerp(targetColor, initColor, t);

            if (gameObj.GetComponent<SpriteRenderer>() != null)
            {
                SpriteRenderer rend = gameObj.GetComponent<SpriteRenderer>();
                rend.color = lerpColor;
            }
            else if ((gameObj.GetComponent<Image>() != null))
            {
                Image rend = gameObj.GetComponent<Image>();
                rend.color = lerpColor;

            }

            yield return null;
        }


        if (gameObj.GetComponent<Image>() != null)
        {
            Image rend = gameObj.GetComponent<Image>();
            rend.color = initColor;

        }

        if (gameObj.GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer rend = gameObj.GetComponent<SpriteRenderer>();
            rend.color = initColor;
        }
    }
}

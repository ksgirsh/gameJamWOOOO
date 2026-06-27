using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;
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

    public void FadeInObj(GameObject obj, float dur = 1f, int dir = 0)
    {
        List<Transform> objectsToFade = new List<Transform>();
        objectsToFade.Add(obj.transform);
        AddDescendants(obj.transform, objectsToFade);
        //get all children with an image

        List<Image> spritesToFade = new List<Image>();
        foreach (Transform imageCheck in objectsToFade)
        {
            if (imageCheck.gameObject.activeSelf == true)
            {
                if (imageCheck.gameObject.GetComponent<Image>() != null)
                {
                    Image imgElem = imageCheck.GetComponent<Image>();
                    spritesToFade.Add(imgElem);
                }
            }

            if (imageCheck.gameObject.GetComponent<TextMeshProUGUI>() != null)
            {
                TextMeshProUGUI txt = imageCheck.gameObject.GetComponent<TextMeshProUGUI>();
                StartCoroutine(FadeInText(dur, txt, dir));
            }

        }

        //for each image, fade 

        foreach (Image img in spritesToFade)
        {
            //find way to fix these magic numbers and add more customizablity

            StartCoroutine(FadeInImg(dur, img, dir));
        }

    }


    public IEnumerator FadeInImg(float dur, Image img, int direction = 0, float fadeTo = 1f)
    {
        //Debug.Log((0f + direction * fadeTo) + " " + ((1f * fadeTo) - (direction * fadeTo)));
        Color initColor = img.color;


        float target = (1f * fadeTo) - (direction * fadeTo);

        if (fadeTo == 1 && direction == 0)
        {
            //fades to initial alpha instead of 1.
            target = initColor.a;
            // Debug.Log("Set Target Alpha: " + target);
        }

        for (float i = 0; i < dur; i += Time.deltaTime)
        {
            float normI = i / dur;

            //if direction is 1 then it fades out
            float alpha = Mathf.Lerp(0f + direction * fadeTo, target, normI);

            Color newImgColor = new Color(initColor.r, initColor.g, initColor.b, alpha);

            img.color = newImgColor;

            //Debug.Log("Fading");
            yield return null;
        }

        Color finC = new Color(initColor.r, initColor.g, initColor.b, target);
        img.color = finC;
    }

    IEnumerator FadeInText(float dur, TextMeshProUGUI text, int direction = 0)
    {
        for (float i = 0; i < dur; i += Time.deltaTime)
        {
            float normI = i / dur;
            float alpha = Mathf.Lerp(0f + direction, 1f - direction, normI);

            Color newImgColor = new Color(text.color.r, text.color.g, text.color.b, alpha);

            text.color = newImgColor;
            yield return null;
        }

        float target = 1f - direction;
        Color finC = new Color(text.color.r, text.color.g, text.color.b, target);
        text.color = finC;
    }

    private void AddDescendants(Transform parent, List<Transform> list)
    {
        foreach (Transform child in parent)
        {
            list.Add(child);
            AddDescendants(child, list);
        }
    }

}

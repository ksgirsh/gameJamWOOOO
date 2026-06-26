using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public string content;

    private float wait = 0.85f;

    bool touching = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(Delay(true));
        touching = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        touching = false;
        StartCoroutine(Delay(false));

    }

    IEnumerator Delay(bool show)
    {

        if (show)
        {
            yield return new WaitForSeconds(wait);
            if (touching)
            {
                TooltipSystem.Show(content, header);
            }

        } else
        {
            yield return null;
            TooltipSystem.Hide();
        }
    }
}

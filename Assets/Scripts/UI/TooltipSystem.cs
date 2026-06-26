using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;

    public void Awake()
    {
        current = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Show(string content, string header = "")
    {
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);

        if (current.tooltip.gameObject.activeSelf)
        {
            current.tooltip.fade.FadeInObj(current.tooltip.gameObject, 0.12f);
        }

    }

    public static void Hide()
    {
        if (current.tooltip.gameObject.activeSelf)
        {
            current.tooltip.fade.FadeInObj(current.tooltip.gameObject, 0.12f, 1);
        }

        current.tooltip.gameObject.SetActive(false);
        //fades OUT object actually
        
    }
}

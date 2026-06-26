using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{

    public TextMeshProUGUI headerField;

    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    [SerializeField] Canvas canvas;

    public RectTransform rectTransform;

    public FadeIn fade;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;

    }

    public void SetText(string content, string header = "")
    {
        //UpdateTextLength();
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);

        } else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DropdownController : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;
        dropdown.captionText.text = "Astroshop";
    }

    public void GetDropdownValue()
    {
        dropdown.captionText.text = "Astroshop";
        int pickedEntryIndex = dropdown.value;
        string selectedOption = (dropdown.options[pickedEntryIndex]).text;
        Debug.Log(selectedOption);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Add New Location")]
    void AddNewLocation()
    {
        string name = "New Hook";
        dropdown.options.Add(new TMP_Dropdown.OptionData(name, null, Color.white));

        dropdown.RefreshShownValue();
        

        //dropdown.AddOptions(List<TMP_Dropdown.OptionData>) adds multiple options at once.
    }

    void RemoveLocation(int index)
    {
        dropdown.options.RemoveAt(index);
    }
}

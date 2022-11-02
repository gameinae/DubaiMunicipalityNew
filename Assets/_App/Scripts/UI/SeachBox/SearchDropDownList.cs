using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchDropDownList : MonoBehaviour
{
    public GameObject ListItem;
    public List<GameObject> Buildinglist = new List<GameObject>();

    public void CreateOptionsList(List<Building> options)
    {
        int optionsCount = options.Count;

        for (int i = 0; i < optionsCount; i++)
        {
            GameObject item = Instantiate(ListItem, transform);
            string n = "";
            if (options[i].buildingData.name.Length > 15)
            {
                int count = options[i].buildingData.name.Length - 15;
                n = options[i].buildingData.name.Remove(15, count);
                n = n + "...";
            }
            else
            {
                n = options[i].buildingData.name;
            }
            item.name = options[i].buildingData.name;
            item.GetComponent<TextMeshProUGUI>().text = n;
            Buildinglist.Add(item);
        }
    }
    public void ChangeDropDownListOptions(string searchWord)
    {
        foreach (var b in Buildinglist)
        {
            if (!b.name.ToLower().StartsWith(searchWord))
            {
                b.gameObject.SetActive(false);
            }
        }
    }
    public void ActivateAllListOptions()
    {
        foreach (var b in Buildinglist)
        {
            b.gameObject.SetActive(true);
        }
    }
    
}

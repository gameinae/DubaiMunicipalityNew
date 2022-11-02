using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDownList : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown List;
    
    private void Awake()
    {
        List = GetComponent<TMP_Dropdown>();
    }

    //TODO: Remove UpdateFunction
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O)) 
        {
            Debug.Log(GetSelectedOption());
        }
    }
    public void AssignListOptions(Building[] buildings) 
    {
        List<string> buildingsNames = new List<string>();

        for (int b = 0; b < buildings.Length; b++) 
        {
            buildingsNames.Add(buildings[b].name);
        }

        List.AddOptions(buildingsNames);
    }
    public void AssignListOptions(District[] districts)
    {
        List<string> districtssNames = new List<string>();

        for (int b = 0; b < districts.Length; b++)
        {
            districtssNames.Add(districts[b].name);
        }

        List.AddOptions(districtssNames);
    }
    public int GetSelectedOption() 
    {
        return List.value;
    }
    public TMP_Dropdown GetList() 
    {
        return List;
    }
}

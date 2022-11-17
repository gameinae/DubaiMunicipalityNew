using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MonthPickerUI : MonoBehaviour
{
    public int year = DateTime.Today.Year;
    public int month = DateTime.Today.Month;
    public Text YearText;
    public GameObject MonthsButtonsParent; 
    void Start()
    {
        DeselectAll();
        ChangeYear(0);
    }

    public void PickMonth(int m)
    {
        month = m;
        DeselectAll();
        MonthsButtonsParent.transform.GetChild(month - 1).GetChild(0).gameObject.SetActive(true);
        MonthsButtonsParent.transform.GetChild(month - 1).GetChild(1).gameObject.SetActive(false);
        MonthsButtonsParent.transform.GetChild(month - 1).GetChild(2).GetComponent<Text>().color = Color.black;

    }

    void DeselectAll()
    {
        for(int i = 0; i < MonthsButtonsParent.transform.childCount; i++)
        {
            MonthsButtonsParent.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            MonthsButtonsParent.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            MonthsButtonsParent.transform.GetChild(i).GetChild(2).GetComponent<Text>().color= Color.white;
        }
    }

    public void ChangeYear(int amount)
    {
        year += amount;
        YearText.text = year.ToString();
    }
    public void AddYear()
    {
        ChangeYear(1);
    }
    public void SubtractYear()
    {
        ChangeYear(-1);
    }

}

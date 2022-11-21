using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class MonthPickerUI : MonoBehaviour
{
    public int year = DateTime.Today.Year;
    public int month = DateTime.Today.Month;
    public Text YearText;
    public GameObject MonthsButtonsParent;

    Vector3 initialScale = Vector3.one;
    public UnityEvent onWindowAnimationOpenComplete;
    public UnityEvent onWindowAnimationCloseComplete;

  
    void Start()
    {

        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
        PickCurrentDate();
    }

    public void PickMonth(int m)
    {
        month = m;
        DeselectAll();
        MonthsButtonsParent.transform.GetChild(month - 1).GetChild(0).gameObject.SetActive(false);
        MonthsButtonsParent.transform.GetChild(month - 1).GetChild(1).gameObject.SetActive(true);
        MonthsButtonsParent.transform.GetChild(month - 1).GetChild(2).GetComponent<Text>().color = Color.black;

    }

    void DeselectAll()
    {
        for(int i = 0; i < MonthsButtonsParent.transform.childCount; i++)
        {
            MonthsButtonsParent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            MonthsButtonsParent.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
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

    public void PickCurrentDate()
    {
        PickMonth(DateTime.Now.Month);
        ChangeYear(DateTime.Now.Year-year);
    }

    private void OnEnable()
    {
        StartCoroutine(AnimateWindow(true));
    }

    public void OnApply()
    {
        StartCoroutine(AnimateWindow(false));
    }
    IEnumerator AnimateWindow(bool opening = true)
    {
        float t = 0f;

        while (t < 1f)
        {
            if (opening)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale, t);

            }
            else
            {

                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);

            }
            t =  t+(Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();

        }
        (opening ? onWindowAnimationOpenComplete : onWindowAnimationCloseComplete).Invoke();
        StopAllCoroutines();

    }

}

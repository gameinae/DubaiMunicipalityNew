using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMonthUIToDatePicker : MonoBehaviour
{
    public MonthPickerUI monthPickerUI;
    public DatePickerControl datePickerControl;

    public void ApplyMapping()
    {
        datePickerControl.fecha = new System.DateTime(monthPickerUI.year, monthPickerUI.month, 1, 0, 0, 0);
        datePickerControl.actualizarFecha();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System.Linq;
using TMPro;
public class Graph : MonoBehaviour {

    public static Graph instance =null;
   
  

   

    // Cached values
    public List<float>  YValueList_Weeklydata, YValueList_Monthlydata;

    public List<float>  FogVol_YValueList_Weeklydata, FogVol_YValueList_Monthlydata;

    public List<float> Septic_Rise_YValueList_Weeklydata, Septic_Rise_YValueList_Monthlydata;

    public List<float> Septic_Vol_YValueList_WeeklyData, Septic_Vol_YValueList_MonthlyData;

    public List<float> Pipeline_Rise_YValueList_Weeklydata, Pipeline_Rise_YValueList_Monthlydata;

    public List<float> Pipeline_Vol_YValueList_WeeklyData, Pipeline_Vol_YValueList_MonthlyData;

    private void Awake() {
       if(instance==null) instance = this;
       
    }
   
}

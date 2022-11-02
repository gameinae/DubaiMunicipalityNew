using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictPanel : MonoBehaviour
{
    [SerializeField] GameObject[] Middlecharts,BottomCharts, BlockModels;
    [SerializeField] GameObject Cam;
    public void SwitchChart(int index) 
    {
        for (int i = 0; i < Middlecharts.Length; i++) {
            Middlecharts[i].gameObject.SetActive(false);
            BottomCharts[i].gameObject.SetActive(false);
        }
        Middlecharts[index].gameObject.SetActive(true);
        BottomCharts[index].gameObject.SetActive(true);

        for (int i=0;i< BlockModels.Length;i++) {
            BlockModels[i].gameObject.SetActive(false);
        }
        BlockModels[index].gameObject.SetActive(true);
        Cam.SetActive(false);
        Cam.SetActive(true);
    }

}

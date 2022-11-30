using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleHighliting : MonoBehaviour
{
    public GameObject districts;
    public GameObject subDistricts;

    public static ToogleHighliting instance;
   
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        districts = GameObject.FindGameObjectWithTag("Districts");
        subDistricts = GameObject.FindGameObjectWithTag("SubDistrict");

        subDistricts.SetActive(false);
    }

    public void EnableDistrictHigligting()
    {
        districts.SetActive(true);
        subDistricts.SetActive(false);
    }
    public void EnableSubDistrictHigligting()
    {

        districts.SetActive(false);
        subDistricts.SetActive(true);
    }
    public void EnableBuildingsUI()
    {

        districts.SetActive(false);
        subDistricts.SetActive(false);
        // add building UI to be activated
    }
}

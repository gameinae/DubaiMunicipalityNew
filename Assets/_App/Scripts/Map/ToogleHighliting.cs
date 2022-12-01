using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ToogleHighliting : MonoBehaviour
{
    public GameObject districts;
    public GameObject subDistricts;
    public List<GameObject> subDistrictsCanvases;

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
        subDistrictsCanvases = GameObject.FindGameObjectsWithTag("SubDistrictsCanvas").ToList();

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
        GameManager.Instance.UIManager.mainUICanvas.ActivateBuildingUI();
        // add building UI to be activated
    }
}

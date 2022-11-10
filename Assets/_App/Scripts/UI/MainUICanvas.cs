using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainUICanvas : MonoBehaviour
{
    public static MainUICanvas instance = null;
    public Animator animator;
    public TextMeshProUGUI districtName,buildingName,SensorName;

    public IntUnityAction unityAction;

    public GameObject DistrictsCanvasNew, MainSpotsCanvasNew, BuildingsInfoCanvasNew, BuildingsCamPositions, DistrictHolder,ZoomIn,ZoomOut;
    public BuilldingInfo _buildingInfo;
    [SerializeField] Button[] BurjKhalifaSensorButtons, PalmHotelSensorButtons;
    [SerializeField] Button[] AddressSkySensorButtons,HolidayInnSensorButtons;
    [SerializeField] Button[] MercureSensorButtons,SheratonSensorButtons;
    [SerializeField] GameObject BuildingsSensorPositions;
    public int DistrictIndex;
    public Transform LeftPanel, RightPanel;
    public Image[] HierarchyImages;
    public Sprite[] HierarchyImagesOn, HierarchyImagesOff,SidePanelToggleSprites;
    bool SidePanelTogglebool;
    [SerializeField] Image SidePanelImg;
    public bool zoomEnabled;
    private void Start()
    {
        foreach (Transform T in DistrictHolder.transform)
        {
            T.gameObject.GetComponent<Button>().onClick.AddListener(() => Highlight_Districts(T.gameObject));
        }
        foreach (Transform T in MainSpotsCanvasNew.transform)
        {
            T.gameObject.SetActive(false);
        }
        for (int i = 0; i < MainSpotsCanvasNew.transform.childCount; i++)
        {
            foreach (Transform T1 in MainSpotsCanvasNew.transform.GetChild(i).transform.GetChild(0).transform)
            {
                T1.gameObject.GetComponent<Button>().onClick.AddListener(() => Highlight_Buildings(T1.gameObject.name));
                T1.gameObject.GetComponent<Button>().onClick.AddListener(() => Get_Lat_Long(T1.gameObject.GetComponent<Building>()));
                T1.gameObject.GetComponent<Button>().onClick.AddListener(() => T1.transform.GetChild(0).gameObject.SetActive(false));
                
            }
        }

    }

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        animator = GetComponent<Animator>();
    }

    #region Buildings
    public void ChangeBuildingNameInAdress(string currentBuildingName)
    {
        buildingName.text = currentBuildingName;
        HierarchyImages[0].sprite = HierarchyImagesOn[0];
        HierarchyImages[1].sprite = HierarchyImagesOn[1];
        HierarchyImages[2].sprite = HierarchyImagesOff[2];
    }
    public void Highlight_Buildings(string buildingName)
    {
        MainUICanvas.instance.zoomEnabled = false;
        Debug.Log("buildingName" + buildingName);
        foreach (Transform T in MainSpotsCanvasNew.transform)
        {
            T.gameObject.SetActive(false);
          

        }
        _buildingInfo.BuildingName.text = buildingName;
        switch (buildingName)
        {
            case "BurjKhalifa":
                AssignBuildingPoint(0);
                break;
            case "PalmHotel":
                AssignBuildingPoint(1);
                break;
            case "AddressSkyView":
                AssignBuildingPoint(2);
                break;
            case "HolidayInnHotel":
                AssignBuildingPoint(3);
                break;
            case "Mercure":
                AssignBuildingPoint(4);
                break;
            case "Sheraton":
                AssignBuildingPoint(5);
                break;
            case "Kempinski":
                AssignBuildingPoint(6);
                break;
            case "DusitD2KenzHotel":
                AssignBuildingPoint(7);
                break;
            case "MediaByRotana":
                AssignBuildingPoint(8);
                break;
            case "IBIS&NovotelHotel":
                AssignBuildingPoint(9);
                break;
            case "NaraCafe":
                AssignBuildingPoint(10);
                break;
            case "LeMeridien":
                AssignBuildingPoint(11);
                break;
            case "WyndhamDubaiMarina":
                AssignBuildingPoint(12);
                break;
            case "MazahRestaurant":
                AssignBuildingPoint(13);
                break;
            case "FirstCentralHotelSuites":
                AssignBuildingPoint(14);
                break;
            case "ClassHotelApartments":
                AssignBuildingPoint(15);
                break;
            case "GrandCosmopolitanHotel":
                AssignBuildingPoint(16);
                break;
            case "HiltonDubaiAlHabtoorCity":
                AssignBuildingPoint(17);
                break;
            case "MajesticCityRetreatHotel":
                AssignBuildingPoint(18);
                break;
            default:
                break;
        }
    }
    void Get_Lat_Long(Building B)
    {
      //  _buildingInfo.gameObject.SetActive(true);
        _buildingInfo.Latitude.text = "Latitude " + B.buildingData.nCoordinate;
        _buildingInfo.Longitude.text = "Longitude " + B.buildingData.eCoordinate;
    }
    public void AssignBuildingPoint(int i)
    {

        GameManager.Instance.CameraController.MoveCameraToCertainPoint(BuildingsCamPositions.transform.GetChild(i).transform, false, 800);
        GameManager.Instance.GridSystem.ChangeBuilding(i);
        StartCoroutine(EnableInfo(i));
        SensorsPositions(i);
    }
    IEnumerator EnableInfo(int i)
    {
        yield return new WaitForSeconds(2.5f);
        _buildingInfo.gameObject.SetActive(true);

        if (BuildingsInfoCanvasNew.transform.childCount > i)
        {
            BuildingsInfoCanvasNew.transform.GetChild(i)?.gameObject.SetActive(true);
        }
    }

    #endregion

   public void SensorsPositions(int i)
    {
        buildingName.GetComponent<Button>().interactable = true;
        buildingName.gameObject.GetComponent<Button>().onClick.AddListener(() => Highlight_Buildings(buildingName.text));

        if (i==0)
        {
            for (int j=0; j <BurjKhalifaSensorButtons.Length;j++)
            {
                BurjKhalifaSensorButtons[j].gameObject.SetActive(true);
                BurjKhalifaSensorButtons[j].interactable = true;
            }
            BurjKhalifaSensorButtons[0].onClick.AddListener(() => FogSensorValues("Sensor1", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(0).transform));
            BurjKhalifaSensorButtons[1].onClick.AddListener(() => FogSensorValues("Sensor2", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(1).transform));           
            BurjKhalifaSensorButtons[2].onClick.AddListener(() => FogSensorValues("Sensor3", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(2).transform));


        }else if (i == 1)
        {
            for (int j = 0; j < PalmHotelSensorButtons.Length; j++)
            {
                PalmHotelSensorButtons[j].gameObject.SetActive(true);
                PalmHotelSensorButtons[j].interactable = true;
            }
            PalmHotelSensorButtons[0].onClick.AddListener(() => FogSensorValues("Sensor1", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(0).transform));
            PalmHotelSensorButtons[1].onClick.AddListener(() => FogSensorValues("Sensor2", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(1).transform));
            PalmHotelSensorButtons[2].onClick.AddListener(() => FogSensorValues("Sensor3", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(2).transform));

        }else if (i == 2)
        {
            for (int j = 0; j < AddressSkySensorButtons.Length; j++)
            {
                AddressSkySensorButtons[j].gameObject.SetActive(true);
                AddressSkySensorButtons[j].interactable = true;
            }
            AddressSkySensorButtons[0].onClick.AddListener(() => FogSensorValues("Sensor1", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(0).transform));
            AddressSkySensorButtons[1].onClick.AddListener(() => FogSensorValues("Sensor2", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(1).transform));
            AddressSkySensorButtons[2].onClick.AddListener(() => FogSensorValues("Sensor3", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(2).transform));

        }else if (i == 3)
        {
            for (int j = 0; j < HolidayInnSensorButtons.Length; j++)
            {
                HolidayInnSensorButtons[j].gameObject.SetActive(true);
                HolidayInnSensorButtons[j].interactable = true;
            }
            HolidayInnSensorButtons[0].onClick.AddListener(() => FogSensorValues("Sensor1", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(0).transform));
            HolidayInnSensorButtons[1].onClick.AddListener(() => FogSensorValues("Sensor2", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(1).transform));
            HolidayInnSensorButtons[2].onClick.AddListener(() => FogSensorValues("Sensor3", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(2).transform));

        }else if (i == 4)
        {
            for (int j = 0; j < MercureSensorButtons.Length; j++)
            {
                MercureSensorButtons[j].gameObject.SetActive(true);
                MercureSensorButtons[j].interactable = true;
            }
            MercureSensorButtons[0].onClick.AddListener(() => FogSensorValues("Sensor1", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(0).transform));
            MercureSensorButtons[1].onClick.AddListener(() => FogSensorValues("Sensor2", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(1).transform));
            MercureSensorButtons[2].onClick.AddListener(() => FogSensorValues("Sensor3", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(2).transform));

        }else if (i == 5)
        {
            for (int j = 0; j < SheratonSensorButtons.Length; j++)
            {
                SheratonSensorButtons[j].gameObject.SetActive(true);
                SheratonSensorButtons[j].interactable = true;
            }
            SheratonSensorButtons[0].onClick.AddListener(() => FogSensorValues("Sensor1", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(0).transform));
            SheratonSensorButtons[1].onClick.AddListener(() => FogSensorValues("Sensor2", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(1).transform));
            SheratonSensorButtons[2].onClick.AddListener(() => FogSensorValues("Sensor3", BuildingsSensorPositions.transform.GetChild(i).transform.GetChild(2).transform));

        }
      


    }

    void FogSensorValues(string SensorId,Transform Target)
    {
        SensorName.text = SensorId;

        GameManager.Instance.CameraController.MoveCameraToCertainPoint(Target, false, 800);
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Target.GetComponent<FogSensorValues>().LevelVal.ToString()+"%";
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Target.GetComponent<FogSensorValues>().DaysLeftVal.ToString();
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Target.GetComponent<FogSensorValues>().BatteryLifeVal.ToString()+"%";
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Target.GetComponent<FogSensorValues>().TapVal.ToString()+"%";
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = Target.GetComponent<FogSensorValues>().TotalSolidsVal.ToString()+"%";
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = Target.GetComponent<FogSensorValues>().BottomVal.ToString()+"%";

    }
    #region Districts
    public void Highlight_Districts(GameObject district)
    {
        GameManager.Instance.GridSystem.ChangeDistrict(district.GetComponent<DistrictButton>().districtData.name);
    }
    public void ChangeDistrictNameInAdress(string currentDistrictName)
    {
        districtName.text = currentDistrictName;
        _buildingInfo.DistrictName.text = currentDistrictName;
        districtName.GetComponent<Button>().interactable = true;
        HierarchyImages[0].sprite = HierarchyImagesOn[0];
        HierarchyImages[1].sprite = HierarchyImagesOff[1];
        HierarchyImages[2].sprite = HierarchyImagesOff[2];
    }
    public void DistrictNavigates() {
        DistrictHolder.transform.GetChild(DistrictIndex - 1).GetComponent<DistrictButton>().GoToPoint();
        Debug.Log("DistrictIndex " + DistrictIndex);
        BuildingsInfoCanvasNew.SetActive(false);
        buildingName.gameObject.GetComponent<Button>().interactable = false;
        SensorName.GetComponent<Button>().interactable = false;
        buildingName.text = "Site";
        ResetAllSensorButtons();
        BuildingsInfoCanvasNew.SetActive(false);
        HierarchyImages[0].sprite = HierarchyImagesOn[0];
        HierarchyImages[1].sprite = HierarchyImagesOff[1];
        HierarchyImages[2].sprite = HierarchyImagesOff[2];
    }

    public void BuildingtNavigates() {
        buildingName.gameObject.GetComponent<Button>().onClick.AddListener(() => Highlight_Buildings(buildingName.text));

        
        BuildingsInfoCanvasNew.SetActive(true);
        SensorName.GetComponent<Button>().interactable = false;

        ResetAllSensorButtons();
        HierarchyImages[0].sprite = HierarchyImagesOn[0];
        HierarchyImages[1].sprite = HierarchyImagesOn[1];
        HierarchyImages[2].sprite = HierarchyImagesOff[2];
    }
    public void ResetAllSensorButtons()
    {
        SensorName.text = "Sensor";
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        BuildingsInfoCanvasNew.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        for (int j = 0; j < BurjKhalifaSensorButtons.Length; j++)
        {
            BurjKhalifaSensorButtons[j].gameObject.SetActive(false);            
            BurjKhalifaSensorButtons[j].interactable = false;
        }

        HierarchyImages[0].sprite = HierarchyImagesOff[0];
        HierarchyImages[1].sprite = HierarchyImagesOff[1];
        HierarchyImages[2].sprite = HierarchyImagesOff[2];
    }
    #endregion
    public void ExitGame()
    {
        Application.Quit();
    }
    //private void OnDisable() {

    //        for (int i = 0; i < MainSpotsCanvasNew.transform.childCount; i++) {
    //            foreach (Transform T1 in MainSpotsCanvasNew.transform.GetChild(i).transform.GetChild(0).transform) {
    //            if (T1.gameObject.GetComponent<Button>() != null) T1.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    //            }
    //        }


    //}

    public void SidePanelToggle()
    {
        SidePanelTogglebool = !SidePanelTogglebool;
        if (SidePanelTogglebool)
        {
            SidePanelImg.sprite = SidePanelToggleSprites[1];
        }
        else
        {
            SidePanelImg.sprite = SidePanelToggleSprites[0];

        }
    }
}

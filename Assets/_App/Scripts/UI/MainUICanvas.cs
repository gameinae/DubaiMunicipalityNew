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
                AssignBuildingName(buildingName);
                break;
            case "PalmHotel":
                AssignBuildingPoint(1);
                AssignBuildingName(buildingName);
                break;
            case "AddressSkyView":
                AssignBuildingPoint(2);
                AssignBuildingName(buildingName);
                break;
            case "HolidayInnHotel":
                AssignBuildingPoint(3);
                AssignBuildingName(buildingName);
                break;
            case "Mercure":
                AssignBuildingPoint(4);
                AssignBuildingName(buildingName);
                break;
            case "Sheraton":
                AssignBuildingPoint(5);
                AssignBuildingName(buildingName);
                break;
            case "Kempinski":
                AssignBuildingPoint(6);
                AssignBuildingName(buildingName);
                break;
            case "DusitD2KenzHotel":
                AssignBuildingPoint(7);
                AssignBuildingName(buildingName);
                break;
            case "MediaByRotana":
                AssignBuildingPoint(8);
                AssignBuildingName(buildingName);
                break;
            case "IBIS&NovotelHotel":
                AssignBuildingPoint(9);
                AssignBuildingName(buildingName);
                break;
            case "NaraCafe":
                AssignBuildingPoint(10);
                AssignBuildingName(buildingName);
                break;
            case "LeMeridien":
                AssignBuildingPoint(11);
                AssignBuildingName(buildingName);
                break;
            case "WyndhamDubaiMarina":
                AssignBuildingPoint(12);
                AssignBuildingName(buildingName);
                break;
            case "MazahRestaurant":
                AssignBuildingPoint(13);
                AssignBuildingName(buildingName);
                break;
            case "FirstCentralHotelSuites":
                AssignBuildingPoint(14);
                AssignBuildingName(buildingName);
                break;
            case "ClassHotelApartments":
                AssignBuildingPoint(15);
                AssignBuildingName(buildingName);
                break;
            case "GrandCosmopolitanHotel":
                AssignBuildingPoint(16);
                AssignBuildingName(buildingName);
                break;
            case "HiltonDubaiAlHabtoorCity":
                AssignBuildingPoint(17);
                AssignBuildingName(buildingName);
                break;
            case "MajesticCityRetreatHotel":
                AssignBuildingPoint(18);
                AssignBuildingName(buildingName);
                break;
            case "CarltonPalaceHotel":
                AssignBuildingPoint(19);
                AssignBuildingName(buildingName);
                break;
            case "AlNaseemHotelMadinatJumeirah":
                AssignBuildingPoint(20);
                AssignBuildingName(buildingName);
                break;
            case "AvenueHotel":
                AssignBuildingPoint(21);
                AssignBuildingName(buildingName);
                break;
            case "BroadwayHotel":
                AssignBuildingPoint(22);
                AssignBuildingName(buildingName);
                break;
            case "BurjumanArjaanByRotanaHote":
                AssignBuildingPoint(23);
                AssignBuildingName(buildingName);
                break;
            case "CarltonTowerHotel":
                AssignBuildingPoint(24);
                AssignBuildingName(buildingName);
                break;
            case "CenterMirageBeachResort":
                AssignBuildingPoint(25);
                AssignBuildingName(buildingName);
                break;
            case "CitySeasonHotel":
                AssignBuildingPoint(26);
                AssignBuildingName(buildingName);
                break;
            case "ComfortinnHotel":
                AssignBuildingPoint(27);
                AssignBuildingName(buildingName);
                break;
            case "DanmacmaisoncanalviwesHotel":
                AssignBuildingPoint(28);
                AssignBuildingName(buildingName);
                break;
            case "HUESBoutiqueHotel":
                AssignBuildingPoint(29);
                AssignBuildingName(buildingName);
                break;
            case "HyatCreekHotel":
                AssignBuildingPoint(30);
                AssignBuildingName(buildingName);
                break;
            case "HyattRegengy":
                AssignBuildingPoint(31);
                AssignBuildingName(buildingName);
                break;
            case "IbisHotelAlRigga":
                AssignBuildingPoint(32);
                AssignBuildingName(buildingName);
                break;
            case "ManhattanAvenueHotel":
                AssignBuildingPoint(33);
                AssignBuildingName(buildingName);
                break;
            case "MillenniumPlace":
                AssignBuildingPoint(34);
                AssignBuildingName(buildingName);
                break;
            case "MJMashMinaAlSalamHotel":
                AssignBuildingPoint(35);
                AssignBuildingName(buildingName);
                break;
            case "MjAlQasrHotelMadinatjumeirah":
                AssignBuildingPoint(36);
                AssignBuildingName(buildingName);
                break;
            case "NIHALHOTEL":
                AssignBuildingPoint(37);
                AssignBuildingName(buildingName);
                break;
            case "OrchidHotel":
                AssignBuildingPoint(38);
                AssignBuildingName(buildingName);
                break;
            case "OrchidVoeHotel":
                AssignBuildingPoint(39);
                AssignBuildingName(buildingName);
                break;
            case "PalmBeachHotel":
                AssignBuildingPoint(40);
                AssignBuildingName(buildingName);
                break;
            case "PanoramaHotelDeira":
                AssignBuildingPoint(41);
                AssignBuildingName(buildingName);
                break;
            case "ParamountHotelDubai":
                AssignBuildingPoint(42);
                AssignBuildingName(buildingName);
                break;
            case "RadissonBlueHotelDubaiCreekHotel":
                AssignBuildingPoint(43);
                AssignBuildingName(buildingName);
                break;
            case "RevierHotel":
                AssignBuildingPoint(44);
                AssignBuildingName(buildingName);
                break;
            case "RollaResidenceHotelApartment":
                AssignBuildingPoint(45);
                AssignBuildingName(buildingName);
                break;
            case "RoveHotel":
                AssignBuildingPoint(46);
                AssignBuildingName(buildingName);
                break;
            case "RoyalContinentalHotel":
                AssignBuildingPoint(47);
                AssignBuildingName(buildingName);
                break;
            case "SavoyCentralHotel":
                AssignBuildingPoint(48);
                AssignBuildingName(buildingName);
                break;
            case "Signatureinnhotel":
                AssignBuildingPoint(49);
                AssignBuildingName(buildingName);
                break;
            case "SLSHotel":
                AssignBuildingPoint(50);
                AssignBuildingName(buildingName);
                break;
            case "SunsandsPlazaHotel":
                AssignBuildingPoint(51);
                AssignBuildingName(buildingName);
                break;
            case "TheOberoiHotel":
                AssignBuildingPoint(52);
                AssignBuildingName(buildingName);
                break;
            case "WhiteFortHotel":
                AssignBuildingPoint(53);
                AssignBuildingName(buildingName);
                break;
            case "LaperGolaMazah":
                AssignBuildingPoint(54);
                AssignBuildingName(buildingName);
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
    public void AssignBuildingName(string _buildingName)
    {
        GameManager.Instance.GridSystem.SetBuildingName(_buildingName);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistrictButton : MonoBehaviour
{
    public Transform point;
    public float travelSpeed;
    public Animator animator;
    public int districtIndex;
    public DistrictSO districtData;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangeZoomDistrict);
        if (!districtData)
        {
            button.interactable = false;
        }
    }
    private void OnEnable()
    {
        Button button = GetComponent<Button>();
        if (!districtData)
        {
            button.interactable = false;
        }
    }
    private void Update()
    {
        if (animator != null)
        {
            if (Input.GetKeyUp(KeyCode.Space) && !animator.GetBool("FadeIn") && !GameManager.Instance.GridSystem.reset)
            {
                animator.SetBool("FadeIn", true);
            }
        }

    }
    public void GoToPoint()
    {
        Debug.Log("clkd");
        MainUICanvas.instance.zoomEnabled = true;
        ToogleHighliting.instance.EnableSubDistrictHigligting();
        GameManager.Instance.GridSystem.ChangeBuildingMaterialToOriginal();
        GameManager.Instance.CameraController.isZoom = true;
        MainUICanvas.instance.DistrictIndex = districtIndex;
        GameManager.Instance.GridSystem.reset = true;
        GameManager.Instance.CameraController.MoveCameraToCertainPoint(point, false, travelSpeed);
        GameManager.Instance.CameraController.currentZoomOutTarget = point;
        foreach (Transform T in GameManager.Instance.UIManager.mainUICanvas.DistrictsCanvasNew.transform.GetChild(0).transform)
        {
            T.gameObject.GetComponent<Image>().enabled = false;
            if (T.gameObject.transform.GetChild(0).gameObject != null) T.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        }
        StartCoroutine(Enablespots());
    }
    private void ChangeZoomDistrict()
    {
        var mainCanvas = GameManager.Instance.UIManager.mainUICanvas;
        if (mainCanvas.ZoomIn != null) mainCanvas.ZoomIn.GetComponent<ZoomButton>().DistrictIndex = districtIndex;
        if (mainCanvas.ZoomOut != null) mainCanvas.ZoomOut.GetComponent<ZoomButton>().DistrictIndex = districtIndex;
        //mainCanvas.unityAction.Invoke(districtIndex);
    }
    IEnumerator Enablespots()
    {

        yield return new WaitForSeconds(2f);
        if (!GameManager.Instance.CameraController.isZoomIn)
        {
            foreach (Transform T in GameManager.Instance.UIManager.mainUICanvas.MainSpotsCanvasNew.transform)
            {
                T?.gameObject.SetActive(false);
            }
            GameManager.Instance.UIManager.mainUICanvas.MainSpotsCanvasNew.transform.GetChild(districtIndex - 1)?.gameObject.SetActive(true);
        }
        GameManager.Instance.GridSystem.reset = false;

    }
}

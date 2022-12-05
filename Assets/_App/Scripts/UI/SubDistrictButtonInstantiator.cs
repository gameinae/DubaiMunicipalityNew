using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubDistrictButtonInstantiator : MonoBehaviour
{
    public GameObject subDistrictButton;
    public List<Collider> subDistricts;
    public List<Transform> cameraPositions;
    public GameObject cameraParent;
    public GameObject districtButtonsParent;
    public float travelSpeed;
    private void Awake()
    {
        if (this.enabled)
        {
            subDistricts = this.GetComponentsInChildren<Collider>().ToList();
            cameraPositions = cameraParent.GetComponentsInChildren<Transform>().ToList();
            cameraPositions.RemoveAt(0);

            for (int i = 0; i < subDistricts.Count; i++)
            {
                var sbgo = Instantiate(subDistrictButton, subDistricts[i].transform);
                var sbButton = sbgo.transform.GetChild(0).GetComponent<Button>();
                var cp = cameraPositions[i];

                if (sbButton)
                {
                    sbButton.onClick.AddListener(() =>
                    GoToSubDistrictPosition(cp));
                }

            }

        }
    }
    public void GoToSubDistrictPosition(Transform subDistrictCameraPosition)
    {
        Debug.Log("subDistrict btn pressed");
        ToogleHighliting.instance.EnableBuildingsUIInvoker();
        GameManager.Instance.CameraController.MoveCameraToCertainPoint(subDistrictCameraPosition, false, travelSpeed);
    }
}

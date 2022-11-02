using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private void Update()
    {
        bool reset = GameManager.Instance.GridSystem.reset;

        if (Input.GetKey(KeyCode.Return))
        {
            GameManager.Instance.GridSystem.FilterBuildings();
        }
        if (Input.GetKey(KeyCode.Alpha0) && !reset)
        {
            GameManager.Instance.GridSystem.GoToOrigin();
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            var mediumHelathSelectedBuilding = GameManager.Instance.GridSystem.mediumHelathSelectedBuilding;
            GameManager.Instance.GridSystem.ChangeBuildingMaterialToSelected(0, mediumHelathSelectedBuilding, true);
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            var badHelathSelectedBuilding = GameManager.Instance.GridSystem.badHelathSelectedBuilding;
            GameManager.Instance.GridSystem.ChangeBuildingMaterialToSelected(0, badHelathSelectedBuilding, true);
        }

        //if (Input.mouseScrollDelta.y > 0 && GameManager.Instance.CameraController.isZoom)
        //{
        //    GameManager.Instance.CameraController.ZoomInAtCursorPosition();
        //}
        //if (Input.mouseScrollDelta.y < 0 && GameManager.Instance.CameraController.isZoom)
        //{
        //    GameManager.Instance.CameraController.ZoomOutCursorPosition();
        //}


    }
}

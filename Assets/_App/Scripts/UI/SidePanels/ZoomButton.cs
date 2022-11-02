using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomButton : MonoBehaviour
{
    public enum ZoomType { ZoomIn, ZoomOut }
    public ZoomType zoomType;
    public int DistrictIndex;

    Button button;

    //Cached
    [Header("Main Panel Alternatives")]
    public List<Transform> MainPanels;
    [Header("First Panel Alternatives")]
    public List<Transform> FirstPanels;
    [Header("Second Panel Alternatives")]
    public List<Transform> SecondPanels;

    private void Awake()
    {
        button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(Zoom);
    }

    public void Zoom()
    {
        Camera camera = Camera.main;
        if (camera != null)
        {
            var fov = camera.fieldOfView;
            camera.fieldOfView = zoomType == ZoomType.ZoomOut ? fov + 10 : fov - 10;
            fov = camera.fieldOfView;
            camera.fieldOfView = Mathf.Clamp(fov, 45, 65);
            ChangePanel((int)fov);
        }
    }
    public void ChangePanel(int fov)
    {
        switch (DistrictIndex)
        {
            case 0:
                ChangeMainPanel(fov);
                break;
            case 1:
                ChangSecondPanel(fov);
                break;
            case 2:
                ChangeFirstPanel(fov);
                break;
            case 3:
                ChangeThirdPanel(fov);
                break;
            default:
                break;
        }
    }
    private void ChangeThirdPanel(int fov)
    {
        switch (fov)
        {
            case 45:
                break;
            case 55:
                break;
            case 65:
                break;
            default:
                break;
        }
    }
    public void ChangeFirstPanel(int fov)
    {
        switch (fov)
        {
            case 45:
                FirstPanels.ForEach(p => p.gameObject.SetActive(false));
                FirstPanels[1].gameObject.SetActive(true);
                break;
            case 55:
                FirstPanels.ForEach(p => p.gameObject.SetActive(false));
                FirstPanels[2].gameObject.SetActive(true);
                break;
            case 65:
                FirstPanels.ForEach(p => p.gameObject.SetActive(false));
                FirstPanels[0].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void ChangSecondPanel(int fov)
    {
        switch (fov)
        {
            case 45:
                SecondPanels.ForEach(p => p.gameObject.SetActive(false));
                SecondPanels[1].gameObject.SetActive(true);
                break;
            case 55:
                SecondPanels.ForEach(p => p.gameObject.SetActive(false));
                SecondPanels[2].gameObject.SetActive(true);
                break;
            case 65:
                SecondPanels.ForEach(p => p.gameObject.SetActive(false));
                SecondPanels[0].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void ChangeMainPanel(int fov)
    {
        switch (fov)
        {
            case 45:
                break;
            case 55:
                break;
            case 65:
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private CameraController cameraController;
    public CameraController CameraController
    {
        get { return cameraController; }
    }

    [SerializeField]
    private UIManager uiManager;
    public UIManager UIManager
    {
        get { return uiManager; }
    }

    [SerializeField]
    private GridSystem gridSystem;
    public GridSystem GridSystem
    {
        get => gridSystem; 
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;

    }

    public bool Init()
    {
        var cc = GameObject.FindObjectOfType<CameraController>();
        var um = GameObject.FindObjectOfType<UIManager>();
        var gs = GameObject.FindObjectOfType<GridSystem>();

        if (!cameraController && cc)
        {
            cameraController = cc;
        }
        if (!uiManager && um) 
        {
            uiManager = um;
        }
        if (!gridSystem && gs) 
        {
            gridSystem = gs;
        }
        return uiManager && cameraController && gridSystem;
    }
   
}

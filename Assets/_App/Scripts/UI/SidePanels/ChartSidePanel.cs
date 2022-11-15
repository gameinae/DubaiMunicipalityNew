using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: Change Switching Between Charts

public class ChartSidePanel : MonoBehaviour,IChartToggle
{
    Button myButton;

    //cached
    public Transform MyPanelParent;
    public Button ClosePanelBtn;
    public Transform HoverPanel;
    public PointerDistrictHighlighter[] districtHighliter;
    public DistrictButton[] districtButton;
    private void Start()
    {
        districtHighliter = FindObjectsOfType(typeof(PointerDistrictHighlighter)) as PointerDistrictHighlighter[];
        districtButton = FindObjectsOfType(typeof(DistrictButton)) as DistrictButton[];
        myButton = gameObject.AddComponent<Button>();
        myButton.onClick.AddListener(OnClick);
        myButton.onClick.AddListener(DisableEnvironmentInteraction);
        ClosePanelBtn?.onClick.AddListener(ClosePanel);
        ClosePanelBtn?.onClick.AddListener(EnableEnvironmentInteraction);

    }
    public void OnMouseEnter()
    {
        Debug.Log("MouseHovered");
    }
    public void OnClick() 
    {
        OpenPanel();
    }
    public void OpenPanel()
    {
        MainUICanvas.instance.LeftPanel.GetComponent<SidePanel>().ClosePanelsAnim();
        MainUICanvas.instance.RightPanel.GetComponent<SidePanel>().ClosePanelsAnim();
        PlayAnimation("close", false);
        PlayAnimation();
    }
    public void EnableEnvironmentInteraction()
    {
        for (int i = 0; i < districtHighliter.Length; i++)
        {
            districtHighliter[i].GetComponent<PointerDistrictHighlighter>().enabled = true;
        }
        for (int i = 0; i < districtButton.Length; i++)
        {
            districtButton[i].gameObject.SetActive(true);
        }
    }
    public void DisableEnvironmentInteraction()
    {
        for (int i = 0; i < districtHighliter.Length; i++)
        {
            districtHighliter[i].GetComponent<PointerDistrictHighlighter>().enabled = false;
            districtHighliter[i].GetComponent<PointerDistrictHighlighter>().DisableEmissionMaterial();
        }
        for (int i = 0; i < districtButton.Length; i++)
        {
            districtButton[i].gameObject.SetActive(false);
        }
    }
    public void ClosePanel()
    {
        PlayAnimation("close");
        PlayAnimation("open",false);
        float animationLength = GetAnimaitonLenght();        
    }
    void PlayAnimation(string flag = "open", bool isEnabled = true) 
    {
        var anim = MyPanelParent.GetComponent<Animator>();
        anim.SetBool(flag, isEnabled);
    }
    float GetAnimaitonLenght()
    {
        var anim = MyPanelParent.GetComponent<Animator>();
        return anim.GetCurrentAnimatorClipInfo(0).Length;
    }
}

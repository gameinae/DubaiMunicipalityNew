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

    private void Start()
    {
        myButton = gameObject.AddComponent<Button>();
        myButton.onClick.AddListener(OnClick);
        ClosePanelBtn?.onClick.AddListener(ClosePanel);
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

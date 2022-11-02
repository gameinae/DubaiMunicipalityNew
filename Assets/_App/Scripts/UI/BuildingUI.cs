using TMPro;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public TextMeshProUGUI buildingName;

    CanvasGroup canvasGroup;
    Camera myCam;
    Animator animator;

    private void Awake()
    {
        myCam = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        buildingName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowCanvas()
    {
        canvasGroup.interactable = true;
        canvasGroup.ignoreParentGroups = true;
        animator.SetBool("FadeIn", true);
    }
    public void HideCanvas()
    {
        canvasGroup.interactable = false;
        canvasGroup.ignoreParentGroups = false;
        animator.SetBool("FadeIn", false);
    }

}

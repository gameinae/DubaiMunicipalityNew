using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class ListOfObjects{
    public List<GameObject> objectToActivate;
    public List<GameObject> objectToDeactivate;
}
public class ActivatorAndDeactivatorTabs : MonoBehaviour
{
    public List<Button> tabsButtons;
    ListOfObjects objects = new ListOfObjects();
    private void Start()
    {
        for (int i = 0; i < tabsButtons.Count; i++)
        {
            tabsButtons[i].onClick.AddListener(TabActivaction);
        }
    }

    public void TabActivaction()
    {
        for (int i = 0; i < objects.objectToActivate.Count; i++)
        {
            objects.objectToActivate[i].gameObject.SetActive(true);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMenus : MonoBehaviour
{
    [SerializeField] List<GameObject> menus;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject underline;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(DeactivateAllActivateOne);
    }
    public void DeactivateAllActivateOne()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].gameObject.SetActive(false);
        }
        menu.SetActive(true);
        underline.transform.position = new Vector3(this.transform.position.x, underline.transform.position.y, underline.transform.position.y);
    }
}

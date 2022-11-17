using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextColorChanger : MonoBehaviour
{
    [SerializeField] List<Button> texts;

    private void Start()
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            int keyIndex = i + 1;
            if (texts[i] != null)
                texts[i].onClick.AddListener(() => ChangeTextColor(keyIndex));
        }
    }

    public void ChangeTextColor(int index)
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].GetComponent<Text>().color = Color.cyan;
        }
        texts[index - 1].GetComponent<Text>().color = Color.black;
    }
}

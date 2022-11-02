using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseCasesHolder : MonoBehaviour
{
    List<UseCase> useCasesOptions = new List<UseCase>();

    private void Awake()
    {
        foreach (Transform T in transform)
        {
            if (T.tag == "UseCaseOption")
            {
                var btn = T.gameObject.GetComponent<Button>() ? T.gameObject.GetComponent<Button>() : T.gameObject.AddComponent<Button>();
                var useCase = T.gameObject.AddComponent<UseCase>() ? T.gameObject.AddComponent<UseCase>() : T.gameObject.AddComponent<UseCase>();
                btn?.onClick.AddListener(OnOptionClick);
                useCasesOptions.Add(useCase);
            }
        }
    }

    void OnOptionClick() 
    {
        useCasesOptions.ForEach(u => u.DeactivateOption());
    }
}

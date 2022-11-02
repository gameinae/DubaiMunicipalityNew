using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManagerScript : MonoBehaviour
{
    public GameObject loadingSprite;
    public bool isLoadingActive;
    public InputField username, password;
    public Button login;

    private void Update()
    {

        login.interactable = isLoadingActive;

        if (username.text != "" && password.text != "")
        {
            isLoadingActive = true;
        }
        else
        {
            isLoadingActive = false;
        }
        if (loadingSprite != null && loadingSprite.activeInHierarchy && !isLoadingActive)
        {
            loadingSprite.SetActive(false);
        }
    }
    public void LoadMapScene()
    {
        if (isLoadingActive)
            StartCoroutine(LoadScene("GameScene"));
    }
    IEnumerator LoadScene(int sceneIndex)
    {
        yield return new WaitForSeconds(sceneIndex);
        SceneManager.LoadScene(sceneIndex);
        yield return new WaitForEndOfFrame();
    }

    [System.Obsolete]
    IEnumerator LoadScene(string sceneName)
    {
        if (SceneManager.GetAllScenes().ToList().FirstOrDefault(s => s.name == sceneName) == null) 
        {
            StartCoroutine(LoadScene("MapScene"));
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(1);
            yield return new WaitForEndOfFrame();
        }
    }
}

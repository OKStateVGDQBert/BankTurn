using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_Buttons : MonoBehaviour {

    public GameObject QuitPanel;

    public void EnableQuitPanel()
    {
        QuitPanel.SetActive(true);
    }

    public void QuitFromQuitPanel()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void MenuFromQuitPanel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
    }

    public void BackFromQuitPanel()
    {
        QuitPanel.SetActive(false);
    }
}

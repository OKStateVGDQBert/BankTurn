using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_functions : MonoBehaviour {

    [SerializeField]
    GameObject StartPanel;
    [SerializeField]
    GameObject ChoosePanel;

    public void StartButtonFunc()
    {
        StartPanel.SetActive(false);
        ChoosePanel.SetActive(true);
    }

    public void ExitButtonFunc()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void BackButtonFunc()
    {
        StartPanel.SetActive(true);
        ChoosePanel.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class button_functions : MonoBehaviour {

    [SerializeField]
    GameObject StartPanel;
    [SerializeField]
    GameObject ChoosePanel;
    [SerializeField]
    GameObject LoadGamePanel;

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

	public void EarthButtonFunc()
    {
        StartPanel.SetActive(false);
        ChoosePanel.SetActive(false);
        LoadGamePanel.SetActive(true);
        Data_Manager.underPlayerControl = false;
        Data_Manager.inMenu = false;
        SceneManager.LoadScene("Earth");
    }

    public void MoonButtonFunc()
    {
        StartPanel.SetActive(false);
        ChoosePanel.SetActive(false);
        LoadGamePanel.SetActive(true);
        Data_Manager.underPlayerControl = false;
        Data_Manager.inMenu = false;
        SceneManager.LoadScene("Moon");
    }

    public void VenusButtonFunc()
    {
        StartPanel.SetActive(false);
        ChoosePanel.SetActive(false);
        LoadGamePanel.SetActive(true);
        Data_Manager.underPlayerControl = false;
        Data_Manager.inMenu = false;
        SceneManager.LoadScene("Venus");
    }

    public void DifficultySliderFunc()
    {
        Data_Manager.difficulty = (int)(gameObject.GetComponent(typeof(Slider)) as Slider).value;
    }

    public void ShipSelectorFunc()
    {
        Data_Manager.shipType = (gameObject.GetComponent(typeof(Dropdown)) as Dropdown).value;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Toggle[] _groupOfToggle;
    [SerializeField] private GameObject[] _contentGroup;
    [SerializeField] private GameObject _mainMenu;

    private bool _gamePaused;

    public void ContentChanger(int ToggleIndex)
    {
        for (int i = 0; i < _groupOfToggle.Length; i++)
        {
            if (ToggleIndex == i)
            {
                _groupOfToggle[i].interactable = false;
                _contentGroup[i].SetActive(true);
            }
            else
            {
                _groupOfToggle[i].interactable = true;
                _contentGroup[i].SetActive(false);
            }
        }
    }
    public void MainMenuOn()
    {
        _mainMenu.SetActive(true);
    }
    public void MainMenuOff()
    {
        _mainMenu.SetActive(false);
    }
    public void GamePoused()
    {
        _gamePaused = !_gamePaused;
        if (_gamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void ButtonPress()
    {
        Debug.Log("Button");
    }
    public void TogglePress()
    {
        Debug.Log("Toggle");
    }
    



}

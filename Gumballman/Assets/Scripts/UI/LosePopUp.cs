using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePopUp : MonoBehaviour
{
    [SerializeField] private Text _scoreValue;
    void Start()
    {
        
    }
    public void SceneLoader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}

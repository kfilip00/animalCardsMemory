using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNextLevel : MonoBehaviour
{
    [SerializeField] private Button yes;
    [SerializeField] private Button no;
    
    private void Start()
    {
        if (GameControl.Difficulty==2)
        {
            ResetProgress();
        }
    }

    private void OnEnable()
    {
        yes.onClick.AddListener(PlayNextLevel);
        no.onClick.AddListener(ResetProgress);
    }

    private void OnDestroy()
    {
        yes.onClick.RemoveListener(PlayNextLevel);
        no.onClick.RemoveListener(ResetProgress);
    }

    private void PlayNextLevel()
    {
        GameControl.Difficulty++;
        RefreshScene();
    }
    

    private void ResetProgress()
    {
        GameControl.Difficulty = -1;
        RefreshScene();
    }

    private void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

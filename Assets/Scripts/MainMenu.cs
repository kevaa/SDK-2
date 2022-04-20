using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI sliderText;

    public event Action<float> OnSliderChanged = delegate { };

    private void Start()
    {
        PlayerPrefs.SetInt("enemyCount", (int)slider.value);
        sliderText.text = "Bot Count: " + (int)slider.value;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateBotCountValue()
    {
        PlayerPrefs.SetInt("enemyCount", (int)slider.value);
        sliderText.text = "Bot Count: " + (int)slider.value;
    }
}

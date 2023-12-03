using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private AudioMixer PrimaryMixer;   // Audio Mixer
    [SerializeField] private Slider BGMSlider;          // Background Music Slider
    [SerializeField] private Slider SFXSlider;          // Sound Effect Slider

    // Constaant Variables
    private const float STANDARD_LOG = 20f;

    private void Awake()
    {
        // Initialize the slider and set its listeners
        BGMSlider.onValueChanged.AddListener(ChangeBGM);
        BGMSlider.value = GetSavedBGM();

        SFXSlider.onValueChanged.AddListener(ChangeSFX);
        SFXSlider.value = GetSavedSFX();
    }

    private void ChangeBGM(float volume)
    {
        // Calculate Log Standard for Volume and set and save it
        float logVolume = Mathf.Log10(volume) * STANDARD_LOG;

        PrimaryMixer.SetFloat("BGMVol", logVolume);
        SaveBGM(volume);
    }

    private float GetSavedBGM()
    {
        return PlayerPrefs.GetFloat("SavedBGM", 0.5f);
    }

    private void SaveBGM(float volume)
    {
        PlayerPrefs.SetFloat("SavedBGM", volume);
        PlayerPrefs.Save();
    }

    private void ChangeSFX(float volume)
    {
        // Calculate Log Standard for Volume and set and save it
        float logVolume = Mathf.Log10(volume) * STANDARD_LOG;

        PrimaryMixer.SetFloat("SFXVol", logVolume);
        SaveSFX(volume);
    }

    private float GetSavedSFX()
    {
        return PlayerPrefs.GetFloat("SavedSFX", 0.5f);
    }

    private void SaveSFX(float volume)
    {
        PlayerPrefs.SetFloat("SavedSFX", volume);
        PlayerPrefs.Save();
    }

    // Close Setting
    public void ActionClose()
    {
        gameObject.SetActive(false);
    }
}
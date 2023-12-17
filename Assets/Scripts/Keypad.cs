using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad { 
public class Keypad : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onAccessGranted;
    [SerializeField] private UnityEvent onAccessDenied;
    [Header("Combination Code (4 Numbers Max)")]
    [SerializeField] private int keypadCombo = 0326;

    public UnityEvent OnAccessGranted => onAccessGranted;
    public UnityEvent OnAccessDenied => onAccessDenied;

    [Header("Settings")]
    [SerializeField] private string accessGrantedText = "Granted";
    [SerializeField] private string accessDeniedText = "Denied";

    [Header("Visuals")]
    [SerializeField] private float displayResultTime = 1f;
    [Range(0,5)]
    [SerializeField] private float screenIntensity = 2.5f;

    [Header("Colors")] [SerializeField] private Color screenNormalColor;
    [SerializeField] private Color screenDeniedColor;
    [SerializeField] private Color screenGrantedColor;
    [Header("SoundFx")]
    [SerializeField] private AudioClip buttonClickedSfx;
    [SerializeField] private AudioClip accessDeniedSfx;
    [SerializeField] private AudioClip accessGrantedSfx;
    [Header("Component References")]
    [SerializeField] private Renderer panelMesh;
    [SerializeField] private TMP_Text keypadDisplayText;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Highlight doorHighlight;

    private string currentInput;
    private bool displayingResult;
    private bool accessWasGranted;

    private void Start()
    {
        doorHighlight.SetHighlight(false);
    }

    private void Awake()
    {
        ClearInput();
        panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
    }
    
    public void AddInput(string input)
    {
        audioSource.PlayOneShot(buttonClickedSfx);
        if (displayingResult || accessWasGranted) return;
        switch (input)
        {
            case "enter":
                CheckCombo();
                break;
            default:
                if (currentInput != null && currentInput.Length == 4) 
                {
                    return;
                }
                currentInput += input;
                keypadDisplayText.text = currentInput;
                break;
        }
        
    }
    public void CheckCombo()
    {
        if(int.TryParse(currentInput, out var currentKombo))
        {
            bool granted = currentKombo == keypadCombo;
            if (!displayingResult)
            {
                StartCoroutine(DisplayResultRoutine(granted));
            }

            if (granted)
            {
                doorHighlight.SetHighlight(true);
                doorHighlight.gameObject.tag = "ExitDoor";
            }
        }
        else
        {
            Debug.LogWarning("Couldn't process input for some reason..");
        }
        
    }
    
    private IEnumerator DisplayResultRoutine(bool granted)
    {
        displayingResult = true;

        if (granted) AccessGranted();
        else AccessDenied();

        yield return new WaitForSeconds(displayResultTime);
        displayingResult = false;
        if (granted) yield break;
        ClearInput();
        panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);

    }

    private void AccessDenied()
    {
        keypadDisplayText.text = accessDeniedText;
        onAccessDenied?.Invoke();
        panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
        audioSource.PlayOneShot(accessDeniedSfx);
    }

    private void ClearInput()
    {
        currentInput = "";
        keypadDisplayText.text = currentInput;
    }

    private void AccessGranted()
    {
        accessWasGranted = true;
        keypadDisplayText.text = accessGrantedText;
        onAccessGranted?.Invoke();
        panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
        audioSource.PlayOneShot(accessGrantedSfx);
    }

}
}
using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private GameObject oculusPlayer;
    [SerializeField] private GameObject mouseNKeyboardPlayer;
    
    private void Start()
    {
        switch(GameManager.Controls)
        {
            case GameManager.ControlsType.MOUSENKEYBOARD:
                oculusPlayer.SetActive(false);
                mouseNKeyboardPlayer.SetActive(true);
                break;
            case GameManager.ControlsType.OCULUS:
                oculusPlayer.SetActive(true);
                mouseNKeyboardPlayer.SetActive(false);
                break;
            case GameManager.ControlsType.OCULUSNPAD:
                oculusPlayer.SetActive(false);
                mouseNKeyboardPlayer.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

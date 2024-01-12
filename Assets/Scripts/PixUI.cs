using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixUI : MonoBehaviour
{
    [SerializeField] private Image[] blocks = new Image[4];
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button acceptButton;
    [SerializeField] private GameObject pixUIWindow;
    [SerializeField] private Pix pix;
    
    private BlocksColor[] blocksColors;
    private int highLightedIndex = 5;
    private Vector3[] scales;
    private Color cancelButtonBasicColor, acceptButtonBasicColor;

    private bool canBePressed = true;

    private void Start()
    {
        scales = new Vector3[6];
        for(var i=0; i< 4; i++)
        {
            scales[i] = blocks[i].gameObject.transform.localScale;
        }
        scales[4] = cancelButton.gameObject.transform.localScale;
        scales[5] = acceptButton.gameObject.transform.localScale;

        cancelButtonBasicColor = cancelButton.image.color;
        acceptButtonBasicColor = acceptButton.image.color;

        blocksColors = new BlocksColor[4];
        blocksColors[0] = BlocksColor.BLACK;
        blocksColors[1] = BlocksColor.RED;
        blocksColors[2] = BlocksColor.RED;
        blocksColors[3] = BlocksColor.BLACK;

        if(GameManager.Controls == GameManager.ControlsType.OCULUSNPAD)
        {
            HighlightElement(highLightedIndex);
        }
        
        for(var i=0; i<4; i++)
            ChangeBlockColor(i);
    }

    private void Update()
    {
        if(GameManager.InInteractiveUI)
        {
            if (!canBePressed) return;

            //lewo
            if(Input.GetKey(KeyCode.JoystickButton4))
            {
                canBePressed = false;
                StartCoroutine(WaitForNextPress());
                UnhighlightAllElements();
                if (highLightedIndex == 0) highLightedIndex = 5;
                else 
                {
                    highLightedIndex--;
                }
                HighlightElement(highLightedIndex);

            }
            // prawo
            else if(Input.GetKey(KeyCode.JoystickButton5))
            {
                canBePressed = false;
                StartCoroutine(WaitForNextPress());
                UnhighlightAllElements();
                highLightedIndex = (highLightedIndex + 1) % 6;
                HighlightElement(highLightedIndex);
            }
            //zatwierdz
            else if (Input.GetKey(KeyCode.JoystickButton0))
            {
                canBePressed = false;
                StartCoroutine(WaitForNextPress());
                if (highLightedIndex >= 0 && highLightedIndex < 4)
                {
                    OnBlockClick(highLightedIndex);
                }
                else if(highLightedIndex == 4)
                {
                    OnCancelButtonClick();
                }
                else if(highLightedIndex == 5)
                {
                    OnAcceptButtonClick();
                }
            }
        }
    }

    private IEnumerator WaitForNextPress()
    {
        yield return new WaitForSeconds(0.25f);
        canBePressed = true;
    }

    private void HighlightElement(int index)
    {
        if (index >= 0 && index < 4)
        {
            blocks[index].gameObject.transform.localScale = Vector3.one;
        }
        else if (index == 4)
        {
            cancelButton.gameObject.transform.localScale *= 1.2f;
            cancelButton.image.color = new Color(cancelButton.image.color.r + 0.4f, cancelButton.image.color.g, cancelButton.image.color.b, cancelButton.image.color.a);
        }
        else if (index == 5)
        {
            acceptButton.gameObject.transform.localScale *= 1.2f;
            acceptButton.image.color = new Color(acceptButton.image.color.r + 0.4f, acceptButton.image.color.g, acceptButton.image.color.b, acceptButton.image.color.a);
        }
    }

    private void UnhighlightAllElements()
    {
        for (var i = 0; i < 4; i++)
            blocks[i].gameObject.transform.localScale = scales[i];
        cancelButton.gameObject.transform.localScale = scales[4];
        acceptButton.gameObject.transform.localScale = scales[5];
        cancelButton.image.color = cancelButtonBasicColor;
        acceptButton.image.color = acceptButtonBasicColor;
    }

    private enum BlocksColor
    {
        BROWN, BLACK, RED, GRAY, LENGTH
    }
    
    public void ShowPixUIWindow()
    {
        pixUIWindow.SetActive(true);
        GameManager.InInteractiveUI = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HidePixUIWindow()
    {
        highLightedIndex = 5;
        pixUIWindow.SetActive(false);
        GameManager.InInteractiveUI = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnAcceptButtonClick()
    {
        if (IsCorrectCombination())
            pix.isUnlocked = true;
        HidePixUIWindow();
    }

    public void OnCancelButtonClick()
    {
        HidePixUIWindow();
    }

    public void OnBlockClick(int blockNumber)
    {
        blocksColors[blockNumber] = (BlocksColor)(((int)blocksColors[blockNumber] + 1) % 4);
        ChangeBlockColor(blockNumber);
    }

    private void ChangeBlockColor(int blockNumber)
    {
        switch (blocksColors[blockNumber])
        {
            case BlocksColor.BROWN:
                blocks[blockNumber].color = new Color(0.376f, 0.129f, 0.078f);
                break;
            case BlocksColor.BLACK:
                blocks[blockNumber].color = new Color(0.01f, 0.01f, 0.01f);
                break;
            case BlocksColor.RED:
                blocks[blockNumber].color = new Color(0.8f, 0f, 0f);
                break;
            case BlocksColor.GRAY:
                blocks[blockNumber].color = new Color(0.5f, 0.5f, 0.5f);
                break;
        }
    }

    private bool IsCorrectCombination()
    {
        if (blocksColors[0] != BlocksColor.BROWN) return false;
        if (blocksColors[1] != BlocksColor.RED) return false;
        if (blocksColors[2] != BlocksColor.BLACK) return false;
        if (blocksColors[3] != BlocksColor.GRAY) return false;
        return true;
    }
}

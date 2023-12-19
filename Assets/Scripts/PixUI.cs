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

    private void Start()
    {
        blocksColors = new BlocksColor[4];
        blocksColors[0] = BlocksColor.BLACK;
        blocksColors[1] = BlocksColor.RED;
        blocksColors[2] = BlocksColor.RED;
        blocksColors[3] = BlocksColor.BLACK;
        
        for(var i=0; i<4; i++)
            ChangeBlockColor(i);
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
                blocks[blockNumber].color = new Color(0.376f, 0.251f, 0.122f);
                break;
            case BlocksColor.BLACK:
                blocks[blockNumber].color = new Color(0.01f, 0.01f, 0.01f);
                break;
            case BlocksColor.RED:
                blocks[blockNumber].color = new Color(0.6f, 0f, 0f);
                break;
            case BlocksColor.GRAY:
                blocks[blockNumber].color = new Color(0.3f, 0.3f, 0.3f);
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

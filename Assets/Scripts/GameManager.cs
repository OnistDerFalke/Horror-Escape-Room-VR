using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static int ItemsFound = 0;
    public static int ItemsToFind = 4;

    public enum GlobalSoundType
    {
        FirstRoom,
        Storage,
        SecondRoom,
        SoundsNumber
    }

    public static GlobalSoundType SoundType = GlobalSoundType.FirstRoom;
}

using UnityEngine;

namespace Interactions
{
    public class DoorPass : Interactable
    {
        protected override bool HandleFire1()
        {
            #if !UNITY_EDITOR
            Application.Quit();
            #endif
            return true;
        }
    }
}

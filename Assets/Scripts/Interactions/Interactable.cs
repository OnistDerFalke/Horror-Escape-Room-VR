using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace Interactions
{
    public class Interactable : MonoBehaviour
    {
        
        public bool Run(string input)
        {
            switch (input)
            {
                case "Fire1":
                    return HandleFire1();
                case "Fire2":
                    return HandleFire2();
            }
            Debug.LogError("Given input string was not found.");
            return false;
        }

        protected virtual bool HandleFire1()
        {
            return false;
        }

        protected virtual bool HandleFire2()
        {
            return false;
        }
    }
}

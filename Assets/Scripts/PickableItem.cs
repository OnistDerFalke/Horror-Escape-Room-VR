using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    public enum PickableType
    {
        AXE, KNIFE, PISTOL, PILLS
    }
    
    public Rigidbody Rb { get; private set; }
    public PickableType type;
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }
}
using Interactions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : Interactable
{
    [SerializeField] private Camera characterCamera;
    [SerializeField] private AudioController audioController;
    [SerializeField] private Transform slot;
    
    [SerializeField] private float throwItemSpeed;
    [SerializeField] private float dropItemSpeed;
    public enum PickableType
    {
        Axe, Knife, Pistol, Pills, Flashlight, Other
    }
    
    public Rigidbody Rb { get; private set; }
    public PickableType type;
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    protected override bool HandleFire1()
    {
        if (type == PickableType.Flashlight)
            PickTorch();
        else PickItem();
        return true;
    }

    private void PickTorch()
    {
        audioController.PlayPickItemSound();
        transform.SetParent(characterCamera.gameObject.transform);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        Rb.isKinematic = true;
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;
    }

    private void PickItem()
    {
        audioController.PlayPickItemSound();
        GameManager.itemInHands = this;
        Rb.isKinematic = true;
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;
        Transform tRef;
        (tRef = transform).SetParent(slot);
        tRef.localPosition = Vector3.zero;
        tRef.localEulerAngles = Vector3.zero;
    }
    
    public void DropItem()
    {
        GameManager.itemInHands = null;
        transform.SetParent(null);
        Rb.isKinematic = false;
        Rb.AddForce(characterCamera.transform.forward * dropItemSpeed, ForceMode.VelocityChange);
    }
    
    public void ThrowItem()
    {
        GameManager.itemInHands = null;
        transform.SetParent(null);
        Rb.isKinematic = false;
        Rb.AddForce(characterCamera.transform.forward * throwItemSpeed, ForceMode.Impulse);
    }
}
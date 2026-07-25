using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private InputActionReference inputAction;
    
    [Header("Detection")]
    [SerializeField, Min(1f)] private float maxDistance;
    [SerializeField] private LayerMask interactableLayer;
    
    private Transform _cameraTransform;
    private IInteractable _currentInteraction;

    private void Awake() => _cameraTransform = Camera.main?.transform;
    private void OnEnable() => inputAction.action.performed += OnInputPerformed;
    private void OnDisable() => inputAction.action.performed -= OnInputPerformed;
    private void OnInputPerformed(InputAction.CallbackContext _) => _currentInteraction?.Interact();

    private void Update()
    {
        Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, maxDistance, interactableLayer);
        OnUpdateCast(hit.collider);
    }
    private void OnUpdateCast(Collider hitCollider)
    {
        if (!hitCollider) {
            Deselect();
            return;
        }

        if (hitCollider.TryGetComponent(out IInteractable interactable))
            Select(interactable);
    }

    private void Deselect()
    {
        _currentInteraction?.Deselect();
        _currentInteraction = null;
    }
    private void Select(IInteractable newInteractable)
    {
        if (_cameraTransform == newInteractable) return;
        
        _currentInteraction = newInteractable;
        _currentInteraction?.Select();
    }
}
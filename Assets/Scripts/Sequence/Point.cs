using System;
using System.Collections;
using UnityEngine;
using TMPro;

[SelectionBase]
public class Point : MonoBehaviour, IInteractable
{
    [SerializeField] private TextMeshPro text;
    
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip idleClip, activeClip;
    
    [Header("Selection")]
    [SerializeField] private LayerMask idleLayer;
    [SerializeField] private LayerMask selectedLayer, errorLayer;

    public Func<int, bool> OnActivated { get; set; }

    private readonly WaitForSeconds _errorDelay = new(0.5f);
    private bool _isActive;
    private int _index;
    
    public void Setup(int index)
    {
        _index =  index;
        text?.SetText((_index + 1).ToString());
    }
    public void Activate()
    {
        text?.gameObject.SetActive(false);
        animator?.Play(activeClip.name);
        _isActive = true;
    }
    public void Deactivate()
    {
        StartCoroutine(ErrorRoutine());
        
        text?.gameObject.SetActive(true);
        animator?.CrossFade(idleClip.name, 0.5f);
        _isActive = false;
    }

    private IEnumerator ErrorRoutine()
    {
        gameObject.SetLayerRecursively(errorLayer.ToLayer());
        yield return _errorDelay;
        gameObject.SetLayerRecursively(idleLayer.ToLayer());
    }
    
    public void Interact()
    {
        if (_isActive) return;
        Deselect();
        
        if (OnActivated.Invoke(_index))
            Activate();
    }

    public void Select()
    {
        if (!_isActive)
            gameObject.SetLayerRecursively(selectedLayer.ToLayer());
    }
    public void Deselect()
    {
        if (!_isActive)
            gameObject.SetLayerRecursively(idleLayer.ToLayer());
    }
}
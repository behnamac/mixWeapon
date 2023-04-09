using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Collider _mainCollider;
    private Rigidbody _mainRigidbody;
    private Collider[] _allColliders;
    private Rigidbody[] _allRigidbodys;
    private Animator _animator;

    private void Awake()
    {
        TryGetComponent(out _mainCollider);
        TryGetComponent(out _mainRigidbody);
        TryGetComponent(out _animator);

        _allColliders = GetComponentsInChildren<Collider>();
        _allRigidbodys = GetComponentsInChildren<Rigidbody>();

        InactiveRagdoll();
    }

    public void ActiveRagdoll() 
    {
        for (int i = 0; i < _allColliders.Length; i++)
        {
            _allColliders[i].enabled = true;
        }
        for (int i = 0; i < _allRigidbodys.Length; i++)
        {
            _allRigidbodys[i].useGravity = true;
            _allRigidbodys[i].isKinematic = false;
        }

        _mainCollider.enabled = false;
        _mainRigidbody.useGravity = false;
        _mainRigidbody.isKinematic = true;
        _animator.enabled = false;

        //Invoke(nameof(EditeChildsLayer), 0.1f);
    }
    public void InactiveRagdoll()
    {
        for (int i = 0; i < _allColliders.Length; i++)
        {
            _allColliders[i].enabled = false;
        }
        for (int i = 0; i < _allRigidbodys.Length; i++)
        {
            _allRigidbodys[i].useGravity = false;
            _allRigidbodys[i].isKinematic = true;
        }

        _mainCollider.enabled = true;
        _mainRigidbody.useGravity = true;
        _mainRigidbody.isKinematic = false;
        _animator.enabled = true;
    }

    private void EditeChildsLayer() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = 0;
        }
    }
}

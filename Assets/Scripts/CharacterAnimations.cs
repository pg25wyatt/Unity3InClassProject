using CharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField] private float _damping = 0.1f;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CharacterMovement3D _movement;
    [SerializeField] private Health _health;

    private void OnValidate()
    {
        // fields cached in OnValidate need to be Serialized!
        // if not, they will not work in builds
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<CharacterMovement3D>();
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        // send values to animator
        _animator.SetBool("IsAlive", _health.IsAlive);
        _animator.SetBool("IsGrounded", _movement.IsGrounded);

        Vector3 flattenedVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        // fix world => local velocity
        Vector3 localVelocity = transform.InverseTransformVector(flattenedVelocity);
        _animator.SetFloat("Forward", localVelocity.z, _damping, Time.deltaTime);
        _animator.SetFloat("Right", localVelocity.x, _damping, Time.deltaTime);
    }
}
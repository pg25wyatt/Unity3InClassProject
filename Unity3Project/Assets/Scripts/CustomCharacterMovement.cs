using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMovement;
using UnityEngine.AI;

public class CustomCharacterMovement : CharacterMovement3D
{
    protected override void OnValidate()
    {
        // get and configure components
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _rigidbody.useGravity = true;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.height = _height;
        _capsuleCollider.center = new Vector3(0f, _height * 0.5f, 0f);
        _capsuleCollider.radius = _radius;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.height = _height;
        _navMeshAgent.radius = _radius;
    }

    // adding "new" keyword to confirm we're ignoring base awake method
    private new void Awake()
    {
        // disable NavMeshAgent
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;

        // add frictionless material to collider
        _capsuleCollider.material = new PhysicMaterial("NoFriction")
        {
            staticFriction = 0f,
            dynamicFriction = 0f,
            frictionCombine = PhysicMaterialCombine.Minimum
        };

    }

    public new void SetMoveInput(Vector3 input)
    {
        // 'sanitize' input to ensure move vector magnitude isn't greater than 1 
        input = Vector3.ClampMagnitude(input, 1.0f);
        if(input.magnitude > 0.1f)
        {
            input = new Vector3(input.x, 0f, input.z);
            MoveInput = input;
        }
        else
        {
            MoveInput = Vector3.zero;
        }
    }

    public new void SetLookPosition(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        SetLookDirection(direction);
    }    
    
    public new void SetLookDirection(Vector3 direction)
    {
        LookDirection = new Vector3(direction.x, 0f, direction.z).normalized;
    }

    public new void Jump()
    {
        if (!IsGrounded) return;

        // velocity = square root of 2 * gravity * height
        float jumpSpeed = Mathf.Sqrt(2f * -_gravity * _jumpHeight);

        //overwrite y velocity to jump
        Vector3 velocity = _rigidbody.velocity;
        _rigidbody.velocity = velocity;
    }

    protected override void FixedUpdate()
    {
        // TODO: check for ground
        IsGrounded = CheckGrounded();

        // TODO: move using navigation if direction is set
        if (_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh && _navMeshAgent.hasPath)
        {
            Vector3 nextPathPoint = _navMeshAgent.path.corners[1];
            Vector3 pathDir = (nextPathPoint - transform.position).normalized;

            SetMoveInput(pathDir);
            SetLookDirection(pathDir);
        }

        _navMeshAgent.nextPosition = transform.position;

        // TODO: move character
        // find ground aligned froward direction
        Vector3 input = MoveInput;
        Vector3 right = Vector3.Cross(transform.up, input);
        Vector3 forward = Vector3.Cross(-GroundNormal, right);

        Vector3 targetVelocity = forward * _speed * MoveSpeedMultiplier;
        Vector3 flattenedVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        Vector3 velocityDiff = targetVelocity - flattenedVelocity;

        // calculate control value (on ground / in air)
        float control = IsGrounded ? 1f : _airControl; // some people hate this
        Vector3 acceleration = velocityDiff * control * _acceleration;

        // add gravity
        acceleration += GroundNormal * _gravity;

        // accelerate incorporating mass (heavy moves light)
        _rigidbody.AddForce(acceleration * _rigidbody.mass);

        // TODO: turn character
        // turn in look direction if valid
    }

    protected override void Update()
    {
        if (LookDirection.magnitude > 0.1f)
        {
            // turn LookDirection (Vector3) into a (Quaternion)
            Quaternion targetRotation = Quaternion.LookRotation(LookDirection);
            Quaternion currentRotation = transform.rotation;
            Quaternion rotation = Quaternion.Slerp(currentRotation, targetRotation, _turnSpeed * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(rotation); // don't set transform values directly when physics are involved
                                               // MoveRotation updates the RigidBody properly, add forces, and also interpolates properly
            _rigidbody.MoveRotation(targetRotation);
        }
    }

    private new bool CheckGrounded()
    {
        Vector3 groundCheckStart = transform.position + transform.up * _groundCheckOffset;

        //raycast for ground 
        bool hit = Physics.Raycast(groundCheckStart, -transform.up, out RaycastHit hitInfo, _groundCheckDistance, _groundMask);

        // set default ground normal
        GroundNormal = Vector3.up;

        if (!hit) return false;

        // set ground normal from hit
        GroundNormal = hitInfo.normal;
        return true;
    }
}
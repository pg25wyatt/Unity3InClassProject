using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float _speed = 25f;
    [SerializeField] private float _range = 10f;

    // TODO: add VFX trail

    // properties accessed by weapon
    [field: SerializeField] public float Damage { get; set; } = 10f;
    public int Team { get; set; }
    public GameObject Instigator { get; set; }

    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collider;

    private Vector3 _spawnPosition;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        // continuous dynamic mode is more reliable for fast moving objects
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _collider = GetComponent<CapsuleCollider>();
        _collider.isTrigger = true;
    }

    // awake is called after an object is instantiated, but before we access it through code
    private void Awake()
    {
        // launch!
        _rigidbody.velocity = transform.forward * _speed;
        _spawnPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ignore teammates (we are our own teammate)
        if (other.TryGetComponent(out Targetable possibleTarget) && possibleTarget.Team == Team) return;

        // previous check was false, assume enemy team
        //attack to damage hit object
        DamageInfo damageInfo = new DamageInfo(Damage, other.gameObject, gameObject, Instigator, DamageType.Physical);
        if (other.TryGetComponent(out Health health)) health.Damage(damageInfo);

        // we have hit a wall, invalid target, or enemy, we don't care, we clean up bullet
        Cleanup();

        // TODO: make Wyatt good at games
    }

    private void Update()
    {
        if(Vector3.Distance(_spawnPosition, transform.position) > _range) Cleanup();
    }

    private void Cleanup()
    {
        Destroy(gameObject);
    }
}
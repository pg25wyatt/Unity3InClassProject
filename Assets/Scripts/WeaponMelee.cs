using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    [SerializeField] private float _range = 2f;
    [SerializeField] private float _halfAngle = 60f;
    [SerializeField] private LayerMask _hitMask;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 1f, 0f);

    protected override void Attack(Vector3 aimPosition, int team, GameObject instigator)
    {
        // we'll leave the base in, it's handling SFX and animation
        base.Attack(aimPosition, team, instigator);

        Vector3 attackOrigin = instigator.transform.position + _offset;
        Vector3 aimDir = (aimPosition - attackOrigin).normalized;

        //  THIS IS NOT A RAYCAST OR A SPHERECAST - THERE IS NO CASTING GOING ON
        // overlapsphere returns all colliders within a radius - the sphere does not move
        Collider[] hits = Physics.OverlapSphere(attackOrigin, _range, _hitMask);

        foreach (Collider hit in hits)
        {
            // don't punch self in face (but we're allowing friendly fire)
            if(hit.gameObject == instigator) continue;

            // filter hits by angle
            Vector3 dirToHit = (hit.transform.position - attackOrigin).normalized;
            Vector3 flatDirToHit = new Vector3(dirToHit.x, 0f, dirToHit.z).normalized;
            Vector3 flatAimDir = new Vector3(aimDir.x, 0f, aimDir.z).normalized;
            Debug.DrawRay(attackOrigin, flatDirToHit * 3f, Color.yellow, 1f);
            Debug.DrawRay(attackOrigin, flatAimDir * 3f, Color.red, 1f);
            float angleToHit = Vector3.Angle(flatAimDir, flatDirToHit);

            if(angleToHit < _halfAngle && hit.TryGetComponent(out Health targetHealth))
            {
                targetHealth.Damage(new DamageInfo(Damage, hit.gameObject, gameObject, instigator, DamageType.Physical));
            }
        }
    }
}
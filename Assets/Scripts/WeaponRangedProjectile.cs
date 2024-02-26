using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WeaponRangedProjectile : Weapon
{
    [BoxGroup("Projectile"), SerializeField] private Transform _muzzle;
    [BoxGroup("Projectile"), SerializeField] private Projectile _prefab;
    [BoxGroup("Projectile"), SerializeField] private int _shotCount = 6;
    [BoxGroup("Projectile"), SerializeField, SuffixLabel("degrees", Overlay = true)] private float _inaccuracy = 5f;

    protected override void Attack(Vector3 aimPosition, int team, GameObject instigator)
    {
        base.Attack(aimPosition, team, instigator);

        Vector3 spawnPos = _muzzle.position;
        // direction from A to B is always B minus A normalized
        Vector3 aimDir = (aimPosition - spawnPos).normalized;
        // find spawn rotation from aim direction
        Quaternion spawnRot = Quaternion.LookRotation(aimDir);

        for (int i = 0; i < _shotCount; i++)
        {
            // randomly generate inaccuracy
            float inaccX = Random.Range(-_inaccuracy, _inaccuracy);
            float inaccY = Random.Range(-_inaccuracy, _inaccuracy);

            // create a rotation from inaccuracy
            Quaternion inaccRot = Quaternion.Euler(_muzzle.up * inaccX + _muzzle.right * inaccY);

            // combine initial and inaccuracy rotations - we combine quaternions by multiplying them
            Quaternion finalRotation = spawnRot * inaccRot;

            // spawn projectile and assign intial values
            Projectile projectile = Instantiate(_prefab, spawnPos, finalRotation);
            projectile.Damage = Damage;
            projectile.Team = team;
            projectile.Instigator = instigator;
        }
    }
}
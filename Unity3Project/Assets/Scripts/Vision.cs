using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Vision : MonoBehaviour
{
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _FOV = 140f;
    [SerializeField] private Transform _head;
    [SerializeField] private LayerMask _occlusionMask;  // walls and obstructions
    [SerializeField] private LayerMask _visiblityMask;  // things we can see (characters)

    public Vector3 HeadPosition => _head.position;
    public Vector3 HeadDirection => _head.forward;

    private void OnValidate()
    {
        if(_head == null)
        {
            _head = transform.GetComponentsInChildren<Transform>().
                Where(t => t.name.Contains("Head")).FirstOrDefault();
        }
    }

    public bool TestVisibility(Vector3 point)
    {
        Vector3 vector = point - HeadPosition;
        float distance = vector.magnitude;
        if (distance > _range) return false;    // point is too far

        Vector3 direction = vector.normalized;
        float angle = Vector3.Angle(HeadDirection, direction);
        if (angle > _FOV * 0.5f) return false;      // point is out of FOV

        if (Physics.Linecast(HeadPosition, point, _occlusionMask)) return false;    // vision blocked if Linecast hits something

        return true;
    }

    public List<Targetable> GetVisibleTargets(int team)
    {
        List<Targetable> targets = new List<Targetable>();
        // find all colliders in range
        // THIS IS NOT A CAST/RAYCAST/SPHERECAST
        Collider[] hits = Physics.OverlapSphere(HeadPosition, _range, _visiblityMask);
        foreach (Collider hit in hits )
        {
            if (hit.gameObject == gameObject) continue;
            if (!hit.TryGetComponent(out Targetable target)) continue;
            if (target.Team == team || !target.IsTargetable) continue;
            if (!TestVisibility(target.VisibilityTransform.position)) continue;

            // if target passes all checks, we can target it
            targets.Add(target);
        }

        return targets;
    }

    public Targetable GetFirstVisibleTarget(int team)
    {
        List<Targetable> targets = GetVisibleTargets(team);
        if (targets.Count < 1) return null;
        return targets[0];
    }

    // draw gizmos only when object is selected in hierarchy
    private void OnDrawGizmosSelected()
    {
        if (_head == null) return;  // stop if no head

        Gizmos.DrawWireSphere(HeadPosition, _range);
        float halfFOV = _FOV * 0.5f;
        Quaternion rotationRight = Quaternion.Euler(0f, halfFOV, 0f) * _head.rotation;
        Quaternion rotationLeft = Quaternion.Euler(0f, -halfFOV, 0f) * _head.rotation;
        // multiplying Quaternions and directions gets the direction from a rotation
        Vector3 rayRight = rotationRight * Vector3.forward * _range;
        Vector3 rayLeft = rotationLeft * Vector3.forward * _range;
        Gizmos.DrawRay(HeadPosition, rayRight);
        Gizmos.DrawRay(HeadPosition, rayLeft);
    }
}
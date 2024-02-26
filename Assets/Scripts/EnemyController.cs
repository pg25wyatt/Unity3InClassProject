using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyController : Controller
{
    [Header("Targeting")]
    [SerializeField, ReadOnly] private Targetable _target;

    [Header("Wandering")]
    [SerializeField] private float _waypointReachedDistance = 1f;
    [SerializeField] private float _wanderSpeedMultiplier = 0.333f;
    [SerializeField] private Vector2 _waitTimeRange = new Vector2(2f, 4f);

    // useful fields for targeting
    public bool IsTargetValid => _target != null && _target.IsTargetable;
    public bool IsTargetVisible => Vision.TestVisibility(_target.VisibilityTransform.position);
    public float TargetDistance => Vector3.Distance(_target.transform.position, transform.position);

    private Waypoint[] _waypoints;
    private IEnumerator _currentState;

    private void Start()
    {
        _waypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);

        // start initial state
        ChangeState(WanderState());
    }

    private Waypoint FindRandomWaypoint()
    {
        int randomIndex = Random.Range(0, _waypoints.Length);
        return _waypoints[randomIndex];
    }

    private void ChangeState(IEnumerator newState)
    {
        // stop current state
        if (_currentState != null) StopCoroutine(_currentState);

        // reset move speed
        Movement.MoveSpeedMultiplier = 1f;

        // assign and start new state
        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private IEnumerator WanderState()
    {
        // Start
        Waypoint waypoint = null;
        Movement.MoveSpeedMultiplier = _wanderSpeedMultiplier;

        // Update
        while(!IsTargetValid)
        {
            // find waypoint and move to it
            if (waypoint == null) waypoint = FindRandomWaypoint();
            else Movement.MoveTo(waypoint.transform.position);

            // find new waypoint if current reached
            float distance = Vector3.Distance(transform.position, waypoint.transform.position);
            if (distance < _waypointReachedDistance) ChangeState(WaitState());

            // yield return null in a while loop in a coroutine tells the engine to loop back every frame to executre this loop once
            // this is like a mini Update function inside out coroutine
            yield return null;
        }

        // OnDestroy
        ChangeState(AttackState());
    }

    private IEnumerator WaitState()
    {
        Movement.Stop();
        float timer = 0f;
        float waitTime = Random.Range(_waitTimeRange.x, _waitTimeRange.y);

        while(!IsTargetValid && timer < waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        ChangeState(WanderState());
    }

    private IEnumerator AttackState()
    {
        // only attack valid target
        while(IsTargetValid)
        {
            Movement.MoveTo(_target.transform.position);
            float targetDistance = Vector3.Distance(transform.position, _target.transform.position);
            Weapon weapon = Weapons[0];     // with multiple weapons we could use cooldowns or priority/score to determine best attack

            if(targetDistance < weapon.EffectiveRange)
            {
                Movement.Stop();
                Movement.SetLookPosition(_target.transform.position);

                // attempt an attack
                if(weapon.TryAttack(_target.VisibilityTransform.position, Targetable.Team, gameObject))
                {
                    yield return new WaitForSeconds(weapon.Duration);
                }
            }

            yield return null;
        }

        ChangeState(WaitState());
    }

    private IEnumerator DeadState()
    {
        Movement.Stop();
        yield return null;
    }

    public void Death()
    {
        ChangeState(DeadState());
    }

    public void DamageReaction(DamageInfo damageInfo)
    {
        if(damageInfo.Instigator.TryGetComponent(out Targetable possibleTarget) && possibleTarget.Team != Targetable.Team)
        {
            _target = possibleTarget;
        }
    }

    private void Update()
    {
        TryFindTarget();
    }

    private void TryFindTarget()
    {
        if (IsTargetValid) return;
        // if target is NOT valid, find new target
        _target = Vision.GetFirstVisibleTarget(Targetable.Team);
    }
}
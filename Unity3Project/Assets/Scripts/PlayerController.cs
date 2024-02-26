using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : Controller
{
    [Header("Aiming")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _aimVerticalOffset = 1f;

    [field: SerializeField] public bool IsEnabled { get; set; } = true;

    // Quin prefers adding an underscore in front of private fields
    private Vector2 _moveInput2D;
    private Vector3 _aimPosition;

    // called from PlayerInput component
    public void OnMove(InputValue value)
    {
        // retrieve Vector2 (up/down, left/right)
        _moveInput2D = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if(!IsEnabled) return;

        Movement.Jump();
    }

    public void OnFire()
    {
        if (!IsEnabled) return;

        // assume weapon 0 is gun
        Weapon gun = Weapons[0];
        gun.TryAttack(_aimPosition, Targetable.Team, gameObject);
    }

    private void Update()
    {
        if(!IsEnabled)
        {
            Movement.Stop();
            return;
        }

        // convert Vector2 input to world space move direction, then move
        Vector3 moveInput3D = new Vector3(_moveInput2D.x, 0f, _moveInput2D.y);
        Movement.SetMoveInput(moveInput3D);

        // use raycast to find world position of mouse
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);   // ray from camera, through cursor, into world
        if(Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, _groundMask))
        {
            Vector3 mouseWorldPosition = hit.point;

            // create virtual plane above the ground
            Plane aimPlane = new Plane(Vector3.up, mouseWorldPosition + Vector3.up * _aimVerticalOffset);

            // raycast against virtual plane, returning distance to plane along ray
            if(aimPlane.Raycast(mouseRay, out float planeDistance))
            {
                mouseWorldPosition = mouseRay.GetPoint(planeDistance);
            }
            
            Debug.DrawLine(Camera.main.transform.position, mouseWorldPosition, Color.yellow);


            // look at aim position
            Movement.SetLookPosition(mouseWorldPosition);

            _aimPosition = mouseWorldPosition;
        }
    }
}
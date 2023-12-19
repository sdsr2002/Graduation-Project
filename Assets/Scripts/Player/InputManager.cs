using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerInput.OnFootActions _onFoot;
    private PlayerMotor _motor;
    private PlayerLook _look;
    //private PlayerShoot _shoot;
    //private Hitscan _hitscan;
    //private ShotGun _shotgun;
    //private ReloadBar _reloadBar;
    //private bool isReloading = false;

    // Start is called before the first frame update
    void Awake()
    {
        _playerInput = new PlayerInput();
        _onFoot = _playerInput.OnFoot;
        _motor = GetComponent<PlayerMotor>();
        _look = GetComponent<PlayerLook>();
        _onFoot.Jump.performed += ctx => _motor.Jump();
        //_shoot = GetComponent<PlayerShoot>();
        //_hitscan = GetComponent<Hitscan>();
        //_shotgun = GetComponent<ShotGun>();
        //_reloadBar = GetComponent<ReloadBar>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _motor.ProcessMove(_onFoot.Movement.ReadValue<Vector2>());
    }

    private void Update()
    {
        //if (_onFoot.Shoot.triggered)
        //{
        //    _shoot.FireWeapon();
        //}

        //if (_onFoot.Shoot.triggered)
        //{
        //    _hitscan.Shoot();
        //}

        //if (_onFoot.Shoot.triggered)
        //{
        //    _shotgun.Shoot();
        //}

        //if (_onFoot.Reload.triggered)
        //{
        //    if (_shoot.ProjectileWeapon && !isReloading)
        //    {
        //        _reloadBar.AnimateBar(_shoot.ReloadTime);
        //        _shoot.ReloadProjectileWeapon();
        //    }
        //    else
        //    {
        //        Debug.Log("Already reloading!");
        //    }
        //}
    }

    private void LateUpdate()
    {
        _look.ProcessLook(_onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        _onFoot.Enable();
    }

    private void OnDisable()
    {
        _onFoot.Disable();
    }
}

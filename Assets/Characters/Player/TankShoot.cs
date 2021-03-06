﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankShoot : MonoBehaviour, IShooter
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] float shootPower;
    [SerializeField] float recoilPower;
    [SerializeField] float reloadSpeed;
    [SerializeField] Transform cabinTransform;
    float reloadTimer;
    bool canShoot = true;
    Rigidbody rb;
    public event Action<Vector3, Vector3> OnShoot;
    public GameObject ProjectilePrefab => projectilePrefab;
    public float Power => shootPower;
    public float ReloadSpeed => reloadSpeed;
    public Transform Muzzle => muzzle;

    public Vector3 GetMuzzleDirection()
    {
        return cabinTransform.forward;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Aim(Vector3 aimPosition)
    {
        var lookPos = aimPosition - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        cabinTransform.rotation = Quaternion.Slerp(cabinTransform.rotation, rotation, 5 * Time.deltaTime);
    }

    public void ResetMuzzle()
    {
        var rotation = Quaternion.LookRotation(transform.forward);
        cabinTransform.rotation = Quaternion.Slerp(cabinTransform.rotation, rotation, 5 * Time.deltaTime);
    }

    private void Update()
    {
        if (!canShoot)
        {
            Reload();
        }
    }

    public void Shoot()
    {
        if (canShoot)
        {
            GameObject go = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            TankProjectile projectile = go.GetComponent<TankProjectile>();
            projectile.SetSpeed(shootPower);
            OnShoot?.Invoke(Muzzle.forward, Muzzle.position);
            Recoil();
            canShoot = !canShoot;
        }
    }

    void Recoil()
    {
        rb.AddForce(-cabinTransform.forward * recoilPower);
    }

    public void Reload()
    {
        reloadTimer += Time.deltaTime;
        if (reloadTimer >= reloadSpeed)
        {
            canShoot = !canShoot;
            reloadTimer = 0;
        }
    }
}

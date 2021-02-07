﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private GunController gunController;
    [SerializeField] private Timer shootTimer;
    [SerializeField] private ObjectPooler bulletPool;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Transform gunsRef;

    private Vector2 dir;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 animDir = new Vector2(0.0f, -1.0f);

    private void Start() {
        Init();
    }

    private void Update() {
        CalcMoveDirection();
        CalcAnimDirection();

        Shoot();
    }

    private void FixedUpdate() {
        rb.velocity = speed * Time.deltaTime * dir;
    }

    private void CalcMoveDirection() {
        dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        dir = (dir.sqrMagnitude > 1.0f) ? dir.normalized : dir;
        anim.SetBool("isMoving", dir != Vector2.zero);
    }

    private void Init() {
        //To be able to shoot the first time
        shootTimer.SetIsTimeCompleted(true);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void CalcAnimDirection() {
        animDir = gunController.GetDirection().normalized;
        anim.SetFloat("dirX", animDir.x);
        anim.SetFloat("dirY", animDir.y);
    }

    private void Shoot() {
        if (Input.GetMouseButton(0) && shootTimer.GetIsTimeCompleted()) {
            shootTimer.StartTimer();
            if (bulletPool.HasObjectsToPool()) {
                GameObject bullet = bulletPool.GetPoolPrefab();
                bullet.transform.rotation = gunsRef.transform.localRotation;
                bullet.transform.position = bulletSpawn.transform.position;
            }
        }
    }

    //Getters And Setters
    public int GetCurrentSortingLayer() => sprite.sortingOrder;
    public Vector2 GetAnimDirection() => animDir;
}

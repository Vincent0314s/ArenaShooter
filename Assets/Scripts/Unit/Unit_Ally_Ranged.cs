using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utils;

public class Unit_Ally_Ranged : Unit_Ally
{
    private Pool_Projectile m_PoolProjectile;

    [Header("Attack Settings"),Space()]
    [SerializeField] private Color rangeColor = Color.white;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackInterval = 1f;
    private float currentIntervalTimer;

    private Transform launchPoint;

    protected Transform firstEnemy { get; private set; }
    [SerializeField] private LayerMask enemyMask;

    private Projectile currentProjectile;

    protected override void Start()
    {
        base.Start();
        launchPoint = transform.Find("LaunchPoint");
    }

    private void FixedUpdate()
    {
        CheckHasFirstEnemy();
    }

    protected void CheckHasFirstEnemy()
    {
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyMask);
        Collider[] hitColliders = new Collider[1];
        int i = Physics.OverlapSphereNonAlloc(transform.position,attackRange, hitColliders,enemyMask);

        if (i != 0 && firstEnemy == null)
        {
            firstEnemy = hitColliders[0].transform;
        }
    }

    protected virtual void Attack(Element _element) {
        if (firstEnemy == null) {
            currentIntervalTimer = 0;
            return;
        }

        LookAtTarget(firstEnemy);
        if (currentIntervalTimer < attackInterval)
        {
            currentIntervalTimer += Time.deltaTime;
        }
        else
        {
            ShootProjectile(_element);
            currentIntervalTimer = 0;
        }

        RemoveEnemyFromRange();
    }

    protected void ShootProjectile(Element _element) {
        currentProjectile = Pool_Projectile.i.GetObjectFromPool((int)_element);
        currentProjectile.gameObject.SetActive(true);
        currentProjectile.gameObject.transform.position = launchPoint.position;
        currentProjectile.SetTarget(firstEnemy);
    }

    protected void RemoveEnemyFromRange() {
        float dis = Vector3.Distance(transform.position, firstEnemy.position);
        if (dis > attackRange + 0.5f)
        {
            firstEnemy = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = rangeColor;
        Handles.DrawWireDisc(transform.position, transform.up, attackRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private DamageSO m_Projectile;
    private Rigidbody rb;

    private bool canShoot;
    private Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnToPoolCoroutine());
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (canShoot) {
            ShootToTarget();
            canShoot = false;
        }
    }

    public void SetTarget(Transform _target)
    {
        canShoot = true;
        target = _target;
    }

    public void ShootToTarget() {
        //Vector3 targetPos = target.position + target.forward * 1.2f;
        Vector3 targetPos = new Vector3(target.position.x,target.position.y + 1, target.position.z);
        transform.LookAt(targetPos);
        rb.AddForce(transform.forward * m_Projectile.speed);
    }

    IEnumerator ReturnToPoolCoroutine() {
        var waitTimer = new WaitForSeconds(2f);
        yield return waitTimer;
        transform.position = Vector3.zero;
        Pool_Projectile.i.ReturnObjectToPool_ObjectDeactived(this, (int)m_Projectile.element);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(ConstStringCollection.ENEMY))
        {
            Debug.Log("HitEnemy");
            other.GetComponent<IHPChange>().GetDamageByAmount(m_Projectile.damage);
            other.GetComponent<IDebuff>().AddDebuff(m_Projectile.element);
            transform.position = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Enemy : MonoBehaviour
{
    //Public vars
    public float m_AggroRange = 15.0f;
    public float m_FireRate = 1.5f;
    public float m_MovementSpeed = 10.0f;
    public float m_TurnSpeed = 5.0f;

    //Component vars
    protected Rigidbody m_Rigidbody;
    protected Collider m_Collider;

    //Target vars
    protected Transform m_Target;

    //Shoot vars
    protected Transform BulletPosition;
    protected bool m_CanShoot = true;
    protected float m_ShootTimer = 0.0f;

	protected virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_Target = FindObjectOfType<ControllerPlayer>().transform;

        m_ShootTimer = m_FireRate;
	}
	
	protected virtual void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3.0f, Color.red);
        FireUpdate();
        RotationUpdate();
	}

    protected virtual void MovementUpdate()
    {

    }

    protected virtual void RotationUpdate()
    {
        if (m_Target)
        {
            if (Vector3.Distance(transform.position, m_Target.position) < m_AggroRange)
            {
                Vector3 target = new Vector3(m_Target.position.x, 0.0f, m_Target.position.z);
                Vector3 pos = new Vector3(transform.position.x, 0.0f, transform.position.z);
                Quaternion q = Quaternion.LookRotation((target - pos).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, m_TurnSpeed * Time.deltaTime);
            }
        }
    }

    void FireUpdate()
    {
        if (!m_CanShoot)
        {
            m_ShootTimer -= Time.deltaTime;
            if (m_ShootTimer <= 0.0f)
            {
                m_ShootTimer = m_FireRate;
                m_CanShoot = true;
            }
        }
    }

    protected virtual void Shoot()
    {

    }
}

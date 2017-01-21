using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ControllerPlayer : MonoBehaviour
{
    //Public vars
    public float m_MovementSpeed = 20.0f;

    //Component vars
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;

    //Movement vars
    private float m_MaxSpeed = 0.0f;

    //Rotation vars
    private Transform m_Pointer;

	void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_MaxSpeed = m_MovementSpeed * m_MovementSpeed;

        if (transform.FindChild("PointerRotation"))
            m_Pointer = transform.FindChild("PointerRotation");
        else
            Debug.Log("Couldn't find pointer!");

    }
	
	void Update()
    {
        MovementUpdate();
        RotationUpdate();
	}

    void FixedUpdate()
    {
        MaxSpeedCheck();
    }

    void MaxSpeedCheck()
    {
        if (m_Rigidbody.velocity.sqrMagnitude > m_MaxSpeed)
            m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * m_MovementSpeed;
    }

    void MovementUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_Rigidbody.velocity = movement * m_MovementSpeed;
    }

    void RotationUpdate()
    {
        //Rotate towards mouseposition
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_Pointer.localRotation = Quaternion.AngleAxis(angle, Vector3.down);
    }
}

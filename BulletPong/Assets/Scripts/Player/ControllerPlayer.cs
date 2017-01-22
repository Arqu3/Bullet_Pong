using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ControllerPlayer : MonoBehaviour
{
    //Public vars
    public float m_MovementSpeed = 20.0f;
    public float m_DashCD = 0.6f;
    public float m_DashTime = 0.3f;
    public float m_DashSpeed = 35.0f;
    public float m_RacketSpeed = 0.5f;
    public float m_RacketCooldown = 1.0f;

    //Component vars
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;

    //Movement vars
    private float m_MaxSpeed = 0.0f;

    //Rotation vars
    private Transform m_Pointer;

    //Ping pong vars
    private Transform m_Racket;
    private const float m_RacketAngle = 60.0f;
    private bool m_IsRacketCD = false;
    private bool m_IsRacketActive = false;
    private float m_RacketCDTimer = 0.0f;
    private float m_RacketTimer = 0.0f;

    //Dash vars
    private bool m_IsDashCD = false;
    private bool m_IsDash = false;
    private float m_DashTimer = 0.0f;
    private float m_DashCDTimer = 0.0f;
    private Vector3 m_DashDir = Vector3.zero;

	void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_MaxSpeed = m_MovementSpeed * m_MovementSpeed;

        if (transform.FindChild("PointerRotation"))
        {
            m_Pointer = transform.FindChild("PointerRotation");

            if (m_Pointer.FindChild("RacketRotation"))
            {
                m_Racket = m_Pointer.FindChild("RacketRotation");
                m_Racket.gameObject.SetActive(false);
            }
        }
        else
            Debug.Log("Couldn't find pointer!");

        m_RacketCDTimer = m_RacketCooldown;
        m_DashTimer = m_DashTime;
        m_DashCDTimer = m_DashCD;
    }
	
	void Update()
    {
        MovementUpdate();
        RotationUpdate();
        RacketUpdate();
        DashUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

    void FixedUpdate()
    {
        MaxSpeedCheck();
    }

    void MaxSpeedCheck()
    {
        if (m_Rigidbody.velocity.sqrMagnitude > m_MaxSpeed && !m_IsDash)
            m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * m_MovementSpeed;
    }

    void MovementUpdate()
    {
        if (!m_IsDash)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            m_Rigidbody.velocity = movement * m_MovementSpeed;
        }
    }

    void RotationUpdate()
    {
        //Rotate towards mouseposition
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_Pointer.localRotation = Quaternion.AngleAxis(angle, Vector3.down);
    }

    void RacketUpdate()
    {
        if (m_Racket)
        {
            if (Input.GetMouseButton(0))
            {
                if (!m_IsRacketCD)
                {
                    m_IsRacketCD = true;
                    m_IsRacketActive = true;
                    m_Racket.gameObject.SetActive(true);
                    m_Racket.localEulerAngles = new Vector3(0, -m_RacketAngle, 0);
                }
            }

            if (m_IsRacketCD)
            {
                m_RacketCDTimer -= Time.deltaTime;
                if (m_RacketCDTimer <= 0.0f)
                {
                    m_RacketCDTimer = m_RacketCooldown;
                    m_IsRacketCD = false;
                }
            }

            if (m_IsRacketActive)
            {
                m_RacketTimer += Time.deltaTime;
                if (m_RacketTimer >= m_RacketSpeed)
                {
                    m_RacketTimer = 0.0f;
                    m_IsRacketActive = false;
                    m_Racket.gameObject.SetActive(false);
                }

                float speed = m_RacketAngle / m_RacketSpeed * 2;
                m_Racket.localEulerAngles += new Vector3(0, speed, 0) * Time.deltaTime;
            }
        }
    }

    void DashUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!m_IsDashCD)
            {
                m_IsDashCD = true;
                m_IsDash = true;
                m_DashDir = m_Rigidbody.velocity.normalized;
            }
        }

        if (m_IsDashCD)
        {
            m_DashCDTimer -= Time.deltaTime;
            if (m_DashCDTimer <= 0.0f)
            {
                m_DashCDTimer = m_DashCD;
                m_IsDashCD = false;
            }
        }

        if (m_IsDash)
        {
            m_DashTimer -= Time.deltaTime;
            if (m_DashTimer <= 0.0f)
            {
                m_DashTimer = m_DashTime;
                m_IsDash = false;
            }

            m_Rigidbody.velocity = m_DashDir * m_DashSpeed;
        }
    }
}

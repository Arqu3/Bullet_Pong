using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    //Public vars
    public Transform m_FollowTransform;
    public float m_MaxDistance = 10.0f;

    //Offset vars
    private Vector3 m_Offset = Vector3.zero;

    //Position vars
    private Vector3 m_Destination = Vector3.zero;
    private RaycastHit m_Hit;

	void Start()
    {
		if (!m_FollowTransform)
        {
            Debug.Log("Followtransform is missing!");
            enabled = false;
            return;
        }

        m_Offset = m_FollowTransform.position - transform.position;
	}
	
	void Update()
    {
        PositionUpdate();
    }

    void PositionUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out m_Hit, 100))
        {
            if (Vector3.Distance(m_FollowTransform.position, m_Hit.point) < m_MaxDistance)
                m_Destination = (m_FollowTransform.position + m_Hit.point) / 2.0f - m_Offset;
            else
            {
                Vector3 outline = m_FollowTransform.position + (m_Hit.point - m_FollowTransform.position).normalized * m_MaxDistance;
                m_Destination = (m_FollowTransform.position + outline) / 2.0f - m_Offset;
            }
        }
        else
            m_Destination = m_FollowTransform.position - m_Offset;

        transform.position = Vector3.Lerp(transform.position, m_Destination, 5 * Time.deltaTime);
    }
}

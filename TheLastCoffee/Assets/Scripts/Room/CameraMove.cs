using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    Transform m_myTransform;
    Vector3 m_targetPos;
    Coroutine m_moving;

	// Use this for initialization
	void Start () {
        m_myTransform = transform;
	}

    public void StartMove(Vector3 target)
    {
        m_targetPos = target;
        m_targetPos.z = m_myTransform.position.z;

        if (m_moving != null)
            StopCoroutine(m_moving);

        m_moving = StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        while (true)
        {
            m_myTransform.position = Vector3.Lerp(m_myTransform.position, m_targetPos, 10 * Time.deltaTime);

            if (m_myTransform.position == m_targetPos)
                yield break;

            yield return new WaitForEndOfFrame();
        }

    }
}

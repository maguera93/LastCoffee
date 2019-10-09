using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Transform positionToMove;

    private Animator m_animator;
    public BoxCollider2D m_collider;

	// Use this for initialization
	void Awake () {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider2D>();
        //OpenDoorAnim(true);
	}

    public void OpenDoorAnim(bool withCollider)
    {
        if (withCollider)
            m_collider.isTrigger = true;

        m_animator.SetTrigger("Open");
    }

    public void CloseDoorAnim(bool withCollider)
    {
        if(withCollider)
            m_collider.isTrigger = false;

        m_animator.SetTrigger("Close");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            MovePlayer(collision.transform);
        }
    }

    public void MovePlayer(Transform playerTranform)
    {
        playerTranform.position = positionToMove.position;
    }
}

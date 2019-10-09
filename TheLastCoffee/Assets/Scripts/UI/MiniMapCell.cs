using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCell : MonoBehaviour {

    [SerializeField]
    private Sprite m_selectedSprite;
    [SerializeField]
    private Sprite m_unselectedSprite;
    [SerializeField]
    private GameObject m_bossSprite;
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    private Transform m_cameraTransform;

	// Use this for initialization
	void Awake () {
        m_cameraTransform = GameObject.FindGameObjectWithTag("MiniMapCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BossRoom()
    {
        m_bossSprite.SetActive(true);
    }

    public void RoomEntered()
    {
        Vector3 cameraPos = transform.position;
        cameraPos.z = m_cameraTransform.position.z;
        m_cameraTransform.position = cameraPos;
        m_spriteRenderer.sprite = m_selectedSprite;
    }

    public void RoomExit()
    {
        m_spriteRenderer.sprite = m_unselectedSprite;
    }
}

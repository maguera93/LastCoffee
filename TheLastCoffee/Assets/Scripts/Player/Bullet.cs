using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float f_speed;
    private Transform m_trans;
    [SerializeField]
    private bool b_active;
    [SerializeField]
    private GameObject m_bulletHit;

    [SerializeField]
    private GameObject m_trailGO;

    [SerializeField]
    private float f_bulletDamage;

    public float Damage
    {
        get
        {
            return f_bulletDamage;
        }
    }

	// Use this for initialization
	void Awake () {
        m_trans = transform;
        
	}
	
	// Update is called once per frame
	void Update () {
        if(b_active)
            m_trans.Translate(Vector3.up * f_speed * Time.deltaTime);
	}

    public void ShootBullet(Vector3 pos, Vector3 rot)
    {
        if (b_active)
            return;

        m_trans.position = pos;
        m_trans.eulerAngles = rot;
        m_trailGO.SetActive(true);
        b_active = true;
    }

    public void SetParent(Transform parent)
    {
        m_trans.parent = parent;
        m_trans.localPosition = Vector3.zero;
    }

    public void ResetPos()
    {
        m_trans.localPosition = Vector3.zero;
        b_active = false;
        m_trailGO.SetActive(false);
    }

    public void BulletHit()
    {
        GameObject ps = Instantiate(m_bulletHit, transform.position, Quaternion.identity);
        Destroy(ps, 0.7f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" || collision.tag == "Door")
        {
            ResetPos();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Room")
        {
            ResetPos();
        }
    }
}

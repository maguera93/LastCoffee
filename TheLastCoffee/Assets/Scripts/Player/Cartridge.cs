using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour {

    private const float SHOOT_DELAY = 0.2F;

    [SerializeField]
    private GameObject m_bulletGO;
    private int i_currentBullet;
    private Bullet[] m_bullets;
    [SerializeField]
    private Transform m_bulletContainer;
    private bool b_bulletShooted;
    [SerializeField]
    private int i_maxBullets;
    private Transform m_trans;
    private float f_currentTime;
    private AudioManager m_audioManager;
    private int _sfxIndex;
    public int SFXIndex
    {
        set
        {
            _sfxIndex = value;
        }
    }

	// Use this for initialization
	void Start () {
        m_trans = transform;
        CreateBullets();
        m_audioManager = FindObjectOfType<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (b_bulletShooted)
        {
            f_currentTime += Time.deltaTime;

            if(f_currentTime >= SHOOT_DELAY)
            {
                f_currentTime = 0;
                b_bulletShooted = false;
            }
        }
	}

    private void CreateBullets()
    {
        m_bullets = new Bullet[i_maxBullets];

        for(int i = 0; i < i_maxBullets; ++i)
        {
            m_bullets[i] = Instantiate(m_bulletGO, m_trans.position, m_trans.rotation).GetComponent<Bullet>();
            m_bullets[i].SetParent(m_bulletContainer);
        }
    }

    public void ShootBullet()
    {
        if (b_bulletShooted)
            return;

        if (i_currentBullet >= i_maxBullets)
            i_currentBullet = 0;

        m_bullets[i_currentBullet].ShootBullet(m_trans.position, m_trans.eulerAngles);
        b_bulletShooted = true;
        ++i_currentBullet;
        m_audioManager.PlayClipRandomPitch(_sfxIndex, 0.2f);
        //StartCoroutine(Reload());
    }

    public void SetContainer(Transform container)
    {
        m_bulletContainer = container;
    }
}

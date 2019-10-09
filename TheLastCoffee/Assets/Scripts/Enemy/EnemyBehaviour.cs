using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour {

    protected float f_health;
    [SerializeField]
    protected float f_maxHealth;

    [SerializeField]
    private GameObject m_hitBlood;
    [SerializeField]
    private GameObject m_deadBloor;
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;
    private bool b_alive;
    protected bool b_active = true;

    public Rect boundaries;

    private EnemyWave m_myWave;
    public EnemyWave Wave
    {
        set
        {
            m_myWave = value;
        }
    }

    private GameManager m_gameManager;
    private AudioManager m_audioManager;

    protected virtual void Start()
    {
        f_health = f_maxHealth;
        m_gameManager = FindObjectOfType<GameManager>();
        m_gameManager.OnGameFinished += OnGameFinished;
        m_audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        b_alive = true;
    }

    public virtual void GetDamage(float damage)
    {
        f_health -= damage;
        Debug.Log(f_health);
        GameObject _bh = Instantiate(m_hitBlood, transform.position, Quaternion.identity);
        Destroy(_bh, 0.7f);
        //Damage Animation
        
        if(f_health <= 0)
        {
            //Death
            b_alive = false;
            CancelInvoke();
            m_myWave.EnemyDead();
            gameObject.SetActive(false);
            GameObject _bd = Instantiate(m_deadBloor, transform.position, Quaternion.identity);
            Destroy(_bd, 10f);
            OnDie();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            Bullet _b = collision.transform.GetComponent<Bullet>();
            GetDamage(_b.Damage);
            _b.BulletHit();
            m_audioManager.PlayClipRandomPitch(3, 0.2F);
            if (b_alive)
            {
                m_spriteRenderer.color = Color.red;
                StartCoroutine(HitAnim());
            }
            _b.ResetPos();
        }
    }

    IEnumerator HitAnim()
    {
        yield return new WaitForSeconds(0.1f);
        m_spriteRenderer.color = Color.white;
    }

    private void OnGameFinished()
    {
        b_active = false;
    }

    protected virtual void OnDie()
    {

    }
}

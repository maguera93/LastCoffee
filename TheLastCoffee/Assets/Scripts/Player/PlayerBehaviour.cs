using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    private const float SHOOT_DELAY = 0.2F;

    [SerializeField]
    private float m_speed;
    private Transform m_trans;
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private GameObject m_bullet;
    private bool bulletShooted;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private Cartridge m_cartridge;
    [SerializeField]
    private float f_maxHealth;
    private float f_currentHealth;
    [SerializeField]
    private HUD m_hud;
    [SerializeField]
    private GameObject m_spriteGO;
    private bool invulnerable = false;
    private GameManager gameManager;
    private bool b_inmovilize = false;
    private AudioManager m_audioManager;

	// Use this for initialization
	void Start () {
        m_trans = transform;
        m_rigidbody = GetComponent<Rigidbody2D>();
        f_currentHealth = f_maxHealth;
        gameManager = FindObjectOfType<GameManager>();
        gameManager.OnGameFinished += InmovilizePlayer;
        m_audioManager = FindObjectOfType<AudioManager>();
        m_cartridge.SFXIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

    //Player move
    public void Move(Vector2 axis)
    {
        if (b_inmovilize)
            return;
        m_rigidbody.velocity = axis * m_speed * Time.deltaTime;

        if(axis.x != 0  || axis.y != 0)
            m_animator.Play("move");
        else
            m_animator.Play("idle_feed");

        //m_trans.Translate(axis * m_speed * Time.deltaTime, Space.World);
    }

    //Shoot rotation
    public void Shoot(Vector2 axis)
    {
        if (invulnerable)
            return;

        if (axis.y > 0)
        {
            m_trans.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
        }
        else if(axis.y < 0)
        {
            m_trans.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        }
        else
        {
            if (axis.x != 0f)
            {
                m_trans.rotation = Quaternion.AngleAxis(-Mathf.Sign(axis.x) * 90, Vector3.forward);
            }
        }

        if(Mathf.Abs(axis.x) == 1 || Mathf.Abs(axis.y) == 1)
        {
            m_cartridge.ShootBullet();
        }
    }

    private void GetDamage(float damage)
    {
        if (invulnerable)
            return;

        f_currentHealth -= damage;

        if(f_currentHealth <= 0)
        {
            f_currentHealth = 0;
            // Game Over
            gameManager.GameOver();
        }

        m_audioManager.PlayClipRandomPitch(2);

        m_hud.SetHealthbar(f_currentHealth / f_maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            GetDamage(7f);
            StartCoroutine(HitAnimation());
        }
        if (collision.tag == "EnemyBullet")
        {
            Bullet _b = collision.transform.GetComponent<Bullet>();
            GetDamage(_b.Damage);
            _b.BulletHit();
            StartCoroutine(HitAnimation());

        }
        if(collision.tag == "Coffee")
        {
            gameManager.GameCompleted();
        }
    }

    IEnumerator HitAnimation()
    {
        int count = 0;
        invulnerable = true;

        while (true)
        {
            m_spriteGO.SetActive(!m_spriteGO.activeInHierarchy);
            ++count;
            if (count > 3)
            {
                invulnerable = false;
                yield break;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void InmovilizePlayer()
    {
        b_inmovilize = true;
        m_rigidbody.velocity = Vector2.zero;
        m_animator.Play("idle_feed");
    }
}

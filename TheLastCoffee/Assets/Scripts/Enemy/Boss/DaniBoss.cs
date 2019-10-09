using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaniBoss : EnemyBehaviour {

    enum BossEnemyState { MOVING, CHARGE }

    BossEnemyState enemyState;

    private Vector3 randomPos;

    private Transform m_playerTransform;
    private Transform m_myTransform;
    [SerializeField]
    private Cartridge enemyCartridge;
    [SerializeField]
    private float f_speed;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private Image m_healthBar;
    [SerializeField]
    private GameObject m_coffe;

    // Use this for initialization
    private void Awake()
    {
        enemyCartridge.SetContainer(GameObject.FindGameObjectWithTag("EnemyBulletContainer").transform);
    }

    protected override void Start()
    {
        base.Start();
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!b_active)
            return;

        switch (enemyState)
        {
            case BossEnemyState.MOVING:
                {
                    MoveEnemy(new Vector3(boundaries.x, boundaries.y, 0));
                    //m_animator.Play("tower_move");
                    break;
                }
            case BossEnemyState.CHARGE:
                {
                    //Loock Player and release
                    LoockAtPlayer(m_playerTransform.position);
                    //m_animator.Play("tower_shoot");
                    break;
                }
        }
    }

    private void ChangeState()
    {
        switch (enemyState)
        {
            case BossEnemyState.MOVING:
                {
                    Invoke("ChangeState", 1f);
                    enemyState = BossEnemyState.CHARGE;
                    break;
                }
            case BossEnemyState.CHARGE:
                {
                    //Loock Player and release
                    Invoke("ChangeState", 1f);
                    enemyCartridge.ShootBullet();
                    break;
                }
        }
    }

    private void LoockAtPlayer(Vector3 targetPos)
    {
        Vector3 diff = targetPos - m_myTransform.position;
        diff.Normalize();
        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        m_myTransform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
    }

    private void MoveEnemy(Vector3 targetPos)
    {
        LoockAtPlayer(targetPos);

        m_myTransform.position = Vector3.MoveTowards(m_myTransform.position, targetPos, f_speed * Time.deltaTime);

        if (m_myTransform.position == targetPos)
        {
            //ChangeState
            ChangeState();
        }

    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        m_healthBar.fillAmount = f_health / f_maxHealth;
    }

    protected override void OnDie()
    {
        base.OnDie();
        Instantiate(m_coffe, transform.position, Quaternion.identity);
    }
}

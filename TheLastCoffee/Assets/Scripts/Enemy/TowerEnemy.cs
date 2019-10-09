using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemy : EnemyBehaviour {

    enum TowerEnemyState { MOVING, CHARGE, RELEASE }

    TowerEnemyState enemyState;

    private Vector3 randomPos;

    private Transform m_playerTransform;
    private Transform m_myTransform;
    [SerializeField]
    private Cartridge enemyCartridge;
    [SerializeField]
    private float f_speed;
    [SerializeField]
    private Animator m_animator;

    // Use this for initialization
    private void Awake()
    {
        enemyCartridge.SetContainer(GameObject.FindGameObjectWithTag("EnemyBulletContainer").transform);
    }

    protected override void Start () {
        base.Start();
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_myTransform = transform;
        randomPos = CalculateRandoPos();
        enemyCartridge.SFXIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (!b_active)
            return;

        switch (enemyState)
        {
            case TowerEnemyState.MOVING:
                {
                    MoveEnemy(randomPos);
                    m_animator.Play("tower_move");
                    break;
                }
            case TowerEnemyState.CHARGE:
                {
                    //Loock Player and release
                    LoockAtPlayer(m_playerTransform.position);
                    m_animator.Play("tower_shoot");
                    break;
                }
            case TowerEnemyState.RELEASE:
                {
                    // Wait to move
                    m_animator.Play("tower_shoot-release");
                    break;
                }
        }
	}

    private Vector3 CalculateRandoPos()
    {
        return new Vector3(Random.Range(boundaries.x - boundaries.width / 2, boundaries.x + boundaries.width / 2),
            Random.Range(boundaries.y - boundaries.height / 2, boundaries.y + boundaries.height / 2), 0);
    }

    private void ChangeState()
    {
        switch (enemyState)
        {
            case TowerEnemyState.MOVING:
                {
                    Invoke("ChangeState", 1f);
                    enemyState = TowerEnemyState.CHARGE;
                    break;
                }
            case TowerEnemyState.CHARGE:
                {
                    //Loock Player and release
                    enemyState = TowerEnemyState.RELEASE;
                    enemyCartridge.ShootBullet();
                    Invoke("ChangeState", 1f);
                    break;
                }
            case TowerEnemyState.RELEASE:
                {
                    // Wait to move
                    randomPos = CalculateRandoPos();
                    enemyState = TowerEnemyState.MOVING;
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
}

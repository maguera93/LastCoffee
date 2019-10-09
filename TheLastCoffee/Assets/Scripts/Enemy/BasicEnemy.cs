using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBehaviour {

    public enum Direction { TOPLAYER, TORANDOM }

    Direction direction = Direction.TORANDOM;

    private Vector3 randomPos;

    private Transform m_playerTransform;
    private Transform m_myTransform;
    [SerializeField]
    private float f_speed;

    private float timeCounter;
    public float timeMax, timeMin;
    private float timeToChange;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_myTransform = transform;
        CalculateRandoPos();
	}
	
	// Update is called once per frame
	void Update () {
        if (!b_active)
            return;

        switch (direction)
        {
            case Direction.TORANDOM:
                {
                    MoveEnemy(randomPos);
                    break;
                }
            case Direction.TOPLAYER:
                {
                    MoveEnemy(m_playerTransform.position);
                    break;
                }
        }

        timeCounter += Time.deltaTime;
        if(timeCounter >= timeToChange)
        {
            ChangeTarget();
        }

	}

    private Vector3 CalculateRandoPos()
    {
        return new Vector3(Random.Range(boundaries.x - boundaries.width / 2, boundaries.x + boundaries.width / 2),
            Random.Range(boundaries.y - boundaries.height / 2, boundaries.y + boundaries.height / 2), 0);
    }

    private void ChangeTarget()
    {
        timeCounter = 0;
        timeToChange = Random.Range(timeMin, timeMax);
        direction = (direction == Direction.TOPLAYER) ? Direction.TORANDOM : Direction.TOPLAYER;
        randomPos = CalculateRandoPos();
    }

    private void MoveEnemy(Vector3 targetPos)
    {
        Vector3 diff = targetPos - m_myTransform.position;
        diff.Normalize();
        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        m_myTransform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

        m_myTransform.position = Vector3.MoveTowards(m_myTransform.position, targetPos, f_speed * Time.deltaTime);

        if(m_myTransform.position == targetPos)
        {
            ChangeTarget();
        }

    }

}

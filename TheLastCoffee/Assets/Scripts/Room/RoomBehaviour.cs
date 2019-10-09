using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

    CameraMove m_mainCamera;

    [SerializeField]
    private SpawnWave[] spawnners;
    public MiniMapCell mapCell;

    private Queue<SpawnWave> spawnQueue;
    private int i_currentWave;
    private bool b_isSpawning;
    private bool b_active = true;

    [Header("Door Data")]
    public DoorData doorUp;
    public DoorData doorLeft;
    public DoorData doorRight;
    public DoorData doorDown;

	// Use this for initialization
	void Awake () {
        m_mainCamera = Camera.main.GetComponent<CameraMove>();

        if (spawnners.Length > 0)
        {
            for (int i = 0; i < spawnners.Length; ++i)
            {
                spawnners[i].roomBehaviour = this;
                spawnners[i].Initialize();
                //spawnQueue.Enqueue(spawnners[i]);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_mainCamera.StartMove(transform.position);

            if(mapCell != null)
                mapCell.RoomEntered();

            if (!b_active)
                return;

            if (spawnners.Length > 0)
            {
                b_isSpawning = true;
                b_active = false;
                Invoke("NextWave", 0.5f);
                CloseDoor();

            }
            else
            {
                CloseDoor();
                OpenDoors();
            }
            //GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            mapCell.RoomExit();
        }
    }

    private void OpenDoors()
    {
        if (doorUp.closeNeighbour)
            doorUp.door.OpenDoorAnim(true);
        if (doorRight.closeNeighbour)
            doorRight.door.OpenDoorAnim(true);
        if (doorDown.closeNeighbour)
            doorDown.door.OpenDoorAnim(true);
        if (doorLeft.closeNeighbour)
            doorLeft.door.OpenDoorAnim(true);

        b_active = false;
    }

    private void CloseDoor()
    {

            doorUp.door.CloseDoorAnim(true);
            doorRight.door.CloseDoorAnim(true);
            doorDown.door.CloseDoorAnim(true);
            doorLeft.door.CloseDoorAnim(true);
    }

    public void NextWave()
    {
        
        Debug.Log(string.Concat("Current Wave: ", i_currentWave));

        if (i_currentWave >= spawnners.Length)
        {
            OpenDoors();
            Debug.Log("Room complete");
            return;
        }

        spawnners[i_currentWave].StartWave();
        i_currentWave++;
    }
}

[System.Serializable]
public struct DoorData
{
    public bool closeNeighbour;
    public Door door;
}

[System.Serializable]
public class SpawnWave
{
    public Spawnner[] spawnners;
    public int currentSpawn;
    public RoomBehaviour roomBehaviour;
    public int spawnsCount;
    public int spawnsFinished;

    public void Initialize()
    {
        for (int i = 0; i < spawnners.Length; ++i)
        {
            spawnners[i].myWave = this;
        }
    }

    public void StartWave()
    {
        if (currentSpawn == spawnners.Length)
            return;
        spawnners[currentSpawn].StartSpawn();
        currentSpawn++;

        StartWave();

    }

    public void NextWave()
    {
        Debug.Log("Nada de nada");
        spawnsFinished++;
        if(spawnsFinished >= spawnners.Length)
            roomBehaviour.NextWave();
    }
}
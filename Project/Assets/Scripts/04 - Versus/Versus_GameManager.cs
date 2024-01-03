using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using UnityEngine.UIElements;

public class Versus_GameManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    public int[] nbOfEnemyByWaves;

    private int currentWave;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject enemyBoss;
    private bool doOnce;

    [SerializeField]
    private GameObject[] plateforms;

    [SerializeField]
    private Transform playerSpawnPoint;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject player;

    private void Start()
    {
        RespawnPlayer();
    }

    private void Update()
    {
        if (currentWave < nbOfEnemyByWaves.Length)
        {
            if (FindObjectsOfType<EnemyController_Clone>().Length <= 0)
            {
                for (int i = 0; i < nbOfEnemyByWaves[currentWave]; i++)
                {
                    Transform chooseSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                    Instantiate(enemyPrefab, chooseSpawnPoint);
                }

                currentWave += 1;
            }
        }
        else
        {
            if (FindObjectsOfType<EnemyController_Clone>().Length <= 0)
            {
                if (!doOnce)
                {
                    foreach(GameObject go in plateforms)
                    {
                        go.GetComponent<Animator>().SetTrigger("Fall");
                    }

                    doOnce = true;

                    Instantiate(enemyBoss, spawnPoints[1]);


                }
            }
        }
    }

    public void RespawnPlayer()
    {
        int actualLife = 3;
        if (player != null)
        {

             actualLife = player.GetComponent<Versus_LifeController>().life;

            Destroy(player.GetComponent<PlayerController>()._functionnal.playerCamera);
            Destroy(player);
        }

        player = Instantiate(playerPrefab);
        player.transform.position = playerSpawnPoint.position;
        player.GetComponent<Versus_LifeController>().life = actualLife;
    }
}
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //wandering
    public Vector3 walkingPoint;
    bool walkPointSet;
    public float walkingRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //ranges
    public float sightRange, attackRange;
    public bool playerInRange, playerInAttackRange;

    // gun/shooting

    public int maxAmmo = 12;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    public float recoilAngle = 1;
    public float maxDistance = 50;

    public RaycastHit rayHit;

    public GameObject wound;

    //health

    public int hp = 10;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //is in attack range?:
        playerInRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInRange && !playerInAttackRange) Wandering();
        if (playerInRange && !playerInAttackRange) ChasePlayer();
        if (playerInRange && playerInAttackRange) AttackPlayer();

    }

    private void Wandering()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkingPoint);

        Vector3 distanceToWalkPoint = transform.position - walkingPoint;

        //walkingpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkingRange, walkingRange);
        float randomX = Random.Range(-walkingRange, walkingRange);

        walkingPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkingPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //make enemy stop moving
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //attack script

            var offsetX = Random.Range(-recoilAngle, recoilAngle);
            var offsetY = Random.Range(-recoilAngle, recoilAngle);

            // Calculate the direction towards the player with the random offset
            Vector3 direction = (player.position + new Vector3(offsetX, offsetY, 0f)) - transform.position;

            // Shoot a raycast towards the player
            if (Physics.Raycast(transform.position, direction, out RaycastHit rayHit, Mathf.Infinity, whatIsPlayer))
            {
                Debug.Log(rayHit.collider.name);

                // Instantiate a bullet prefab at the hit point
                Instantiate(wound, rayHit.point, Quaternion.identity);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void TakeDamage(int damage)
    {

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Rigidbody rb;

    public Transform player;
    public Transform enemy;

    public LayerMask whatIsGround, whatIsPlayer;

    public PlayerShooting playerShooting;
    public PlayerHealth playerHealth;

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
    public int damage = 12;

    public RaycastHit rayHit;

    public GameObject wound;
    public GameObject bulletHole;

    public AudioSource audioSourceShoot;
    public AudioSource audioSourceReload;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    //public Animator walking;

    public int min;
    public int max;

    //health

    private Health health;

    public bool dead;
    public bool destroyOnDeath;
    public bool deletePieces;

    public GameObject enemyPrefab;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //playerShooting = GetComponent<PlayerShooting>();

        health = GetComponent<Health>();
        health.onDamage.AddListener(TakeDamage);
        health.onDeath.AddListener(Death);

        audioSourceShoot = GetComponent<AudioSource>();
        audioSourceReload = GetComponent<AudioSource>();

        dead = false;

        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        //is in attack range?:
        playerInRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInRange && !playerInAttackRange && !dead) Wandering();
        if (playerInRange && !playerInAttackRange && !dead) ChasePlayer();
        if (playerInRange && playerInAttackRange && !dead || health.currentHp < health.maxHp && playerInRange && playerInAttackRange && !dead) AttackPlayer();

        if (currentAmmo <= 0) Invoke("Reload", 2);

    }

    private void Wandering()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkingPoint);

        //walking.pl

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

        if (!alreadyAttacked && currentAmmo > 0)
        {
            //attack script

            var offsetX = Random.Range(-recoilAngle, recoilAngle);
            var offsetY = Random.Range(-recoilAngle, recoilAngle);

            currentAmmo--;
            audioSourceShoot.PlayOneShot(shootSound, 0.7f);
            audioSourceReload.PlayOneShot(reloadSound, 0.7f);

            // Calculate the direction towards the player with the random offset
            Vector3 direction = (player.position + new Vector3(offsetX, offsetY, 0f)) - transform.position;

            // Shoot a raycast towards the player
            if (Physics.Raycast(transform.position, direction, out RaycastHit rayHit, whatIsPlayer))
            {
                if (rayHit.collider.CompareTag("Player"))
                {
                    // Instantiate a bullet prefab at the hit point
                    Instantiate(wound, rayHit.point, Quaternion.identity);

                    if (playerHealth.currentHp > 0) playerHealth.TakeDamage();
                    if (playerHealth.currentHp <= 0) playerHealth.Death();
                }
                else
                {
                    var bullet = Instantiate(bulletHole, rayHit.point, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                    Destroy(bullet, 5);
                }
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void TakeDamage()
    {
        if (health.currentHp > 0) health.currentHp -= playerShooting.damage;    

        if (health.currentHp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        dead = true;

        if (destroyOnDeath)
        {
            int randomIndex = Random.Range(min, max);

            Destroy(gameObject);

            for (var i = 0; i < randomIndex; i++)
            {
                var enemyParts = Instantiate(enemyPrefab, enemy.transform.position, Quaternion.identity);
                if (deletePieces) Destroy(enemyParts, 12);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Reload()
    {
        currentAmmo = maxAmmo;
    }
}

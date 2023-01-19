

SOLUTIONS TO CHALLENGE 4:

***********(1)***********	3.Hitting an enemy sends it back towards you | When you hit an enemy, it should send it away from the player

Open PlayerControllerX.cs script..
Inside OnCollisionEnter(Collision other) func..

Replace or comment below line.. 
//Vector3 awayFromPlayer =  transform.position - other.gameObject.transform.position;

with..
Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;


***********(2)***********	4.A new wave spawns when the player gets a powerup | A new wave should spawn when all enemy balls have been removed

Open SpawnManagerX.cs script..
Inside Update() func..
Replace tag "Powerup" with "Enemy"..

enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

This will spawn new wave and reset player's position in-front of goal when enemy count is 0



***********(3)***********	5.The powerup never goes away | The powerup should only last for a certain duration, then disappear


Coroutine was never called.. although PowerupCooldown() func is available.. so I called it..
Inside PlayerControllerX.cs script..
Inside OnTriggerEnter(Collider other) func..

if (other.gameObject.CompareTag("Powerup"))
{
      Destroy(other.gameObject);
      hasPowerup = true;
      powerupIndicator.SetActive(true);
      StartCoroutine(PowerupCooldown());
}


***********(4)***********	6.2 enemies are spawned in every wave | One enemy should be spawned in wave 1, two in wave 2, three in wave 3, etc


For everyone who get 4 Enemies in wave 3, cut waveCount++ from SpawnEnemyWave and paste it in your Update Method.
void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
            waveCount++;
        }

    }


+++++++++++++++++++++++++++++++++++++++++++++++++
I was wondering what's the difference.. if I replace '2' in for loop with 'waveCount' or 'enemiesToSpawn'.. both do the same job!

void SpawnEnemyWave(int enemiesToSpawn)
    {
        ...
        for (int i = 0; i < waveCount; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
        waveCount++;
        ...
    }


***********(5)***********	7.The enemy balls are not moving anywhere | The enemy balls should go towards the “Player Goal” object


in the EnemyX.cs scripts add below code and don't forget check speed in enemy prefabs in Project window :

public float speed = 100.f;
void Start()
...
        playerGoal = GameObject.Find("Player Goal");
++++++++++++++++++++++++++++++++++++++++++++

JeremyFang
3 days ago
private float speed = 5.0f;

0 
Reply


JeremyFang
3 days ago
playerGoal = GameObject.Find("Player");



***********(6)***********	8.Bonus: The player needs a turbo boost | The player should get a speed boost whenever the player presses spacebar - and a particle effect should appear when they use it 


PriyaRanjanPani
16 days ago
i find it easy and best this type ...ref (prototype 2)

 private float boostSpeed = 2000.0f;
 private float checkTime = 0.0f;
 private float spamDelay = 3.0f;

in update ()

 //set checktime
        if (checkTime < spamDelay)
        {
            checkTime += Time.deltaTime;
        }

        //Press spacebar & if checkTime >= spamDelay then call BoostTheBall() function

        if (Input.GetKey(KeyCode.Space) && checkTime >= spamDelay)
        {
            BoostTheBall();

        }

 // boostTheBall in forward direction with impulse force and reset check time to 0
    void BoostTheBall()
    {
        playerRb.AddForce(focalPoint.transform.forward * boostSpeed * Time.deltaTime, ForceMode.Impulse);
        boostParticle.GetComponent<ParticleSystem>().Play();
        checkTime = 0.0f;
    }


***********(7)***********	9.Bonus: The enemies never get more difficult | The enemies’ speed should increase in speed by a small amount with every new wave


My solution:
1. In SpawnManagerX.cs track and increase the enemy speed:
public float enemySpeed = 0;
void SpawnEnemyWave(int enemiesToSpawn)
....
enemySpeed += 100;
2. In EnemyX.cs reference that speed variable and set it in Start()
private SpawnManagerX spawnManagerScripts;
void Start()
...
spawnManagerScripts = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();
void Update()
...
enemyRb.AddForce(lookDirection * spawnManagerScripts.enemySpeed * Time.deltaTime);


***********(8)***********



***********(9)***********
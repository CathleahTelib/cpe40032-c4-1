using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 2000;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    public GameObject boostParticle; //hint 6 

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

	// hint 6: the player needs turbo boost
	private float boostSpeed = 2000.0f;
	private float checkTime = 0.0f;
	private float spamDelay = 3.0f;
	
	
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

		// hint 6: the player needs a boost
        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
		
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
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
			Destroy(other.gameObject);
			hasPowerup = true;
			powerupIndicator.SetActive(true);
			// hint 3: added the coroutine as solution 3
			StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            // Vector3 awayFromPlayer =  transform.position - other.gameObject.transform.position;
			// hint 1: replace with this
			Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }



}

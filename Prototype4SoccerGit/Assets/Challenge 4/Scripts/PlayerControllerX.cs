using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    public float dashSpeed = 20;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrengthHit = 10;
    private float powerupStrengthHit = 25;

    public ParticleSystem dashParticle;
    public float coolDown = 0.5f;
    public float nextDashTime = 0;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    private void Update()
    {        
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 
        
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
        
        if (Time.time > nextDashTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRb.AddForce(focalPoint.transform.forward * dashSpeed, ForceMode.Impulse);
                dashParticle.Play();
                nextDashTime = Time.time + coolDown;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine (PowerupCooldown());
        }
    }
    
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer =  other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup)
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrengthHit, ForceMode.Impulse);
            }
            else
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrengthHit, ForceMode.Impulse);
            }
        }
    }
}

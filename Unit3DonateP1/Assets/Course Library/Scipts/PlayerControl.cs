using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    public ParticleSystem dirtParticle;
    private Animator playerAnim;
    // Start is called before the first frame update
    public ParticleSystem explosionParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    public bool doubleJumpUsed = false;
    public float doubleJumpForce;
    public bool doubleSpeed = false;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            doubleSpeed = true;
            playerAnim.SetFloat("Speed_Mutipler", 2.0f);
        }
        else if (doubleSpeed)
        {
            doubleSpeed = false;
            playerAnim.SetFloat("Speed_Mutipler", 1.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)  
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            doubleJumpUsed = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space)&& !isOnGround && !doubleJumpUsed)
        {
            doubleJumpUsed = true;
            playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
          
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

    }
    

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
       else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("GAME OVER!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }
    
}

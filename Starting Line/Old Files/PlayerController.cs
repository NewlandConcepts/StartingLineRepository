using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveForce = 365f;					//Amount of force added to move the player left and right
	public float maxSpeed = 5f;						//The fastest the player can travel in the x axis

	public bool jump = false;						//Condition for whether the player should jump
	public float jumpForce = 1000f;					//Amount of force added when the player jumps

	private Transform groundCheck;					//A position marking where to check if the player is grounded.
	private bool grounded = false; 					//Whether or not the player is grounded

    private Animator anim;                          //Reference to the player's animator component

    public bool attack = false;                     //Condition for whether the player should attack. 
	
    
    // Use this for initialization
	void Awake () {
		//setting up references
		groundCheck = transform.Find ("groundCheck");
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer
		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		// If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }

        if (Input.GetButtonDown("Attack"))
        {
            attack = true;
        }
        if(Input.GetButtonUp("Attack"))
        {
            attack = false;
        }

        if (Input.GetButtonDown("Debug"))
        {
            DebugPause();
        }
        //Cache the horizontal input
        float h = Input.GetAxis("Horizontal");

        //The SpeedParam animator parameter is set to the absolute value of the horizontal input. 
        anim.SetFloat("SpeedParam", Mathf.Abs(h));

    }

    // Mostly used for adding forces at the moment. 
	void FixedUpdate(){
        // A check to call the attack method and reinstate the gravity. 
        if (!attack)
        {
            Shoot(attack);
        }

        //If the player is attacking, we want to stop all their current momentum.
        if (attack)
        {
            Shoot(attack);
        }
        if (attack)
        { }
        else
        {
            //Cache the horizontal input
            float h = Input.GetAxis("Horizontal"); // GetAxis returns 1 and -1

            //The SpeedParam animator parameter is set to the absolute value of the horizontal input. 
            //anim.SetFloat("SpeedParam", Mathf.Abs(h));

            //If the player is changing direction (h has a different sign to velocity.x) or hasn't reached max speed yet...
            if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            {

                //... add a force to the player
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
            }
            //If the player's horizontal velocity is greater than the maxSpeed
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            {

                //...set the player's velocity to the maxSpeed in the x axis
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }

            //If the player should jump...
            if (jump)
            {

                //Add a vertical force to the player
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

                //Make sure the player can't jump again until the jump conditions from Update are satisfied
                jump = false;
            }
        }
	}

    // Run the attacking sequence. 
    void Shoot(bool isAttacking)
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

        if (isAttacking)
        {
            //Set both velocity and angularVelocity to 0f
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.GetComponent<Rigidbody2D>().angularVelocity = 0f;

            //The effect of gravity on the character must also be set to zero. However this value must be changed back to 1 when the character is done attacking. 	
            this.GetComponent<Rigidbody2D>().gravityScale = 0f;

            //Make sure the player can't attack again until the attack conditions from Update are satisfied?
        }
        else
        {
            //Reinstate the gravity if the player is no longer attacking.
            this.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }           
    }


	//A simple break used to stop the program and find the values of certain variables. 
    void DebugPause()
    {
		//Makes a Rigidbody2D
		Rigidbody2D test = this.GetComponent<Rigidbody2D> ();
		//Sets a variable float to the default gravity scale (Which is 1). 
		float debug = test.gravityScale;  
		//Adds a breakpoint. 
		string falseBreakpoint = "Whatever"; 
    }

}

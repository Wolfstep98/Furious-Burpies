using System;
using System.Collections;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour, IStickBehaviour, IBounceBehaviour, IFrictionBehaviour
{
    #region Fields & Properties
    [Header("States")]
    [SerializeField]
    private bool isGrounded = false;
    /// <summary>
    /// Is the character grounded ?
    /// </summary>
    public bool IsGrounded { get { return this.isGrounded; } }

    [SerializeField]
    private bool isStick = false;
    /// <summary>
    /// Is the character stick ?
    /// </summary>
    public bool IsStick { get { return this.isStick; } }

    [Header("Gravity")]
    [SerializeField]
    private bool gravityEnabled = true;
    /// <summary>
    /// Is the gravity enabled ?
    /// </summary>
    public bool GravityEnabled { get { return this.gravityEnabled; } }

    [SerializeField]
    private float gravityForce = 9.81f;
    /// <summary>
    /// The current gravity that affects the character.
    /// </summary>
    public float GravityForce { get { return this.gravityForce; } set { this.gravityForce = value; } }

    [SerializeField]
    private float gravityMaxVelocity = 20.0f;
    /// <summary>
    /// The max force the gravity can be.
    /// </summary>
    public float GravityMaxVelocity { get { return this.gravityMaxVelocity; } }

    [SerializeField]
    private float gravityModifier = 1.0f;
    /// <summary>
    /// The current modifier applied to gravity.
    /// </summary>
    public float GravityModifier { get { return this.gravityModifier; } }

    [SerializeField]
    private Vector3 gravityApplied = Vector3.zero;
    /// <summary>
    /// The current velocity of the character.
    /// </summary>
    public Vector2 GravityApplied { get { return this.gravityApplied; } set { this.gravityApplied = value; } }

    [Header("Velocity")]
    [SerializeField]
    private Vector3 finalDirection = Vector3.zero;
    public Vector3 FinalDirection { get { return this.finalDirection; } }

    [SerializeField]
    private Vector3 direction = Vector3.zero;
    public Vector3 Direction { get { return this.direction; } }

    [Header("Stick behaviour")]
    [SerializeField]
    private float stickTime = 0.0f;
    [SerializeField]
    private float stickBehaviourTime = 1.5f;

    [Header("References")]
    [SerializeField]
    private CharacterController characterController = null;
    #endregion

    #region Methods

    #region Initialization
    private void Awake()
    {
        this.Initialize();
    }

    private void Initialize()
    {
#if UNITY_EDITOR
        if (this.characterController == null)
            Debug.LogError("[Missing Reference] characterController not set !");
#endif
    }
    #endregion

    #region MonoBehaviour
    private void Update()
    {
        //Test Better Behaviour
        this.UpdateGravity();

        Vector3 finalDirection = this.direction + this.gravityApplied;
        this.characterController.Move(finalDirection * GameTime.deltaTime);
    }
    #endregion

    private void UpdateGravity()
    {
        if (!this.gravityEnabled)
            return;

        if(!this.characterController.isGrounded && !this.isStick)
        {
            this.gravityApplied = new Vector3(0.0f, this.gravityApplied.y - this.gravityForce * this.gravityModifier * GameTime.deltaTime, 0.0f);

            if(this.gravityApplied.y < -this.gravityMaxVelocity)
            {
                this.gravityApplied = new Vector3(0.0f, -this.gravityMaxVelocity, 0.0f);
            }
        }
        else
        {
            this.gravityApplied = Vector3.zero;
        }
    }

    public void Bounce(Vector2 frictionCoef, Vector2 normal)
    {
        //Vector2 vel = this.characterController.velocity;

        //Vector2 result = vel - (2 * (Vector2.Dot(vel, normal)) * normal);

        //result *= frictionCoef;

        //this.characterController.Move(result);

        //Debug.Log("vector : " + vel + " | result : " + result);
    }

    public void UpdateDirection(Vector3 direction, float forceApplied)
    {
        this.direction = direction * forceApplied;
    }

    public void Stick(Vector3 hitNormal)
    {
        int normalX = Mathf.RoundToInt(hitNormal.x);
        int normalY = Mathf.RoundToInt(hitNormal.y);
        
        if(Math.Abs(normalX) == 1 || normalY == -1)
        {
            this.isStick = true;
        }
        if(normalY == 1)
        {
            this.isGrounded = true;
        }

        this.direction = Vector3.zero;
    }

    private void UpdateStick()
    {
        if (this.isStick)
        {
            this.stickTime += GameTime.deltaTime;
            if (this.stickTime >= this.stickBehaviourTime)
            {
                this.isStick = false;
                this.stickTime = 0.0f;
            }
        }
    }
    public void Friction(Vector2 frictionCoef)
    {
        throw new NotImplementedException();
    }

    public void CatapultFromGround()
    {
        this.transform.parent = null;
        this.isGrounded = false;
        this.isStick = false;
    }
    #endregion
}

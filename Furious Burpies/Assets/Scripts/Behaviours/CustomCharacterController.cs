using System;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{
    #region Fields & Properties
    [Header("Gravity")]
    //[SerializeField]
    //private bool isGrounded = false;
    ///// <summary>
    ///// Is the character grounded ?
    ///// </summary>
    //public bool IsGrounded { get { return this.isGrounded; } }

    [SerializeField]
    private bool gravityEnabled = true;
    /// <summary>
    /// Is the gravity enabled ?
    /// </summary>
    public bool GravityEnabled { get { return this.gravityEnabled; } }

    [SerializeField]
    private float gravity = -9.81f;
    /// <summary>
    /// The current gravity that affects the character.
    /// </summary>
    public float Gravity { get { return this.gravity; } set { this.gravity = value; } }

    [Header("Velocity")]
    [SerializeField]
    private Vector3 gravityApplied = Vector3.zero;
    /// <summary>
    /// The current velocity of the character.
    /// </summary>
    public Vector2 GravityApplied { get { return this.gravityApplied; } set { this.gravityApplied = value; } }

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
        if(!this.characterController.isGrounded)
        {
            this.ApplyGravity();
        }
        else
        {
            this.gravityApplied = Vector3.zero;
        }
    }
    #endregion

    private void ApplyGravity()
    {
        if (!this.gravityEnabled)
            return;

        this.gravityApplied += new Vector3(0.0f, this.gravity * GameTime.deltaTime, 0.0f);
        Mathf.Clamp(this.gravityApplied.y, this.gravity, 0.0f);

        this.characterController.Move(this.gravityApplied * GameTime.deltaTime);
    }

    public void Bounce(Vector2 frictionCoef, Vector2 normal)
    {
        //Vector2 vel = this.characterController.velocity;

        //Vector2 result = vel - (2 * (Vector2.Dot(vel, normal)) * normal);

        //result *= frictionCoef;

        //this.characterController.Move(result);

        //Debug.Log("vector : " + vel + " | result : " + result);
    }
    #endregion
}

using System;
using UnityEngine;

public class CustomRigidbody2D : MonoBehaviour
{
    #region Fields & Properties
    [Header("Properties")]
    [SerializeField]
    private bool isMoving = false;
    public bool IsMoving { get { return this.isMoving; } set { this.isMoving = value; } }

    [SerializeField]
    private bool isGrounded = false;
    public bool IsGrounded { get { return this.isGrounded; } set { this.isGrounded = value; } }

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float maxGravity = -9.81f;

    [SerializeField]
    private Vector2 velocity = Vector2.zero;
    public Vector2 Velocity { get { return this.velocity; } set { this.velocity = value; } }

    [Header("References")]
    [SerializeField]
    new private Rigidbody2D rigidbody2D = null;
    #endregion

    #region Methods
    private void Awake()
    {
        this.isMoving = true;
        this.isGrounded = false;
    }

    private void Update()
    {
        Vector2 currentPosition = this.rigidbody2D.position;

        if (!this.isGrounded)
        {
            this.velocity.y += this.gravity * GameTime.deltaTime;
            this.velocity.y = Mathf.Clamp(this.velocity.y, this.maxGravity, 1000.0f);
        }

        currentPosition += this.velocity * GameTime.deltaTime;

        if (this.velocity == Vector2.zero)
            this.isMoving = false;
        else
            this.isMoving = true;

        this.rigidbody2D.MovePosition(currentPosition);
    }

    #region Catapult
    public void CatapultFromGround()
    {
        this.isMoving = true;
        this.isGrounded = false;
    }
    #endregion

    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GameObjectsTags.Wall)
        {
            int normalY = Mathf.RoundToInt(collision.contacts[0].normal.y);
            int normalX = Mathf.RoundToInt(collision.contacts[0].normal.x);
            if (normalY == 1)
            {
                this.isGrounded = true;
                this.velocity.y = 0.0f;
            }
            if(Mathf.Abs(normalX) == 1)
            {
                this.velocity.x = 0.0f;
            }
        }
    }
    #endregion
    #endregion
}

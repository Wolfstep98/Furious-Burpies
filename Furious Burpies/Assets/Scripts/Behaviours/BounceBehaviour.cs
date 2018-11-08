using System;
using UnityEngine;

public interface IBounceBehaviour
{
    void Bounce(IBounceProperty property);
}

/*
[RequireComponent(typeof(Rigidbody2D))]
public class BounceBehaviour : MonoBehaviour, IBounceBehaviour
{
    #region Fields & Properties
    [Header("Properties")]
    [SerializeField]
    private float bounceThreshold = 1.0f;

    [SerializeField]
    private Vector2 velocity = Vector2.zero;

    [Header("References")]
    [SerializeField]
    private CustomRigidbody2D customRigidbody2D = null;
    [SerializeField]
    new private Rigidbody2D rigidbody2D = null;

    #endregion

    #region Methods
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.rigidbody2D == null)
            Debug.LogError("[Missing references] - rigidbody2D not set !");
        if (this.customRigidbody2D == null)
            Debug.LogError("[Missing references] - customRigidbody2D not set !");
#endif
    }

    private void FixedUpdate()
    {
        //this.velocity = this.rigidbody2D.velocity;
        this.velocity = this.customRigidbody2D.Velocity;
    }

    public void Bounce(Vector2 bounceCoef, Vector2 normal)
    {
        Vector2 vel = this.customRigidbody2D.Velocity;

        Vector2 result = vel - (2 * (Vector2.Dot(vel, normal)) * normal);

        if (Mathf.RoundToInt(normal.y) == 1)
        {
            if (vel.y > -this.bounceThreshold)
            {
                Debug.Log("STOP");
                result = Vector2.zero;
                this.customRigidbody2D.IsGrounded = true;
            }
            else
            {
                result *= bounceCoef;
                Debug.Log("Bounce Ground");
            }
        }
        else if(Mathf.RoundToInt(normal.y) == -1)
        {
            result *= bounceCoef;
            Debug.Log("Bounce top");
        }

        if(result.y == 0)
        {
            this.customRigidbody2D.IsGrounded = true;
        }

        this.customRigidbody2D.Velocity = result;

        Debug.Log("vector : " + vel + " | result : " + result);
    }
    #endregion
}*/

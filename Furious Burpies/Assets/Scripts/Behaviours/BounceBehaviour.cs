using System;
using UnityEngine;

public interface IBounceBehaviour
{
    void Bounce(Vector2 frictionCoef, Vector2 normal);
}

[RequireComponent(typeof(Rigidbody2D))]
public class BounceBehaviour : MonoBehaviour, IBounceBehaviour
{
    #region Fields & Properties
    [Header("References")]
    [SerializeField]
    new private Rigidbody2D rigidbody2D = null;

    [SerializeField]
    private Vector2 velocity = Vector2.zero;
    #endregion

    #region Methods
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.rigidbody2D == null)
            Debug.LogError("[Missing references] - rigidbody2D not set !");
#endif
    }

    private void FixedUpdate()
    {
        this.velocity = this.rigidbody2D.velocity;
    }

    public void Bounce(Vector2 frictionCoef, Vector2 normal)
    {
        Vector2 vel = this.velocity;
        
        Vector2 result = vel - (2 * (Vector2.Dot(vel, normal)) * normal);

        result *= frictionCoef;

        this.rigidbody2D.velocity = result;

        Debug.Log("vector : " + vel + " | result : " + result);
    }
    #endregion
}

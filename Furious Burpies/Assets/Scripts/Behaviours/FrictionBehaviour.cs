using System;
using UnityEngine;

public interface IFrictionBehaviour
{
    void Friction(Vector2 frictionCoef);
}

public class FrictionBehaviour : MonoBehaviour, IFrictionBehaviour
{
    #region Fields & Properties
    [Header("Properties")]
    [SerializeField]
    private float frictionThreshold = 1.0f;

    [Header("References")]
    [SerializeField]
    private CustomRigidbody2D customRigidbody2D = null;


    #endregion

    #region Methods
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.customRigidbody2D == null)
            Debug.LogError("[Missing references] - customRigidbody2D not set !");
#endif
    }

    public void Friction(Vector2 frictionCoef)
    {
        if (this.customRigidbody2D.IsMoving)
        {
            Vector2 vel = this.customRigidbody2D.Velocity;

            if(vel.x > 0)
                vel -= frictionCoef * GameTime.deltaTime;
            else
                vel += frictionCoef * GameTime.deltaTime;

            if (Mathf.Abs(vel.x) < this.frictionThreshold)
                vel.x = 0.0f;

            this.customRigidbody2D.Velocity = vel;
        }
    }

    #endregion

}

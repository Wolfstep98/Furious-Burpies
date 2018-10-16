using System;
using UnityEngine;

public class BounceProperty : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private Vector2 bounceFrictionCoef = new Vector2(0.9f, 0.9f);

    #endregion

    #region Methods
    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GameObjectsTags.Player)
        {
            IBounceBehaviour bounceBehaviour = collision.gameObject.GetComponent<BounceBehaviour>();
            bounceBehaviour.Bounce(this.bounceFrictionCoef, -collision.contacts[0].normal);
            Debug.Log("BOUNCE !");
        }
    }
    #endregion
    #endregion
}

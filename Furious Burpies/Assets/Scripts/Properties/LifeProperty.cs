using System;
using UnityEngine;

public interface ILifeProperty
{
    //Properties
    int Life { get; }

    //Methods
    void TakeDamage(int amount);
    void AddLife(int amount);
}

public class LifeProperty : MonoBehaviour, ILifeProperty
{
    #region Fields & Properties
    [Header("Properties")]
    [SerializeField]
    private int life = 0;
    public int Life
    {
        get
        {
            return this.life;
        }
    }

    [SerializeField]
    private int maxLife = 10;
    [SerializeField]
    private int minLife = 0;

    [Header("References")]
    [SerializeField]
    private LifePropertyUI propertyUI = null;
    #endregion

    #region Methods
    private void Awake()
    {
        this.propertyUI.UpdateUI();
    }

    public void AddLife(int amount)
    {
        this.life += amount;

        if (this.life > this.maxLife)
            this.life = this.maxLife;

        this.propertyUI.UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        this.life -= amount;
        if (this.life < this.minLife)
            this.life = this.minLife;

        this.propertyUI.UpdateUI();
    }
    #endregion
}

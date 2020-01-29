using System;
using UnityEngine;
using UnityEngine.UI;

public class LifePropertyUI : MonoBehaviour
{
    #region Fields & Properties
    [SerializeField]
    private Text text_life = null;

    [SerializeField]
    private LifeProperty lifeProperty = null;
    #endregion

    #region Methods
    public void UpdateUI()
    {
        int life = this.lifeProperty.Life;

        this.text_life.text = life.ToString();
    }
    #endregion
}

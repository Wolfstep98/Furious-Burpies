using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SliderToText : MonoBehaviour
{
    [SerializeField]
    private Text text = null;

    private void Awake()
    {
        if(this.text == null)
            this.text = this.gameObject.GetComponent<Text>();
    }

    public void UpdateTextWithSliderValue(float value)
    {
        this.text.text = value.ToString();
    }
}

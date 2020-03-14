using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelateCounter : MonoBehaviour
{
    public Transform scale;
    public float Value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Value = GetComponent<Slider>().value;
        scale.GetComponent<TextMeshProUGUI>().text = Value.ToString() + " / 10";
    }
}

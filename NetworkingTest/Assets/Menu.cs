using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button testButton;

    // Update is called once per frame
    void OnEnable()
    {
        testButton.onClick.AddListener(delegate { GenerateKeyCode(); });
    }

    private void GenerateKeyCode()
    {
        float[] newCode = {Random.Range(0, 10),
                           Random.Range(0, 10),
                            Random.Range(0, 10),
                            Random.Range(0, 10)};


        Debug.Log(newCode[0]);

        //return newCode;
    }
}

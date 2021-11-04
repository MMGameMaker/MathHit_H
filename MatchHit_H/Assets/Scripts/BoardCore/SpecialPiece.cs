using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialPiece : MonoBehaviour
{
    private int specialValue;

    public int SpecialValue 
    { 
        get => this.specialValue; 
        set 
        {
            this.specialValue = value;
            valueText.text = "x " + specialValue.ToString();
        }
    }

    [SerializeField]
    private TextMesh valueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

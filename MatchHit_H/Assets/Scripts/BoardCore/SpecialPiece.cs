using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialPiece : MonoBehaviour
{
    private GamePiece piece;

    private int specialValue;

    [SerializeField]
    private SpriteRenderer sunRayBG;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private SpriteRenderer sungRingBG;

    public Animator animator;

    public AnimationClip matchedClip;

    private SpriteRenderer sprite;


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

    private void Awake()
    {
        piece = GetComponent<GamePiece>();

        sprite = GetComponent<SpriteRenderer>();
    }

    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void ChangeToInactiveSprite()
    {
        this.sprite.color = Color.grey;
    }

    public void ChangeToNormalSprite()
    {
        this.sprite.color = Color.white;
    }



}

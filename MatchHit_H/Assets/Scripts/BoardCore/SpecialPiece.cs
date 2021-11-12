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
    }

    


    // Start is called before the first frame update
    void Start()
    {
        piece.OnPieceMatchedHandler += OnSpecialMatchedHandler;

        piece.OnClearMatchedHandler += OnClearMatchedHandler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpecialMatchedHandler()
    {
        piece.transform.localScale = new Vector3(0.4f, 0.4f, 0);
    }

    public void OnClearMatchedHandler()
    {
        piece.transform.localScale = new Vector3(0.3f, 0.3f, 0);
    }

}

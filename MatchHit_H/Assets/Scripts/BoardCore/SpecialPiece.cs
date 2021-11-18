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
    private GameObject sunRayBG;

    [SerializeField]
    private float rotationSpeed;

    private float rotateAngle;

    [SerializeField]
    private SpriteRenderer sunRingBG;

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
        this.sunRayBG.transform.rotation = Quaternion.Euler(0, 0, SetRotateAngle()) ;
    }

    public void OnSpecialMatchedHandler()
    {
        ChangeToBigScale();
    }

    public void OnSpecialOutMatchedHandler()
    {
        ChangeToNormalScale();
    }

    public void ChangeToBigScale()
    {
        piece.transform.localScale = new Vector3(0.4f, 0.4f, 0);
    }

    public void ChangeToNormalScale()
    {
        piece.transform.localScale = new Vector3(0.3f, 0.3f, 0);
    }


    public void ChangeToInactiveSprite()
    {
        this.sprite.color = Color.grey;
    }

    public void ChangeToNormalSprite()
    {
        this.sprite.color = Color.white;
    }

    public float SetRotateAngle()
    {
        this.rotateAngle += rotationSpeed * Time.deltaTime;
        return rotateAngle;
    }

}

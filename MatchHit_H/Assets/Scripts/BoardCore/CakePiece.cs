using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CakePiece : MonoBehaviour
{
    public enum CakeType
    {
        PIZZARD,
        HOTDOG,
        CHIP,
        COLA,
        ROUNDCAKE,
    }
    
    [System.Serializable]
    public struct CakeSprite
    {
        public CakeType type;
        public Sprite sprite;
    }

    [System.Serializable]
    public struct CakeSpriteInactive
    {
        public CakeType type;
        public Sprite spriteInactive;
    }

    [System.Serializable]
    public struct CakeSpriteLightBG
    {
        public CakeType type;
        public Sprite spriteLightBG;
    }

    [System.Serializable]
    public struct CircleBGColor
    {
        public CakeType type;
        public Color circleBGColor;
    }

    public CakeSprite[] cakeTypes;

    public CakeSpriteInactive[] inactiveCakeTypes;

    public CakeSpriteLightBG[] LightBGTypes;

    public CircleBGColor[] circleBGTypes;

    private CakeType type;

    public CakeType Type 
    {
        get { return type; }
        set { SetType(value); }
    }

    private GamePiece piece;

    private SpriteRenderer sprite;

    [SerializeField]
    public SpriteRenderer lightBGSprite;

    [SerializeField]
    public SpriteRenderer Circle_blur;

    private Dictionary<CakeType, Sprite> caketypeSpriteDict;

    private Dictionary<CakeType, Sprite> inactiveCakeTypesSpriteDict;

    private Dictionary<CakeType, Sprite> LightBGTypesDict;

    private Dictionary<CakeType, Color> circleBGTypesDict;

    public int NumCakeType
    {
        get { return cakeTypes.Length; }
    }

    

    [SerializeField]
    private ParticleSystem cakePartical;


    private void Awake()
    {
        piece = GetComponent<GamePiece>();

        sprite = GetComponent<SpriteRenderer>();

        // setup caketypeSpriteDict
        caketypeSpriteDict = new Dictionary<CakeType, Sprite>();

        for(int i=0; i<cakeTypes.Length; i++)
        {
            if (!caketypeSpriteDict.ContainsKey(cakeTypes[i].type))
            {
                caketypeSpriteDict.Add(cakeTypes[i].type, cakeTypes[i].sprite);
            }
        }

        // setup inactiveCakeTypesSpriteDict
        inactiveCakeTypesSpriteDict = new Dictionary<CakeType, Sprite>();

        for (int i = 0; i < inactiveCakeTypes.Length; i++)
        {
            if (!inactiveCakeTypesSpriteDict.ContainsKey(inactiveCakeTypes[i].type))
            {
                inactiveCakeTypesSpriteDict.Add(inactiveCakeTypes[i].type, inactiveCakeTypes[i].spriteInactive);
            }
        }

        // setup LightBGTypesDict
        LightBGTypesDict = new Dictionary<CakeType, Sprite>();

        for (int i = 0; i < LightBGTypes.Length; i++)
        {
            if (!LightBGTypesDict.ContainsKey(LightBGTypes[i].type))
            {
                LightBGTypesDict.Add(LightBGTypes[i].type, LightBGTypes[i].spriteLightBG);
            }
        }

        lightBGSprite.gameObject.SetActive(false);


        // setup circleBGTypesDict
        circleBGTypesDict = new Dictionary<CakeType, Color>();

        for (int i = 0; i < circleBGTypes.Length; i++)
        {
            if (!circleBGTypesDict.ContainsKey(circleBGTypes[i].type))
            {
                circleBGTypesDict.Add(circleBGTypes[i].type, circleBGTypes[i].circleBGColor);
            }
        }

        Circle_blur.gameObject.SetActive(false);

        cakePartical.gameObject.SetActive(false);

    }

    private void Start()
    {
        
    }

    

    public void SetType(CakeType _type)
    {
        this.type = _type;

        if (caketypeSpriteDict.ContainsKey(_type))
        {
            sprite.sprite = caketypeSpriteDict[_type];
        }

        if (LightBGTypesDict.ContainsKey(_type))
        {
            lightBGSprite.sprite = LightBGTypesDict[_type];
        }

        if (circleBGTypesDict.ContainsKey(_type))
        {
            Circle_blur.color = circleBGTypesDict[_type];
        }

    }

    public void ChangeToInactiveSprite()
    {
            this.sprite.sprite = inactiveCakeTypesSpriteDict[type];
    }

    public void ChangeToNormalSprite()
    {
        sprite.sprite = caketypeSpriteDict[type];
    }

    public void OnCakeMatchedHandler()
    {
        this.lightBGSprite.gameObject.SetActive(true);

        this.Circle_blur.gameObject.SetActive(true);

        ChangeToBigScale();

        cakePartical.gameObject.SetActive(true);

        cakePartical.Play();
    }

    public void OnCakeOutMatchedHandler()
    {
        this.lightBGSprite.gameObject.SetActive(false);

        this.Circle_blur.gameObject.SetActive(false);

        ChangeToNormalScale();

        cakePartical.gameObject.SetActive(false);

        cakePartical.Stop();
    }

    public void ChangeToSmallScale()
    {
        piece.transform.localScale = new Vector3(0.4f, 0.4f, 0);
    }

    public void ChangeToBigScale()
    {
        piece.transform.localScale = new Vector3(0.66f, 0.66f, 0);
    }

    public void ChangeToNormalScale()
    {
        piece.transform.localScale = new Vector3(0.6f, 0.6f, 0);
    }

}

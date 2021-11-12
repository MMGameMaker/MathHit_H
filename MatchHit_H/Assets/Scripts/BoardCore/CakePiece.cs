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

    public CakeSprite[] cakeTypes;

    public CakeSpriteInactive[] inactiveCakeTypes;

    public CakeSpriteLightBG[] LightBGTypes;

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

    private Dictionary<CakeType, Sprite> caketypeSpriteDict;

    private Dictionary<CakeType, Sprite> inactiveCakeTypesSpriteDict;

    private Dictionary<CakeType, Sprite> LightBGTypesDict;

    public int NumCakeType
    {
        get { return cakeTypes.Length; }
    }

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
    }

    private void Start()
    {
        piece.OnPieceMatchedHandler += OnCakeMatchedHandler;

        piece.OnClearMatchedHandler += OnClearMatchedHandler;
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
    }

    public void ChangeToInactiveSprite()
    {
        if(BoardManager.boardInstance.IsMatching && this.type != BoardManager.boardInstance.ListCakeType)
        {
            this.sprite.sprite = inactiveCakeTypesSpriteDict[type];
        }
    }

    public void ChangeToNormalSprite()
    {
        sprite.sprite = caketypeSpriteDict[type];
    }

    public void OnCakeMatchedHandler()
    {
        this.lightBGSprite.gameObject.SetActive(true);

        piece.transform.localScale = new Vector3(0.75f, 0.75f, 0);
    }

    private void OnClearMatchedHandler()
    {
        this.lightBGSprite.gameObject.SetActive(false);

        piece.transform.localScale = new Vector3(0.6f, 0.6f, 0);
    }

}

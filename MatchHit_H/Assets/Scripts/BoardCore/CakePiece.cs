using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public CakeSprite[] cakeTypes;

    public CakeSpriteInactive[] inactiveCakeTypes;

    private CakeType type;

    public CakeType Type 
    {
        get { return type; }
        set { SetType(value); }
    }

    private GamePiece piece;

    private SpriteRenderer sprite;

    private Dictionary<CakeType, Sprite> caketypeSpriteDict;

    public int NumCakeType
    {
        get { return cakeTypes.Length; }
    }

    private Dictionary<CakeType, Sprite> inactiveCakeTypesSpriteDict;


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
    }


    public void SetType(CakeType _type)
    {
        this.type = _type;

        if (caketypeSpriteDict.ContainsKey(_type))
        {
            sprite.sprite = caketypeSpriteDict[_type];
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

}

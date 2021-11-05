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

        caketypeSpriteDict = new Dictionary<CakeType, Sprite>();

        for(int i=0; i<cakeTypes.Length; i++)
        {
            if (!caketypeSpriteDict.ContainsKey(cakeTypes[i].type))
            {
                caketypeSpriteDict.Add(cakeTypes[i].type, cakeTypes[i].sprite);
            }
        }

        inactiveCakeTypesSpriteDict = new Dictionary<CakeType, Sprite>();

        for (int i = 0; i < inactiveCakeTypes.Length; i++)
        {
            if (!inactiveCakeTypesSpriteDict.ContainsKey(inactiveCakeTypes[i].type))
            {
                inactiveCakeTypesSpriteDict.Add(inactiveCakeTypes[i].type, inactiveCakeTypes[i].spriteInactive);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType(CakeType _type)
    {
        this.type = _type;

        if (caketypeSpriteDict.ContainsKey(_type))
        {
            sprite.sprite = caketypeSpriteDict[_type];
        }
    }

    public void OnBoardStateChange(BoardEvent.eBoardState state)
    {
        if (this.type == piece.BoardRef.ListCakeType)
            return;
        
        if (state == BoardEvent.eBoardState.MATCHING_A_TYPE)
        {
            sprite.sprite = inactiveCakeTypesSpriteDict[type];
            this.transform.localScale = new Vector2(0.6f, 0.6f); 
        }
        else if(state == BoardEvent.eBoardState.MATCHFINISHED)
        {
            sprite.sprite = caketypeSpriteDict[type];
            this.transform.localScale = new Vector2(1, 1);
        }
    }

}

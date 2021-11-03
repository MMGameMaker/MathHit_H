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

    public CakeSprite[] cakeTypes;

    private CakeType type;

    public CakeType Type 
    {
        get { return type; }
        set { SetType(value); }
    }

    private SpriteRenderer sprite;

    private Dictionary<CakeType, Sprite> caketypeSpriteDict;

    public int NumCakeType
    {
        get { return cakeTypes.Length; }
    }


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        caketypeSpriteDict = new Dictionary<CakeType, Sprite>();

        for(int i=0; i<cakeTypes.Length; i++)
        {
            if (!caketypeSpriteDict.ContainsKey(cakeTypes[i].type))
            {
                caketypeSpriteDict.Add(cakeTypes[i].type, cakeTypes[i].sprite);
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
}

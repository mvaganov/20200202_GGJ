using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaslowManager : MonoBehaviour
{
    public static MaslowManager Instance {
        get{
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<MaslowManager>();
            }
            return m_Instance;
        }
    }
    private static MaslowManager m_Instance;
    public Sprite[] happies = new Sprite[10];
        public int[] happiesIndices;

    public Sprite[] sickies = new Sprite[3];
    public int[] sickiesIndices;

    public Sprite death;
    public int deathIndex;
    public Sprite physiology;
    public int physiologyIndex;
    public Sprite safety;
    public int safetyIndex;
    public Sprite belonging;
    public int belongingIndex;
    public Sprite esteem;
    public int esteemIndex;
    public Sprite actualization;
    public int actualizationIndex;
    public Sprite sick;
    public int sickIndex;

    public Sprite scared;
    public int scaredIndex;
    public Sprite lonely;
    public int lonelyIndex;
    public Sprite shame;
    public Sprite apathy;
    public Sprite burning;
    public Sprite cooling;

    public Sprite[] emojiSprites;

    // TODO: Make this a cached private hashtable replacement for the array so the lookup is free
    public int GetEmojiSpriteIndex(Sprite spriteSought)
    {
        int index = 0;

        if(spriteSought == null)
        {
            Debug.LogWarning("spriteSought null in GetEmojiSpriteIndex");
        }
        else
        {
            Debug.Log(spriteSought.name);
            string stringToIntify = "";
            if(spriteSought.name.Length == 18)
            {
                stringToIntify = spriteSought.name.Substring(17,1);
            }
            if(spriteSought.name.Length == 19)
            {
                stringToIntify = spriteSought.name.Substring(17,2);
            }
            if(spriteSought.name.Length == 20)
            {
                stringToIntify = spriteSought.name.Substring(17,3);
            }
            if(spriteSought.name.Length == 21)
            {
                stringToIntify = spriteSought.name.Substring(17,4);
            }
            Debug.Log("stringtointify:"+stringToIntify);
            int.TryParse(stringToIntify, out index);//System.Array.FindIndex(emojiSprites, s => s.name == spriteSought.name);
        }
        return index;
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

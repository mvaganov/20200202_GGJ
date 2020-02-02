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
    public Sprite[] sickies = new Sprite[3];

    public Sprite death;
    public Sprite physiology;
    public Sprite safety;
    public Sprite belonging;
    public Sprite esteem;
    public Sprite actualization;
    public Sprite sick;

    public Sprite scared;
    public Sprite lonely;
    public Sprite shame;
    public Sprite apathy;
    public Sprite burning;
    public Sprite cooling;

    public Sprite[] emojiSprites;

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

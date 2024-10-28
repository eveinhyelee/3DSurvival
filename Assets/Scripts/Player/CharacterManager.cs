using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return instance;
        }
    }
    private Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else //instance가 null이 아닐때 현재것을 파괴해라.
        {
            if (instance == this)
            { 
                Destroy(gameObject);
            }
        }
    }

}


using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    static GlobalManager s_Instance;
    public static GlobalManager Instance {  get { Init(); return s_Instance; } }
    private static void Init()
    {
        if(s_Instance == null)
        {
            GameObject go = GameObject.Find("@GlobalManager");
            if(go == null )
            {
                go = new GameObject { name = "@GlobalManager" };
                go.AddComponent<GlobalManager>();   
                DontDestroyOnLoad(go);  
                s_Instance = go.GetComponent<GlobalManager>();      
            }
        }
    }

    public string AccountID;
    public byte[] AuthToken;
    public int TokenLength;
}
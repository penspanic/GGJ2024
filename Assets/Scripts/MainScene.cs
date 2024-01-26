using UnityEngine;

public class MainScene : MonoBehaviour 
{
    private static MainScene instance;

    public static MainScene Instance 
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<MainScene>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(MainScene).Name;
                    instance = obj.AddComponent<MainScene>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

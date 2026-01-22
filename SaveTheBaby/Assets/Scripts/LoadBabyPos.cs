using UnityEngine;

public class LoadBabyPos : MonoBehaviour
{

    public static LoadBabyPos INSTANCE;

    public Vector2 pos;
    public Vector2 magPos;


    private void Awake()
    {
        if(INSTANCE == null || INSTANCE == this)
        {
            INSTANCE = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            INSTANCE.SetBabyPos();
            Destroy(gameObject);
        }
    }

    public void SetBabyPos()
    {
        GameObject.Find("Baby").transform.position = pos;
        GameObject.Find("Crane").transform.position = magPos;
    }

    public void updateBabySpawn(Vector2 newPos, Vector2 newMagPos)
    {
        pos = newPos;
        magPos = newMagPos;
    }


}

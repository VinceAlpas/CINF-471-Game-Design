using UnityEngine;

public class testing : MonoBehaviour
{

    public GameObject cube;
    Transform t;
    float speed = 0.01f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        t = cube.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //  rotation = rotation + 0.001f;
        if(t.position.y > 10)
        {
            speed = speed * -1;
        }else if (t.position.y < 0)
        {
            speed = speed * -1;
        }
            t.Translate(0,speed,0);
        }
    }
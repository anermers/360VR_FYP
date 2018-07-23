using UnityEngine;
using System.Collections;

public class ButtonFloating : MonoBehaviour
{
    public float speed = 1.0f;
    public float delta = 0.1f;
    private Vector3 startPos;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
            Vector3 v = startPos;
            v.y += delta * Mathf.Sin(Time.time * speed);
            transform.position = v;       
    }
}

using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public float scrollSpeed = 8f; //背景滚动速度
    private Vector3 startPos;
    private float width;

    public void Start()
    {
        startPos = transform.position;
       
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            width = sr.bounds.size.x;
        }
    }

    public void Update()
    {
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        if ((int)transform.position.x == 44)
        {
            transform.position = new Vector3(-44.2f,4.6f,0f);
            //Debug.Log(transform.position);z
        }
    }
}


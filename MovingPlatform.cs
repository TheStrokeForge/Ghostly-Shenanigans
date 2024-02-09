using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform start, End;
    public int Speed;
    Vector2 targetPos;
    void Start()
    {
        targetPos = End.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, start.position) < .1f) targetPos = End.position;
        if (Vector2.Distance(transform.position, End.position) < .1f) targetPos = start.position;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start.position, End.position);
    }
}

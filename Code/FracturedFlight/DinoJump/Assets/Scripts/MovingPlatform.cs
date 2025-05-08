using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform pointC = null;
    public Transform pointD = null;
    public float moveSpeed = 8f;
    public bool threeMovingPoints = false;
    private bool towardsC = true;

    private Vector3 nextPosition;

    public bool activated = true;
    void Start()
    {
        threeMovingPoints = pointA != null && pointB != null && pointC != null && pointD == null;
        nextPosition = pointB.position;
    }

    void FixedUpdate()
    {
        if (activated)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

            if (transform.position == nextPosition)
            {
                if (nextPosition == pointA.position)
                {
                    nextPosition = pointB.position;
                }
                else if (nextPosition == pointB.position)
                {
                    if (pointC == null)
                    {
                        nextPosition = pointA.position;
                    }
                    else
                    {
                        if (threeMovingPoints)
                        {
                            if (towardsC)
                            {
                                nextPosition = pointC.position;
                                towardsC = !towardsC;
                            }
                            else
                            {
                                nextPosition = pointA.position;
                                towardsC = !towardsC;
                            }
                        }
                        else
                        {
                            nextPosition = pointC.position;
                        }
                    }
                }else if (nextPosition == pointC.position)
                {
                    if (pointD == null)
                    {
                        nextPosition = pointB.position;
                    }
                    else
                    {
                        nextPosition = pointD.position;
                    }
                }
                else if (nextPosition == pointD.position)
                {
                    nextPosition = pointA.position;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            activated = true;
            collision.transform.parent = transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}

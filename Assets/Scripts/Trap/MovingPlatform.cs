using UnityEngine;

//TP2 - Fernando Claro

public class MovingPlatform : MonoBehaviour, ITrap<GameObject>
{
    public Transform startPosition;
    public Transform endPosition;
    public float speed = 3f;

    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = endPosition.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

        if (transform.position == endPosition.position)
        {
            targetPosition = startPosition.position;
        }

        if (transform.position == startPosition.position)
        {
            targetPosition = endPosition.position;
        }
    }

    public void ActivateTrap(GameObject target)
    {

    }

    public void DeactivateTrap(GameObject target)
    {

    }
}


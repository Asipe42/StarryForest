using UnityEngine;

namespace Unit
{
    public class HourGlass : MonoBehaviour
    {
        [SerializeField]
        float moveSpeed;

        [SerializeField]
        float deadlineX;

        void Update()
        {
            Move();
            CheckPosition();
        }

        void Move()
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);
        }

        void CheckPosition()
        {
            if (transform.position.x < deadlineX)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Judge"))
            {
                CatchController.instance.OnCatch = true;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Judge"))
            {
                CatchController.instance.OnCatch = false;
            }
        }
    }
}
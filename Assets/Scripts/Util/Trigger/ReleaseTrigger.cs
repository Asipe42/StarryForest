using UnityEngine;

namespace Trigger
{
    public class ReleaseTrigger : MonoBehaviour
    {
        [SerializeField] EActionType actionType;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController pc;

                if (collision.gameObject.TryGetComponent<PlayerController>(out pc))
                {
                    pc.playerMovement.ReleaseMovement(actionType);
                    pc.playerAnimation.ChangeAnimationState(EActionType.Default);
                }
            }
        }
    }

}
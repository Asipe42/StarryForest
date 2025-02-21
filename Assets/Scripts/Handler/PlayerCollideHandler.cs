using System.Collections;
using UnityEngine;
using UniRx;

public class PlayerCollideHandler : MonoBehaviour
{
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundMask;

    RaycastHit2D groundHit;

    ReactiveProperty<bool> onGround;

    public ReactiveProperty<bool> OnGround
    {
        get => onGround;
        set => onGround.Value = value.Value;
    }

    void Start()
    {
        InitProperty();
    }

    void InitProperty()
    {
        onGround = new ReactiveProperty<bool>(true);

        onGround.Subscribe(state => PlayerController.instance.playerAnimation.ChangeAnimationState(EActionType.Default));
    }

    void Update()
    {
        CheckGround();
    }

    void CheckGround()
    {
        groundHit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundMask);

        onGround.Value = (groundHit.collider == null ? false : true);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
    }
}


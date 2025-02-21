using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class PlayerMovementData
{
    public float jumpForce;
    public float knockbackForce;
    public float downhillGravityScale;
    public float defaultGravityScale;
}

public class PlayerMovementHandler : SerializedMonoBehaviour
{
    [SerializeField] 
    Dictionary<EActionType, Collider2D> colliderDictionary;

    Rigidbody2D rigid;

    EActionType currentAction;

    [HideInInspector] 
    public bool onMovement = false;

    [HideInInspector] 
    public bool onSlide = false;

    [SerializeField]
    PlayerMovementData playerMovementData;

    void Awake()
    {
        InitProperty();
    }

    void InitProperty()
    {
        rigid = GetComponent<Rigidbody2D>();

        colliderDictionary[EActionType.Slide].enabled = false;
    }

    public void ExcuteAction(EActionType actionType)
    {
        currentAction = actionType;

        PlayMovement();
    }

    public void PlayMovement()
    {
        onMovement = true;

        SetupMovement();
    }

    void SetupMovement()
    {
        onMovement = false;

        switch (currentAction)
        {
            case EActionType.Jump:
                Jump();
                break;
            case EActionType.Slide:
                Slide();
                break;
            case EActionType.Downhill:
                Downhill();
                break;
        }
    }

    public void ReleaseMovement(EActionType type)
    {
        switch (type)
        {
            case EActionType.Jump:
                ReleaseJump();
                break;
            case EActionType.Slide:
                ReleaseSlide();
                break;
            case EActionType.Downhill:
                RelaseDownhill();
                break;
        }
    }

    void Jump()
    {
        if (PlayerController.instance.playerCollider.OnGround.Value)
        {
            if (rigid.velocity.y > 0)
                return;

            #region SFX
            SFXHandler.instance.PlaySFX(SFXDefine.JUMP);
            #endregion

            rigid.AddForce(Vector2.up * playerMovementData.jumpForce, ForceMode2D.Impulse);
        }
    }

    void Slide()
    {
        #region SFX
        SFXHandler.instance.PlaySFX(SFXDefine.SLIDE);
        #endregion

        colliderDictionary[EActionType.Default].enabled = false;
        colliderDictionary[EActionType.Slide].enabled = true;
    }

    void Downhill()
    {
        #region SFX
        SFXHandler.instance.PlaySFX(SFXDefine.DOWNHILL);
        #endregion

        rigid.velocity = new Vector2(rigid.velocity.x, 2f);
        rigid.gravityScale = playerMovementData.downhillGravityScale;
    }

    void ReleaseJump()
    {
        ;
    }

    void ReleaseSlide()
    {
        colliderDictionary[EActionType.Default].enabled = true;
        colliderDictionary[EActionType.Slide].enabled = false;
    }

    void RelaseDownhill()
    {
        rigid.gravityScale = playerMovementData.defaultGravityScale;
    }

    public void Knockback()
    {
        rigid.velocity = Vector2.zero;

        rigid.AddForce(new Vector2(-0.8f, 1.0f) * playerMovementData.knockbackForce, ForceMode2D.Impulse);
    }
}

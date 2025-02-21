using UnityEngine;
using Spine.Unity;

public class SpineAnimationBehavior : StateMachineBehaviour
{
    public AnimationClip motion;
    string animationClip;
    bool loop;

    public int layer = 0;
    public float timeScale = 1.0f;

    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState spineAnimationState;
    Spine.TrackEntry trackEntry;

    void Awake()
    {
        if (motion != null)
        {
            animationClip = motion.name;
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (skeletonAnimation == null)
        {
            skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.state;
        }

        if (animationClip != null)
        {
            loop = stateInfo.loop;
            trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
            trackEntry.TimeScale = timeScale;
        }
    }
}

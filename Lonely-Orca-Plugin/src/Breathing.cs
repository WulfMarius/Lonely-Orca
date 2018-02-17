using UnityEngine;

namespace LonelyOrca
{
    public class Breathing : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.applyRootMotion = true;

            int breathingCount = animator.GetInteger("BreathingCount");
            animator.SetInteger("BreathingCount", breathingCount - 1);
        }
    }
}
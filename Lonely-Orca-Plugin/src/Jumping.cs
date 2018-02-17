using UnityEngine;

namespace LonelyOrca
{
    public class Jumping : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.applyRootMotion = true;

            animator.SetBool("Jump", false);
        }
    }
}
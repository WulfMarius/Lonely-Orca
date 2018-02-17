using UnityEngine;

namespace LonelyOrca
{
    public class Idle : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.applyRootMotion = false;
        }
    }
}
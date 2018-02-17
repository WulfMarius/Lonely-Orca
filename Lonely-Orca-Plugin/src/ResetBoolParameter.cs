using UnityEngine;

namespace LonelyOrca
{
    public class ResetBoolParameter : StateMachineBehaviour
    {
        public string parameter;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.applyRootMotion = true;
            animator.SetBool(parameter, false);
        }
    }
}

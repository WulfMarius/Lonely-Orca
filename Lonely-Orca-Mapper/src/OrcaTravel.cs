using UnityEngine;

namespace LonelyOrca
{
    public class OrcaTravel : MonoBehaviour
    {
        private const float BREATH_DISTANCE = 15;
        private const float BREATHE_MAX_DELAY = 10;
        private const float BREATHE_MIN_DELAY = 0;

        private const int DEFAULT_MAX_ROTATION = 10;

        private const int DIVE_CHANCE = 80;
        private const float DIVE_MAX_DELAY = 120;
        private const float DIVE_MIN_DELAY = 60;

        private const int JUMP_CHANCE = 10;
        private const float JUMP_DISTANCE = 50;
        private const float JUMP_MAX_DELAY = 20;
        private const float JUMP_MIN_DELAY = 5;

        private const int MAX_TARGET_FINDING_LOOPS = 50;
        private const int MAX_PLAYER_DISTANCE = 150 * 150;
        private const float WATER_LEVEL = 15.3f;

        private Animator animator;
        private float[] areaX = { 1330, 1428, 1399, 1327, 1249, 1163, 1199, 1225, 1221, 1263, 1307, 1243, 1076, 762, 662, 625, 504, 407, 369, 203, 121, 1600, 1600 };
        private float[] areaY = { 1815, 1571, 1388, 1370, 1430, 1308, 1162, 1114, 1052, 1025, 872, 729, 657, 641, 681, 669, 669, 676, 768, 749, 400, 400, 1815 };
        private int layerIndex;
        private float maxX;
        private float maxY;
        private float minX;
        private float minY;

        private Vector3 target;
        private float delay;
        private int remainingBreaths;
        private int idleStateHash;

        void Start()
        {
            this.minX = Mathf.Min(areaX);
            this.maxX = Mathf.Max(areaX);
            this.minY = Mathf.Min(areaY);
            this.maxY = Mathf.Max(areaY);

            this.animator = this.GetComponentInChildren<Animator>();
            this.idleStateHash = Animator.StringToHash("Idle");
            this.layerIndex = this.animator.GetLayerIndex("Default");

            while (!GetRandomPosition(out this.target)) ;
            this.transform.position = this.target;
        }

        void Update()
        {
            this.delay -= Time.deltaTime;
            if (this.delay > 0)
            {
                return;
            }

            if (this.animator.GetCurrentAnimatorStateInfo(layerIndex).shortNameHash != idleStateHash)
            {
                this.delay += 0.1f;
                return;
            }

            this.transform.position = target;

            if (remainingBreaths <= 0)
            {
                this.DoDive();
                return;
            }

            if (this.ShouldJump())
            {
                this.DoJump();
            }
            else
            {
                this.DoBreathe();
            }
        }

        private bool IsPlayerCloseEnough()
        {
            Vector3 playerPosition = GameManager.GetPlayerTransform().position;

            for (int i = 0; i < areaX.Length; i++)
            {
                float deltaX = areaX[i] - playerPosition.x;
                float deltaY = areaY[i] - playerPosition.z;

                if (deltaX * deltaX + deltaY * deltaY < MAX_PLAYER_DISTANCE)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsOrcaActive()
        {
            TODBlendState todBlendState = GameManager.GetTimeOfDayComponent().GetTODBlendState();

            if (todBlendState == TODBlendState.NightEndToDawn || todBlendState == TODBlendState.NightStartToNightEnd)
            {
                return false;
            }

            if (GameManager.GetWeatherComponent().IsBlizzard())
            {
                return false;
            }

            return true;
        }

        private void DoBreathe()
        {
            this.remainingBreaths--;

            if (!this.GetTarget(BREATH_DISTANCE, out this.target))
            {
                return;
            }

            this.transform.LookAt(this.target, Vector3.up);
            this.animator.SetBool("Breathe", true);

            this.delay = Random.Range(BREATHE_MIN_DELAY, BREATHE_MAX_DELAY);
        }

        private void DoDive()
        {
            if (!this.GetRandomPosition(out this.target))
            {
                return;
            }

            this.delay = Random.Range(DIVE_MIN_DELAY, DIVE_MAX_DELAY);

            if (IsOrcaActive() && IsPlayerCloseEnough())
            {
                this.remainingBreaths = Random.Range(1, 5);
            }
        }

        private void DoJump()
        {
            if (!this.GetTarget(JUMP_DISTANCE, out this.target))
            {
                return;
            }

            this.transform.LookAt(target, Vector3.up);
            this.animator.SetBool("Jump", true);

            this.delay = Random.Range(JUMP_MIN_DELAY, JUMP_MAX_DELAY);
        }

        private bool GetRandomPosition(out Vector3 target)
        {
            for (int i = 0; i < MAX_TARGET_FINDING_LOOPS; i++)
            {
                target = new Vector3(Random.Range(minX, maxX), WATER_LEVEL, Random.Range(minY, maxY));
                if (IsInArea(target.x, target.z))
                {
                    return true;
                }
            }

            Debug.Log("Failed to create random position inside area");
            target = new Vector3(0, WATER_LEVEL, 0);
            return false;
        }

        private bool GetTarget(float requiredTravelDistance, out Vector3 target)
        {
            int maxRotation = DEFAULT_MAX_ROTATION;

            for (int i = 0; i < MAX_TARGET_FINDING_LOOPS; i++)
            {
                Quaternion randomRotation = Quaternion.AngleAxis(Random.value * maxRotation, Vector3.up);
                target = this.transform.position + randomRotation * this.transform.forward * requiredTravelDistance;
                target.y = WATER_LEVEL;

                if (IsInArea(target.x, target.z))
                {
                    return true;
                }

                maxRotation++;
            }

            Debug.Log("Failed to create target position inside area");
            target = new Vector3(0, WATER_LEVEL, 0);
            return false;
        }

        private bool IsInArea(float x, float y)
        {
            bool result = false;

            for (int i = 0, j = this.areaX.Length - 1; i < this.areaX.Length; i++)
            {
                float polyStartX = areaX[i];
                float polyEndX = areaX[j];

                float polyStartY = areaY[i];
                float polyEndY = areaY[j];

                if ((polyStartY < y && polyEndY >= y || polyEndY < y && polyStartY >= y)
                    && (polyStartX <= x || polyEndX <= x))
                {
                    if (polyStartX + (y - polyStartY) / (polyEndY - polyStartY) * (polyEndX - polyStartX) < x)
                    {
                        result = !result;
                    }
                }

                j = i;
            }

            return result;
        }

        private bool ShouldDive()
        {
            return Utils.RollChance(DIVE_CHANCE);
        }

        private bool ShouldJump()
        {
            return Utils.RollChance(JUMP_CHANCE);
        }
    }
}
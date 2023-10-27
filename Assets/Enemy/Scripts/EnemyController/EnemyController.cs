using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
namespace Game
{
    public enum MoveDirectionX {
        Left = -1,
        Right = 1,
        None = 0
    }
    [Serializable]
    public class AdvancedEnemySettings
    {
        public float enemyAutoPathingEdgeOffsetY = 1f;
        public float enemyAutoPathingEdgeOffsetX = 0.2f;
        [Range(0,1)]
        public float enemyAutoPathingEdgeOffsetX2 = 0.9f;
    }
    public partial class EnemyController : StateController {
        [NonSerialized]
        public ObjectState shouldBeNextState = ObjectState.Default;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Rigidbody2D rb2d;
        [Header("边界大小")]
        [Tooltip("仅用于和场景的碰撞检测")]
        public Vector2 boundingBoxSize;
        public LayerMask groundMask;
        [Header("速度")]
        [Tooltip("调试信息，无需修改")]
        public Vector2 velocity;
        [Header("位置")]
        [Tooltip("调试信息，无需修改")]
        public Vector2 position;
        [Tooltip("调试信息，无需修改")]
        public bool isGrounded = true;
        [Header("玩家信息")]
        [Tooltip("玩家信息在此SO中实时更新")]
        public PlayerController playerInfo;
        public Vector2 playerPos => playerInfo.transform.position;
        [Header("移动速度")]
        [Tooltip("水平方向的最大速度")]
        public float velocityX = 3f;
        [Tooltip("水平方向的最大加速度")]
        public float accelerationX = 20f;
        [Tooltip("水平方向的最大减速度")]
        public float decelerationX = 100f;
        [Header("是否受到重力影响")]
        public bool gravity = true;
        [Header("跳跃信息")]
        [Tooltip("跳跃初速度")]
        public float jumpspeedY = 9f;
        public float jumpspeedX = 3f;
        [Tooltip("跳跃持续时间")]
        public int jumpTimeByFrames = 9;
        [Header("飞行信息")]
        [Tooltip("垂直方向的最大速度")]
        public float velocityY = 3f;
        //TODO: 飞行信息
        [Header("强制移动（比如被击退）")]
        [Tooltip("强制移动速率")]
        public float forceMovementRate = 10f;
        [Tooltip("强制移动速率衰减")]
        public float forceMovementMult = 0.15f;
        private Vector2 forceMovement;
        [Header("动作指令")]
        [Tooltip("用于让state传递水平方向的移动")]
        public MoveDirectionX moveDirectionX = MoveDirectionX.None;
        [Tooltip("用于让state传递跳跃指令")]
        public bool jump = false;
        [Header("攻击动画时长")]
        [Tooltip("时长以帧计算")]
        public int attackTimeByFrame = 60;
        [Header("死亡动画时长")]
        [Tooltip("时长以帧计算，在HP归零时播放死亡动画，时间结束时destroy当前gameObject")]
        public int dieTimeByFrame = 40;
        [Header("攻击预制体")]
        public GameObject[] attackTemplate;
        private GameObject[] attackInstance;
        [Header("最大生命值")]
        public int maxHP = 30;
        [Header("当前生命值")]
        [Tooltip("调试信息，无需修改")]
        public int currentHP = 30;
        private bool isJumping = false;
        
        private float patrolRangeL;
        private float patrolRangeR;
        private float patrolRangeU;
        private float patrolRangeD;
        [Header("自动行为")]
        [Tooltip("由状态修改，无需手动修改")]
        public AutoActionState autoAction = AutoActionState.None;

        [Header("高级设置")]
        public AdvancedEnemySettings advancedSettings;
        private float offsetY => advancedSettings.enemyAutoPathingEdgeOffsetY;
        private float offsetX => advancedSettings.enemyAutoPathingEdgeOffsetX;
        private float offsetRange => advancedSettings.enemyAutoPathingEdgeOffsetX2;


        public override void Start() {
            base.Start();
            currentHP = maxHP;
            GameRestart(); // clear powerup in the beginning, go to start state
        }

        public override void UpdateObj(float deltaTime) {
            base.UpdateObj(deltaTime);
            isGrounded = CheckGround();
            AutoMoveUpdate();
            UpdateVelocity(deltaTime);
            UpdateColliderX((velocity.x + forceMovement.x) * deltaTime);
            UpdateColliderY((velocity.y + forceMovement.y) * deltaTime);
            transform.position = position;
        }


        private IEnumerator WaitAndDestroy() {
            gameObject.GetComponent<Collider2D>().enabled = false;
            for (int i = 0; i < dieTimeByFrame; i++) {
                yield return null;
            }
            Destroy(gameObject);
        }


        public void TakeDamage(int damage) {
            currentHP -= damage;
            if (currentHP <= 0) {
                PlayDeadAnimation();
                StartCoroutine(WaitAndDestroy());
            } else {
                StartCoroutine(FlashRedOnce());
            }
        }

        public void PassAway() {
            Destroy(gameObject);
        }

        // this should be added to the GameRestart EventListener as callback
        public void GameRestart() {
            // set the start state
            TransitionToState(startState);
        }

        private void OnDrawGizmos() {
            // 设置Gizmo的颜色
            Gizmos.color = Color.red;

            // 计算矩形的各个点
            Vector3 bottomLeft = new Vector3(patrolRangeL, patrolRangeD, 0);
            Vector3 bottomRight = new Vector3(patrolRangeR, patrolRangeD, 0);
            Vector3 topLeft = new Vector3(patrolRangeL, patrolRangeU, 0);
            Vector3 topRight = new Vector3(patrolRangeR, patrolRangeU, 0);

            if (autoAction == AutoActionState.Patrol) {
                // 绘制矩形的四条边
                Gizmos.DrawLine(bottomLeft, bottomRight);
                Gizmos.DrawLine(bottomRight, topRight);
                Gizmos.DrawLine(topRight, topLeft);
                Gizmos.DrawLine(topLeft, bottomLeft);
            }

            Gizmos.DrawCube(position, boundingBoxSize);
        }

    }
}

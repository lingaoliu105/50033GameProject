using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
namespace EnemyPro {
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
    public partial class EnemyControllerPro : StateController {
        [NonSerialized]
        public ObjectState shouldBeNextState = ObjectState.Default;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Rigidbody2D rb2d;
        [Header("�߽��С")]
        [Tooltip("�����ںͳ�������ײ���")]
        public Vector2 boundingBoxSize;
        public LayerMask groundMask;
        [Header("�ٶ�")]
        [Tooltip("������Ϣ�������޸�")]
        public Vector2 velocity;
        [Header("λ��")]
        [Tooltip("������Ϣ�������޸�")]
        public Vector2 position;
        [Tooltip("������Ϣ�������޸�")]
        public bool isGrounded = true;
        [Header("�����Ϣ")]
        [Tooltip("�����Ϣ�ڴ�SO��ʵʱ����")]
        public PlayerInfo playerInfo;
        public Vector2 playerPos => playerInfo.position;
        [Header("�ƶ��ٶ�")]
        [Tooltip("ˮƽ���������ٶ�")]
        public float velocityX = 3f;
        [Tooltip("ˮƽ����������ٶ�")]
        public float accelerationX = 20f;
        [Tooltip("ˮƽ����������ٶ�")]
        public float decelerationX = 100f;
        [Header("�Ƿ��ܵ�����Ӱ��")]
        public bool gravity = true;
        [Header("��Ծ��Ϣ")]
        [Tooltip("��Ծ���ٶ�")]
        public float jumpspeedY = 9f;
        public float jumpspeedX = 3f;
        [Tooltip("��Ծ����ʱ��")]
        public int jumpTimeByFrames = 9;
        [Header("������Ϣ")]
        [Tooltip("��ֱ���������ٶ�")]
        public float velocityY = 3f;
        //TODO: ������Ϣ
        [Header("ǿ���ƶ������类���ˣ�")]
        [Tooltip("ǿ���ƶ�����")]
        public float forceMovementRate = 10f;
        [Tooltip("ǿ���ƶ�����˥��")]
        public float forceMovementMult = 0.15f;
        private Vector2 forceMovement;
        [Header("����ָ��")]
        [Tooltip("������state����ˮƽ������ƶ�")]
        public MoveDirectionX moveDirectionX = MoveDirectionX.None;
        [Tooltip("������state������Ծָ��")]
        public bool jump = false;
        [Header("��������ʱ��")]
        [Tooltip("ʱ����֡����")]
        public int attackTimeByFrame = 60;
        [Header("��������ʱ��")]
        [Tooltip("ʱ����֡���㣬��HP����ʱ��������������ʱ�����ʱdestroy��ǰgameObject")]
        public int dieTimeByFrame = 40;
        [Header("����Ԥ����")]
        public GameObject[] attackTemplate;
        private GameObject[] attackInstance;
        [Header("�������ֵ")]
        public int maxHP = 30;
        [Header("��ǰ����ֵ")]
        [Tooltip("������Ϣ�������޸�")]
        public int currentHP = 30;
        private bool isJumping = false;
        
        private float patrolRangeL;
        private float patrolRangeR;
        private float patrolRangeU;
        private float patrolRangeD;
        [Header("�Զ���Ϊ")]
        [Tooltip("��״̬�޸ģ������ֶ��޸�")]
        public AutoActionState autoAction = AutoActionState.None;

        [Header("�߼�����")]
        public AdvancedEnemySettings advancedSettings;
        private float offsetY => advancedSettings.enemyAutoPathingEdgeOffsetY;
        private float offsetX => advancedSettings.enemyAutoPathingEdgeOffsetX;
        private float offsetRange => advancedSettings.enemyAutoPathingEdgeOffsetX2;


        public override void Start() {
            base.Start();
            currentHP = maxHP;
            GameRestart(); // clear powerup in the beginning, go to start state
        }

        public void UpdateObj(float deltaTime) {
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
                PlayDeadAnimate();
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

        private new void OnDrawGizmos() {
            // ����Gizmo����ɫ
            Gizmos.color = Color.red;

            // ������εĸ�����
            Vector3 bottomLeft = new Vector3(patrolRangeL, patrolRangeD, 0);
            Vector3 bottomRight = new Vector3(patrolRangeR, patrolRangeD, 0);
            Vector3 topLeft = new Vector3(patrolRangeL, patrolRangeU, 0);
            Vector3 topRight = new Vector3(patrolRangeR, patrolRangeU, 0);

            if (autoAction == AutoActionState.Patrol) {
                // ���ƾ��ε�������
                Gizmos.DrawLine(bottomLeft, bottomRight);
                Gizmos.DrawLine(bottomRight, topRight);
                Gizmos.DrawLine(topRight, topLeft);
                Gizmos.DrawLine(topLeft, bottomLeft);
            }

            Gizmos.DrawCube(position, boundingBoxSize);
        }

    }
}

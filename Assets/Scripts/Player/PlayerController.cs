using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Rendering.Universal;

namespace Game {
    public partial class PlayerController : MonoBehaviour {

        public LayerMask groundMask;

        private int frameRate = 60;
        private float deltaTime;

        public Vector2 Position;
        public Vector2 InitPos = new Vector2(1f, -1.5f);
        public Vector2 Speed;
        [SerializeField]
        private bool onGround;
        public bool OnGround { get => onGround;}
        private bool wasOnGround;

        public JumpCheck JumpCheck { get; set; }
        [SerializeField]
        private float varJumpTimer = 0f;
        public float VarJumpTimer { get => varJumpTimer; set => varJumpTimer = value; }
        private float varJumpSpeed = 0f;
        public float VarJumpSpeed { get => varJumpSpeed; set => varJumpSpeed = value; }

        private float maxFall;
        private float fastMaxFall;
        public float MaxFall { get => maxFall; set => maxFall = value; }

        private FiniteStateMachine<BaseActionState> stateMachine;

        public Vector2 LastAim { get; set; }

        private int moveX;
        public int MoveX { get => moveX; }
        private int moveY;
        public int MoveY { get => moveY; }

        public object Holding => null;

        private SpriteRenderer spriteRenderer;


        public bool launched;//����ʱ������Ч��
        public float launchedTimer;
        private float dashCooldownTimer;                //�����ȴʱ���������Ϊ0ʱ�������ٴγ��
        private float dashRefillCooldownTimer;          //
        public int dashes;
        public int lastDashes;
        private float wallSpeedRetentionTimer; // If you hit a wall, start this timer. If coast is clear within this timer, retain h-speed
        private float wallSpeedRetained;


        public float DashCooldownTimer { get => dashCooldownTimer; set => dashCooldownTimer = value; }
        public float DashRefillCooldownTimer { get => dashRefillCooldownTimer; set => dashRefillCooldownTimer = value; }

        public bool DashStartedOnGround { get; set; }

        public float WallSlideTimer { get; set; } = Constants.WallSlideTime;
        public int WallSlideDir { get; set; }


        public Facings Facing { get; set; }  //��ǰ����

        void Awake() {
            this.stateMachine = new FiniteStateMachine<BaseActionState>((int)EActionState.Size);
            this.stateMachine.AddState(new NormalState(this));
            this.stateMachine.AddState(new DashState(this));
            this.stateMachine.AddState(new ClimbState(this));

            this.Facing = Facings.Right;
            this.LastAim = Vector2.right;

            Position = InitPos;
            collider = normalHitbox;
            Application.targetFrameRate = 60;
            deltaTime = 1f / frameRate;
            this.JumpCheck = new JumpCheck(this, Constants.EnableJumpGrace);
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        #region Debug
        [SerializeField]
        private Vector2 JoystickValue;
        [SerializeField]
        private float ButtonBufferTime = 0;
        [SerializeField]
        private bool CanJump = false;
        private void OnDestroy() {
            outputToFile();
        }
        List<Vector2> debugPoints = new List<Vector2>();
        List<float> debugTime = new List<float>();
        List<float> debugJump = new List<float>();
        float lastTime = 0;
        public void outputToFile() {
            string path = Application.dataPath + "/debug1.txt";
            System.IO.File.WriteAllText(path, string.Empty);
            foreach (var item in debugPoints) {
                System.IO.File.AppendAllText(path, item.ToString() + "\n");

            }
            path = Application.dataPath + "/debug2.txt";
            System.IO.File.WriteAllText(path, string.Empty);
            foreach (var item in debugTime) {
                System.IO.File.AppendAllText(path, item.ToString() + "\n");
            }
            path = Application.dataPath + "/debug3.txt";
            System.IO.File.WriteAllText(path, string.Empty);
            foreach (var item in debugJump) {
                System.IO.File.AppendAllText(path, item.ToString() + "\n");
            }
        }
        #endregion


        void Start() {
            this.stateMachine.State = (int)EActionState.Normal;
        }

        // Update is called once per frame
        void Update() {
            
            GameInput.Update(deltaTime);
            JoystickValue = GameInput.Joystick.Value;
            ButtonBufferTime = GameInput.JumpButton.buffer;

            if (varJumpTimer > 0) {
                varJumpTimer -= deltaTime;
            }

            // move x
            moveX = Math.Sign(JoystickValue.x);
            if (moveX != 0) {
                Facing = (Facings)moveX;
            }
            moveY = Math.Sign(JoystickValue.y);
            LastAim = GameInput.GetAimVector(Facing);

            //Get ground
            wasOnGround = onGround;
            if (Speed.y <= 0) {
                this.onGround = CheckGround();//��ײ������
            } else {
                this.onGround = false;
            }

            //Dash
            {
                if (dashCooldownTimer > 0)
                    dashCooldownTimer -= deltaTime;
                if (dashRefillCooldownTimer > 0) {
                    dashRefillCooldownTimer -= deltaTime;
                } else if (onGround) {
                    RefillDash();
                }
            }

            stateMachine.Update(deltaTime);

            //��Ծ���
            JumpCheck.Update(deltaTime);

            CanJump = JumpCheck.AllowJump();
            UpdateColliderX(Speed.x*deltaTime);
            UpdateColliderY(Speed.y*deltaTime);
            if (lastTime != Time.fixedTime) {
                debugPoints.Add(Speed);
                debugTime.Add(Time.fixedTime);
                lastTime = Time.fixedTime;
                debugJump.Add(varJumpTimer);
            }
            UpdateRender();
        }

        public void SetState(int state) {
            this.stateMachine.State = state;
        }

        public void Jump() { 
            // TODO: Wall Jump
            GameInput.JumpButton.ConsumeBuffer();
            JumpCheck.ResetTimer();
            varJumpTimer = Constants.VarJumpTime;
            Speed.x += Constants.JumpSpeed * moveX;
            Speed.y = Constants.JumpSpeed;
            varJumpSpeed = Speed.y;
            // TODO: Play Jump Animation and Sound and Particle
        }

        public bool Ducking {
            get {
                return this.collider == this.duckHitbox || this.collider == this.duckHurtbox;
            }
            set {
                if (value) {
                    this.collider = this.duckHitbox;
                    return;
                } else {
                    this.collider = this.normalHitbox;
                }
                PlayDuck(value);
            }
        }

        //��⵱ǰ�Ƿ����վ��
        public bool CanUnDuck {
            get { 
                if (!Ducking)
                    return true;
                Rect lastCollider = this.collider;
                this.collider = normalHitbox;
                bool noCollide = !CollideCheck(this.Position, Vector2.zero);
                this.collider = lastCollider;
                return noCollide;
            }
        }

        public bool DuckFreeAt(Vector2 at) {
            Vector2 lastPosition = Position;
            Rect lastCollider = this.collider;
            Position = at;
            this.collider = duckHitbox;

            bool noCollide = !CollideCheck(this.Position, Vector2.zero);

            this.Position = lastPosition;
            this.collider = lastCollider;

            return noCollide;
        }

        public bool CanDash{
            get {
                return GameInput.DashButton.Pressed() && DashCooldownTimer <= 0 && dashes > 0;
            }   
        }

        public EActionState Dash() {
            //wasDashB = Dashes == 2;
            this.dashes = Math.Max(0, this.dashes - 1);
            GameInput.DashButton.ConsumeBuffer();
            return EActionState.Dash;
        }

        public bool RefillDash() {
            if (this.dashes < Constants.MaxDashes) {
                this.dashes = Constants.MaxDashes;
                return true;
            } else
                return false;
        }


    }

}
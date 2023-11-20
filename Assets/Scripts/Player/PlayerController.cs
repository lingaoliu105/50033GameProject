using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Rendering.Universal;
using Assets.Scripts.Attack;

namespace Game {
    public partial class PlayerController : MonoBehaviour {

        public LayerMask groundMask;

        private float invinsibleOnHitTimer = 0f;

        private int frameRate = 60;
        private float deltaTime;

        public bool isAlive = true;

        public Vector2 Position;
        public Vector2 InitPos = new Vector2(1f, -1.5f);
        public Vector2 Speed;
        [SerializeField]
        private bool onGround;
        public bool OnGround { get => onGround;}
        private bool wasOnGround;
        public bool WasOnGround { get => wasOnGround; }

        public JumpCheck JumpCheck { get; set; }
        public WallBoost WallBoost { get; set; }

        public int ForceMoveX { get; set; }
        public float ForceMoveXTimer { get; set; }
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

        public float ClimbNoMoveTimer { get; set; } // Stop moving for a bit after climbing to avoid regrabbing the same wall

        public int HopWaitX;   // If you climb hop onto a moving solid, snap to beside it until you get above it
        public float HopWaitXSpeed;

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

        public Weapon TestRangedWeapon;
        public Weapon TestMeleeWeapon;


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
            this.stateMachine.AddState(new AttackState(this));
            this.stateMachine.AddState(new ComboState(this));
            this.Facing = Facings.Right;
            this.LastAim = Vector2.right;

            Position = InitPos;
            collider = normalHitbox;
            Application.targetFrameRate = 60;
            deltaTime = 1f / frameRate;
            GameInput.Init();
            this.JumpCheck = new JumpCheck(this, Constants.EnableJumpGrace);
            this.WallBoost = new WallBoost(this);
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            this.LoadPlayerInfo();
        }
        #region Debug
        [SerializeField]
        private Vector2 JoystickValue;
        [SerializeField]
        private float ButtonBufferTime = 0;
        [SerializeField]
        private bool CanJump = false;
/*        private void OnDestroy() {
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
        }*/
        #endregion


        void Start() {
            this.stateMachine.State = (int)EActionState.Normal;
            this.SetUpWeapons(TestRangedWeapon, TestMeleeWeapon);
            this.InitInventory();
            isAlive = true;
        }

        private void OnDestroy() {
            SavePlayerInfo();
        }

        // Update is called once per frame
        void Update() {
            if (!isAlive) {
                return;
            }
            
            GameInput.Update(deltaTime);

            UpdateBuff(deltaTime);
            CalcFix();
            CheckForSomeObject();
            UpdateEquipsOnUpdate();
            JoystickValue = GameInput.Joystick.Value;
            ButtonBufferTime = GameInput.JumpButton.buffer;
            RecoverStamina(deltaTime);
            if (varJumpTimer > 0) {
                varJumpTimer -= deltaTime;
            }
            if (invinsibleOnHitTimer > 0) {
                invinsibleOnHitTimer -= deltaTime;
            }
            // switch tube 
            if (GameInput.SwitchItem.Pressed()) {
                if (tubeSwitchColdDown <= 0) {
                    SwitchTubeType();
                    tubeSwitchColdDown = Constants.TubeSwtichColdDownTime;
                }
            }
            if (tubeSwitchColdDown > 0) {
                tubeSwitchColdDown -= deltaTime;
            }

            // move x
            if (ForceMoveXTimer > 0) {
                ForceMoveXTimer -= deltaTime;
                this.moveX = ForceMoveX;
            } else {
                moveX = Math.Sign(JoystickValue.x);
            }
            
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

            //Wall Slide
            if (this.WallSlideDir != 0) {
                this.WallSlideTimer = Math.Max(this.WallSlideTimer - deltaTime, 0);
                this.WallSlideDir = 0;
            }
            if (this.onGround && this.stateMachine.State != (int)EActionState.Climb) {
                this.WallSlideTimer = Constants.WallSlideTime;
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

            //Wall Boost, ����������WallJump
            this.WallBoost?.Update(deltaTime);

            //��Ծ���
            JumpCheck.Update(deltaTime);

            CanJump = JumpCheck.AllowJump();
            UpdateColliderX(Speed.x*deltaTime);
            UpdateColliderY(Speed.y*deltaTime);
/*            if (lastTime != Time.fixedTime) {
                debugPoints.Add(Speed);
                debugTime.Add(Time.fixedTime);
                lastTime = Time.fixedTime;
                debugJump.Add(varJumpTimer);
            }*/
            transform.position = this.Position + collider.position;
            UpdateRender();
        }

        public void Die() {
            isAlive = false;
            PlayAnimation("Die");
        }

        public void SetState(int state) {
            this.stateMachine.State = state;
        }

        public void Jump() { 
            GameInput.JumpButton.ConsumeBuffer();
            JumpCheck.ResetTimer();
            WallSlideTimer = Constants.WallSlideTime;
            WallBoost?.ResetTime();
            varJumpTimer = Constants.VarJumpTime;
            Speed.x += Constants.JumpSpeed * moveX;
            Speed.y = Constants.JumpSpeed;
            varJumpSpeed = Speed.y;
            // TODO: Play Jump Animation and Sound and Particle
            PlayAnimation("Jump");
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

        private bool inSandevistan = false;
        public bool CanSandevistan {
            get {
                return GameInput.Sandevistan.Pressed()&& !inSandevistan;
            }
        }

        public void Sandevistan() {
            deltaTime = 1f / 120f;
            EffectManager.Instance.ToggleChromaticAberration(true);
            inSandevistan = true;
            StartCoroutine(JustForTestCountDown());
        }

        private IEnumerator JustForTestCountDown() {            
               yield return new WaitForSeconds(5f);
            deltaTime = 1f / 60f;
            EffectManager.Instance.ToggleChromaticAberration(false);
            inSandevistan = false;
        }

        public void WallJump(int dir) {
            GameInput.JumpButton.ConsumeBuffer();
            PlayAnimation("Jump");
            Ducking = false;
            JumpCheck?.ResetTimer();
            varJumpTimer = Constants.VarJumpTime;
            WallSlideTimer = Constants.WallSlideTime;
            WallBoost?.ResetTime();
            if (moveX != 0) {
                this.ForceMoveX = dir;
                this.ForceMoveXTimer = Constants.WallJumpForceTime;
            }
            
            Speed.x = Constants.WallJumpHSpeed * dir;
            Speed.y = Constants.JumpSpeed;
            //TODO ���ǵ��ݶ��ٶȵļӳ�
            //Speed += LiftBoost;
            varJumpSpeed = Speed.y;

            //ǽ������Ч����

        }

        

        public void ClimbJump() {
            if (!onGround) {
                //Stamina -= ClimbJumpCost;

                //sweatSprite.Play("jump", true);
                //Input.Rumble(RumbleStrength.Light, RumbleLength.Medium);
            }
            Jump();
            WallBoost?.Active();
        }

    }

}
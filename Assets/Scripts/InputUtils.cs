using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public struct virtualJoystick {
        public Vector2 Value { get => new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical")); }

    }

    public enum Facings {
        Right = 1,
        Left = -1
    }

    public struct VirtualButton {
        private KeyCode key;
        private KeyCode overloadKey;
        private float bufferTime;
        public float buffer { get => this.bufferCounter; }
        private bool consumed;
        private float bufferCounter;
        public VirtualButton(KeyCode key) : this(key, 0) {
        }
        public VirtualButton(KeyCode key, float bufferTime) {
            this.key = key;
            this.overloadKey = KeyCode.None;
            this.bufferTime = bufferTime;
            this.consumed = false;
            this.bufferCounter = 0f;
        }
        public void Overload(KeyCode key) { 
            this.overloadKey = key;
        }
        public void ConsumeBuffer() {
            this.bufferCounter = 0f;
        }
        public bool Pressed() {
            return UnityEngine.Input.GetKeyDown(key) || UnityEngine.Input.GetKeyDown(overloadKey) || (!this.consumed && (this.bufferCounter > 0f));
        }
        public bool Checked() {
            return UnityEngine.Input.GetKey(key)|| UnityEngine.Input.GetKey(overloadKey);
        }
        public void Update(float deltaTime) {
            this.consumed = false;
            this.bufferCounter -= deltaTime;
            bool flag = false;
            if (UnityEngine.Input.GetKeyDown(key) || UnityEngine.Input.GetKeyDown(overloadKey)) {
                this.bufferCounter = this.bufferTime;
                flag = true;
            } else if (UnityEngine.Input.GetKey(key) || UnityEngine.Input.GetKey(overloadKey)) {
                flag = true;
            }
            if (!flag) {
                this.bufferCounter = 0f;
                return;
            }
        }
    }

    public static class GameInput {
        public static VirtualButton JumpButton = new VirtualButton(KeyCode.Space, 0.08f);
        public static VirtualButton DashButton = new VirtualButton(KeyCode.L, 0.08f);
        public static VirtualButton AttackButton = new VirtualButton(KeyCode.J, 0.08f);
        public static VirtualButton ShootButton = new VirtualButton(KeyCode.C, 0.08f);
        public static VirtualButton HeavyAttackButton = new VirtualButton(KeyCode.K, 0.1f);
        // public static VirtualButton Sandevistan = new VirtualButton(KeyCode.T);
        public static VirtualButton GrabButton = new VirtualButton(KeyCode.LeftShift);
        public static VirtualButton SwitchItem = new VirtualButton(KeyCode.X);
        public static VirtualButton ConsumeButton = new VirtualButton(KeyCode.R,0.08f);
        public static virtualJoystick Joystick = new virtualJoystick();
        public static Vector2 LastAim;

        public static void Init() {
            JumpButton.Overload(KeyCode.Joystick1Button0);
            DashButton.Overload(KeyCode.Joystick1Button5);
            AttackButton.Overload(KeyCode.Joystick1Button2);
            HeavyAttackButton.Overload(KeyCode.Joystick1Button3);
            GrabButton.Overload(KeyCode.Joystick1Button4);
        }

        public static void Update(float deltaTime) {
            JumpButton.Update(deltaTime);
        }

        public static Vector2 GetAimVector(Facings defaultFacing = Facings.Right) {
            Vector2 value = GameInput.Joystick.Value;
            //TODO ¿¼ÂÇ¸¨ÖúÄ£Ê½

            //TODO ¿¼ÂÇÒ¡¸Ë
            if (value == Vector2.zero) {
                GameInput.LastAim = Vector2.right * ((int)defaultFacing);
            } else {
                GameInput.LastAim = value;
            }
            return GameInput.LastAim.normalized;
        }
    }
}

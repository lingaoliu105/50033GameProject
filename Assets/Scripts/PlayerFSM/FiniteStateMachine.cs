
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public enum EActionState {
        Normal,
        Dash,
        Climb,
        Attack,
        Combo,
        Hold,
        Size,
    }

    public abstract class BaseActionState {
        protected EActionState state;
        protected PlayerController player;

        protected BaseActionState(EActionState state, PlayerController context) {
            this.state = state;
            this.player = context;
        }

        public EActionState State { get => state; }

        //ÿһ֡��ִ�е��߼�
        public abstract EActionState Update(float deltaTime);

        public abstract IEnumerator Coroutine();

        public abstract void OnBegin();

        public abstract void OnEnd();

        public abstract bool IsCoroutine();
    }

    /// <summary>
    /// ����״̬��
    /// </summary>
    public class FiniteStateMachine<S> where S : BaseActionState {
        private S[] states;

        private int currState = -1;
        private int prevState = -1;
        private Coroutine currentCoroutine;

        public FiniteStateMachine(int size) {
            this.states = new S[size];
            this.currentCoroutine = new Coroutine(true);
        }

        public void AddState(S state) {
            Debug.Log($"====AddState[{(EActionState)state.State}]");
            this.states[(int)state.State] = state;
        }

        public void Update(float deltaTime) {
            State = (int)this.states[this.currState].Update(deltaTime);
            if (this.currentCoroutine.Active) {
                this.currentCoroutine.Update(deltaTime);
            }
        }

        public int State {
            get {
                return this.currState;
            }
            set {

                if (this.currState == value)
                    return;
                this.prevState = this.currState;
                this.currState = value;
                if (this.prevState != -1) {
                    Debug.Log($"====State[{(EActionState)this.prevState}] OnEnd ");
                    this.states[this.prevState].OnEnd();
                }
                Debug.Log($"====State[{(EActionState)this.currState}] OnBegin ");
                this.states[this.currState].OnBegin();
                Debug.Log(this.states[this.currState]);
                if (this.states[this.currState].IsCoroutine()) {
                    this.currentCoroutine.Replace(this.states[this.currState].Coroutine());
                    return;
                }
                this.currentCoroutine.Cancel();
            }
        }
    }
}

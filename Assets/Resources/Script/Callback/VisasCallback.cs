using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// 해당 이벤트 함수들은 비동기 타이밍에 대한 처리 편의성을 위해 만들었습니다.
// DoTween 에셋에서 사용되고 있는 DO~~~.OnComplete(()=>); 처리 시스템과 유사하게 제작하였습니다.
namespace Visas.GeneralCode
{
    [Serializable] public class OnValueChangeEvent : UnityEvent<string> { };
    [Serializable] public class OnIndexChangeEvent : UnityEvent<int> { };

    // 콜백 함수 호출 순서 (열기)
    // OnCreate / OnOpen
    // OnComplete

    // 콜백 함수 호출 순서 (닫기)
    // OnComplete
    // OnClose (모든 등록 이벤트 함수 제거)
    public delegate void VisasEventHandler();
    public delegate void VisasEventHandler<T>(T value);
    public delegate void VisasEventHandler<T1, T2>(T1 value1, T2 value2);

    // 호출 순서 
    // 1. .OnComplete() : 비동기 호출 예약
    // 2. .InvokeComplete() : 예약 함수가 내부에서 실행되는 타이밍 구간

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////// CompleteCallback 관련 부분 ////////// ////////// ////////// ////////// ////////// ////////// ////////// ////////// ////////// 

    // 이벤트 함수의 가장 베이스가 되는 단일 완료 콜백 클래스입니다.
    public class CompleteCallback : CustomYieldInstruction // 코루틴의 yield return Waiting 로직을 구현하기 위해 상속
    {
        public event VisasEventHandler onStart;                  // 행위가 시작되었을 경우의 이벤트 
        public event VisasEventHandler onComplete;               // 행위가 완료되었을 경우의 이벤트 원형 함수    

        protected bool _invokeStartQueue = false;
        protected bool _invokeCompleteQueue = false;              // InvokeComplete가 OnComplte보다 먼저 호출되었을 경우, 호출 예약 함수(onComplete 내용물)가 무엇인지 아직 알 수 없으므로 대기하기 위한 트리거 변수입니다.
        public override bool keepWaiting => (onComplete != null); // 예약 함수가 모두 호출 완료될 때까지 대기하는, WaitForCompletion 목적의 코루틴에 활용하기 위한 추가 변수 (MoveNext가 false를 반환할 때 까지 대기합니다.)
        public bool getIsFinishComplete => (onComplete == null);

        public virtual void Clear()
        {
            this.onStart = null;
            this.onComplete = null;
            this._invokeStartQueue = false;
            this._invokeCompleteQueue = false;
        }

        /// <summary>
        /// 코루틴의 yeild return 커스텀 등록을 위한 호출 함수입니다.
        /// </summary>
        /// <returns></returns>
        public CompleteCallback WaitForCompletionRoutine()
        {
            if (_invokeCompleteQueue == true) return this; // 이미 invoke가 호출된 상태라면 코루틴에서 대기할 필요가 없습니다.

            if (this.onComplete == null)    // OnComplete를 null로 만들지 않는 작업을 통해 InvokeComplete가 호출되기 전에 null인 현상을 방지합니다
                this.onComplete += () => { Debug.Log("onComplete에 더미값 추가"); };
            return this;
        }
        public CompleteCallback AddOnComplete(VisasEventHandler onComplete)
        {
            this.onComplete += onComplete;

            if (_invokeCompleteQueue == true)
                this.InvokeComplete();

            return this;
        }

        public CompleteCallback OnStart(VisasEventHandler onStart)
        {
            this.onStart = onStart;

            if (_invokeStartQueue == true)
                this.InvokeStart();

            return this;
        }
        public virtual void InvokeStart()
        {
            if (this.onStart == null) // 예외처리 : onComplete(호출 대상 실행함수)가 비어 있다면 onComplete를 받아올 때 까지 호출을 대기합니다.
            {
                _invokeStartQueue = true;
                return;
            }

            _invokeStartQueue = false;
            this.onStart.Invoke();
            this.onStart = null;
        }

        public CompleteCallback OnComplete(VisasEventHandler onComplete)
        {
            this.onComplete = onComplete;

            if (_invokeCompleteQueue == true)
                this.InvokeComplete();

            return this;
        }
        public virtual void InvokeComplete()
        {
            if (this.onComplete == null) // 예외처리 : onComplete(호출 대상 실행함수)가 비어 있다면 onComplete를 받아올 때 까지 호출을 대기합니다.
            {
                _invokeCompleteQueue = true;
                return;
            }

            _invokeCompleteQueue = false;
            this.onComplete.Invoke();
            this.onComplete = null;
        }
    }

    public class CompleteCallback<T> : CustomYieldInstruction // 코루틴의 yield return Waiting 로직을 구현하기 위해 상속
    {
        public event VisasEventHandler<T> onComplete;   // 비동기 결과가 완료되었을 경우의 이벤트    
        protected bool _invokeCompleteQueue = false;
        private T _invokeCompleteTarget = default;

        // 코루틴에 활용하기 위한 추가 변수
        public override bool keepWaiting => (onComplete != null); // MoveNext가 false를 반환할 때 까지 대기합니다.

        public virtual void Clear()
        {
            this.onComplete = null;
            _invokeCompleteQueue = false;
            _invokeCompleteTarget = default;
        }

        public CompleteCallback<T> WaitForCompletionRoutine()
        {
            if (_invokeCompleteQueue == true) return this; // 이미 invoke가 호출된 상태라면 코루틴에서 대기할 필요가 없습니다.

            if (this.onComplete == null)
                this.onComplete += (T) => { Debug.Log($"CompleteCallback<T> : 코루틴 대기모드 더미 세팅해 호출시 예외 방지"); }; // OnComplete를 null로 만들지 않는 작업을 통해 이벤트를 반드시 호출됩니다.
            return this;
        }

        public CompleteCallback<T> OnComplete(VisasEventHandler<T> onComplete)
        {
            this.onComplete = onComplete;

            if (_invokeCompleteQueue == true)
            {
                this.InvokeComplete(_invokeCompleteTarget);
                _invokeCompleteTarget = default;
            }
            return this;
        }
        public virtual void InvokeComplete(T value)
        {
            if (this.onComplete == null) // 예외처리 : onComplete(호출 대상 실행함수)가 비어 있다면 onComplete를 받아올 때 까지 호출을 대기합니다.
            {
                _invokeCompleteQueue = true;
                _invokeCompleteTarget = value;
                return;
            }

            this.onComplete.Invoke(value);
            this.onComplete = null;
            _invokeCompleteQueue = false;
        }
    }

    public class CompleteCallback<T1, T2> : CustomYieldInstruction // 코루틴의 yield return Waiting 로직을 구현하기 위해 상속
    {
        public event VisasEventHandler<T1, T2> onComplete;   // 비동기 결과가 완료되었을 경우의 이벤트    
        protected bool _invokeCompleteQueue = false;
        private T1 _invokeCompleteTarget1 = default;
        private T2 _invokeCompleteTarget2 = default;

        // 코루틴에 활용하기 위한 추가 변수
        public override bool keepWaiting => (onComplete != null); // MoveNext가 false를 반환할 때 까지 대기합니다.

        public virtual void Clear()
        {
            this.onComplete = null;
            _invokeCompleteQueue = false;
            _invokeCompleteTarget1 = default;
            _invokeCompleteTarget2 = default;
        }

        public CompleteCallback<T1, T2> OnComplete(VisasEventHandler<T1, T2> onComplete)
        {
            this.onComplete = onComplete;

            if (_invokeCompleteQueue == true)
            {
                this.InvokeComplete(_invokeCompleteTarget1, _invokeCompleteTarget2);
                _invokeCompleteTarget1 = default;
                _invokeCompleteTarget2 = default;
            }
            return this;
        }
        public virtual void InvokeComplete(T1 value1, T2 value2)
        {
            if (this.onComplete == null) // 예외처리 : onComplete(호출 대상 실행함수)가 비어 있다면 onComplete를 받아올 때 까지 호출을 대기합니다.
            {
                _invokeCompleteQueue = true;
                _invokeCompleteTarget1 = value1;
                _invokeCompleteTarget2 = value2;
                return;
            }

            this.onComplete?.Invoke(value1, value2);
            this.onComplete = null;
            _invokeCompleteQueue = false;
        }
    }

    ////////// ////////// ////////// ////////// ////////// ////////// ////////// ////////// ////////// CompleteCallback 관련 부분 ////////// 
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    /// <summary>
    /// 패널에 사용하기 위해 상속한 이벤트 클래스입니다. (등록된 이벤트는 1회성으로 한번 호출된 이후 소멸합니다.)
    /// </summary>
    public class PanelCallback : CompleteCallback
    {
        // 패널 목적의 이벤트
        public event VisasEventHandler onOpen;     // 여는 행위가 일어났을 경우의 이벤트     
        public event VisasEventHandler onClose;    // 닫는 행위가 일어났을 경우의 이벤트 (최종 마무리 콜백으로 호출 이후 초기화)
        public event VisasEventHandler onCreate; // 생성 행위가 완료되었을 경우의 이벤트

        protected bool _invokeOpenQueue = false;
        protected bool _invokeCloseQueue = false;
        protected bool _invokeCreateQueue = false;

        public override void Clear()
        {
            base.Clear();

            this.onOpen = null;
            this.onClose = null;
            this.onCreate = null;
            _invokeOpenQueue = false;
            _invokeCloseQueue = false;
            _invokeCreateQueue = false;
        }

        public PanelCallback OnOpen(VisasEventHandler onOpen)
        {
            this.onOpen = onOpen;

            if (_invokeOpenQueue == true)
                this.InvokeOpen();

            return this;
        }
        public PanelCallback OnClose(VisasEventHandler onClose)
        {
            this.onClose = onClose;

            if (_invokeCloseQueue == true)
                this.InvokeClose();

            return this;
        }

        public PanelCallback OnCreate(VisasEventHandler onCreate)
        {
            this.onCreate = onCreate;

            if (_invokeCreateQueue == true)
                this.InvokeCreate();

            return this;
        }

        public void InvokeCreate()
        {
            if (this.onCreate == null)
            {
                _invokeCreateQueue = true;
                return;
            }

            _invokeCreateQueue = false;
            this.onCreate?.Invoke();
            this.onCreate = null;
        }
        public void InvokeOpen()
        {
            if (this.onOpen == null)
            {
                _invokeOpenQueue = true;
                return;
            }

            _invokeOpenQueue = false;
            this.onOpen?.Invoke();
            this.onOpen = null;
        }
        public virtual void InvokeClose()
        {
            if (this.onClose == null)
            {
                _invokeCloseQueue = true;
                return;
            }

            _invokeCloseQueue = false;
            this.onClose?.Invoke();
            this.onClose = null;
        }
    }






    /// <summary>
    /// 팝업에 사용하기 위해 Ok, Cancle 버튼에 대응하는 이벤트가 추가
    /// </summary>
    public class PopupCallback : PanelCallback
    {
        // 팝업의 용도로 사용하는 이벤트
        public event VisasEventHandler onPositive; // 긍정적인 액션이 일어났을 경우의 이벤트 ex) 팝업의 예
        public event VisasEventHandler onNagative; // 부정적인 액션이 일어났을 경우의 이벤트 ex) 팝업의 아니오

        public PopupCallback OnPositive(VisasEventHandler onPositive)
        {
            this.onPositive = onPositive;
            return this;
        }
        public PopupCallback OnNagative(VisasEventHandler onNagative)
        {
            this.onNagative = onNagative;
            return this;
        }

        // 팝업 버튼의 경우 팝업이 닫히지 않는 한, 중복 버튼 클릭이 가능하므로 Invoke를 즉시 소멸시키지 않습니다.
        public void InvokePositive()
        {
            onPositive?.Invoke();
        }
        public void InvokeNagative()
        {
            onNagative?.Invoke();
        }
        // 수동 소멸 함수입니다.
        public void ResetPopupCallback()
        {
            onPositive = null;
            onNagative = null;
        }
    }


    /// <summary>퍼센테이지를 지속적으로 처리하기 위해 onUpdate 콜백이 추가된 클래스</summary>
    public class ProcessCallback : CompleteCallback
    {
        public event VisasEventHandler onUpdate;

        public ProcessCallback OnUpdate(VisasEventHandler onUpdate)
        {
            this.onUpdate = onUpdate;
            return this;
        }

        // 업데이트는 완료될 때까지 호출하므로 Invoke를 즉시 소멸시키지 않습니다.
        public void InvokeUpdate() => onUpdate?.Invoke();

        // 완료 되면 업데이트 콜백을 소멸시킵니다.
        public override void InvokeComplete()
        {
            base.InvokeComplete();
            onUpdate = null;
        }
    }

    /// <summary>퍼센테이지를 지속적으로 처리하기 위해 onUpdate 콜백이 추가된 템플릿 클래스</summary>
    public class ProcessCallback<T> : CompleteCallback<T>
    {
        public event VisasEventHandler<T> onUpdate;

        public ProcessCallback<T> OnUpdate(VisasEventHandler<T> onUpdate)
        {
            this.onUpdate = onUpdate;
            return this;
        }

        // 업데이트는 완료될 때까지 호출하므로 Invoke를 즉시 소멸시키지 않습니다.
        public void InvokeUpdate(T value) => onUpdate?.Invoke(value);

        // 완료 되면 업데이트 콜백을 소멸시킵니다.
        public override void InvokeComplete(T value)
        {
            base.InvokeComplete(value);
            onUpdate = null;
        }
    }
}
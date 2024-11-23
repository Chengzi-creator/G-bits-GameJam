
    using GameFramework.Event;
    using UnityEditor;
    using UnityEngine.UI;
    using UnityGameFramework.Runtime;

    public class SleepValueUI: UIFormLogic
    {
        public Slider SleepValueSlider;
        public float speed;
        public float valueMax = 100;
        public float value;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(WindowShowEventArgs.EventId, OnValueSpeedChange);
            GameEntry.Event.Subscribe(WindowBeClickEventArgs.EventId, OnWindowBeClicked);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            value += speed / 60 * elapseSeconds;
            if (value > valueMax)
            {
                value = valueMax;
            }

            SleepValueSlider.value = value / valueMax;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(WindowShowEventArgs.EventId, OnValueSpeedChange);
            GameEntry.Event.Unsubscribe(WindowBeClickEventArgs.EventId, OnWindowBeClicked);
        }


        void OnValueSpeedChange(object sender, GameEventArgs e)
        {
            WindowShowEventArgs ne = e as WindowShowEventArgs;
            if (ne == null)
            {
                return;
            }
            float valueSpeed = ne.SleepSpeedValue;
            speed += valueSpeed;
        }
        
        void OnWindowBeClicked(object sender, GameEventArgs e)
        {
            WindowBeClickEventArgs ne = e as WindowBeClickEventArgs;
            if (ne == null)
            {
                return;
            }
            float valueSpeed = ne.SleepSpeedReduce;
            speed -= valueSpeed;
        }
    }

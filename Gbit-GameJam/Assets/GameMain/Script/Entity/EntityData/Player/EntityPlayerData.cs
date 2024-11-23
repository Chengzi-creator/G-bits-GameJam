
    using GameFramework;
    using UnityEngine;
    using UnityEngine.PlayerLoop;

    public class EntityPlayerData: IReference
    {
        
        public Vector2 InitPosition { get; set; }
        
        public EntityPlayerData Init(Vector2 initPosition)
        {
            InitPosition = initPosition;
            return this;
        }
        
        public void Clear()
        {
            InitPosition = Vector2.zero;
        }
        
        public static EntityPlayerData Create(Vector2 initPosition)
        {
            EntityPlayerData playerData = ReferencePool.Acquire<EntityPlayerData>();
            return playerData.Init(initPosition);
        }
    }

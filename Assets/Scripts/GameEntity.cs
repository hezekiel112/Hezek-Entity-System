using System;
using UnityEngine;

#if UNITY_EDITOR
using NaughtyAttributes;
#endif

namespace HezekEntitySystem{
    public class GameEntity : MonoBehaviour, IGameEntity, IEquatable<GameEntity>{
        [SerializeField] [Header("Data :")] private string _guid;
        [SerializeField] private GameEntityData _current;
        
        public string GUID
        {
            get => _guid;
        }

        public GameEntityData Current
        {
            get => _current;
        }
        
        public virtual void OnEntityStart(){
            print(name+" "+"started");
            WhenEntityStarted?.Invoke();
        }

        public virtual void OnEntityUpdated(){
            WhenEntityUpdate?.Invoke();
        }

        public override string ToString(){
            return _guid;
        }

        public override bool Equals(object other){
            return _guid.Equals(other);
        }

        public bool Equals(GameEntity other){
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return base.Equals(other) && _guid == other._guid;
        }

        public override int GetHashCode(){
            return HashCode.Combine(base.GetHashCode(), GUID);
        }

        public event Action WhenEntityStarted, WhenEntityUpdate;
        
        #if UNITY_EDITOR
        [Button("Generate New GUID")]
        private void GenerateGUID(){
            _guid = $"{Guid.NewGuid().ToString()}".ToUpper();
        }
        #endif
    }
}
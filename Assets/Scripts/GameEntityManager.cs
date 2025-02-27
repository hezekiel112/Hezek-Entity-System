using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace HezekEntitySystem 
{
    public sealed class GameEntityManager : MonoBehaviour{

        public static GameEntityManager Instance
        {
            get;
            set;
        }
        
        private IEnumerator[] _runSequence = null;

        [SerializeField] [Header("Data :")] private GameEntity[] _entity;

        public GameEntity[] Entity
        {
            get => _entity;
        }
        
        public event Action<GameEntity> WhenInitializeEntity, WhenLoadEntity, WhenStartEntity, WhenUpdateEntity;//, WhenDestroyEntity;
        
        
        private readonly Hashtable _entityHashset = new Hashtable();

        [SerializeField] private bool _isInitialized = true;
        
        public bool IsInitialized
        {
            get => _isInitialized;
        }
        
        private void Awake(){
            Instance = this;

            WhenInitializeEntity += (e) =>
            {
                print($"{nameof(WhenInitializeEntity)}: {e.name}");
            };

            WhenLoadEntity += (e) =>
            {
                print($"{nameof(WhenLoadEntity)}: {e.name}");
            };

            WhenStartEntity += (e) =>
            {
                print($"{nameof(WhenStartEntity)}: {e.name}");
            };

            WhenUpdateEntity += (e) =>
            {
                print($"{nameof(WhenUpdateEntity)}: {e.name}");
            };
            
            _runSequence = new[]
            {
                InitializeEntity(_entity),
                LoadEntity(_entity),
            };

            
            Run();
        }
        
        private void Run(){
            for (int i = 0; i < _runSequence.Length; i++)
            {
                var current = _runSequence[i].MoveNext();

                if (!current)
                {
                    _isInitialized = false;
                    print("<color=red>GameEntityManager failed to run sequence </color>");
                    return;
                }
            }

            List<Action> entityStart = Entity.Select(e => (Action)e.OnEntityStart).ToList();

            if (entityStart.Count == 0) return;
            
            foreach (var e in entityStart)
            {
                e.Invoke();
            }
        }
        
        /// <summary>
        /// Run check sequence checking if all entities is ready to operate.
        /// </summary>
        private IEnumerator InitializeEntity(GameEntity[] entity){
            _entityHashset.Clear();
            
            foreach (var e in entity)
            {
                if (string.IsNullOrEmpty(e.GUID))
                {
                    print($"{e.name} <color=red>GUID is empty.</color>");

                    yield break;
                }

                WhenInitializeEntity?.Invoke(e);
            }
            
            yield return true;
        }
        
        /// <summary>
        /// Load all entity in memory and creates a hashset collection of them.
        /// </summary>
        private IEnumerator LoadEntity(GameEntity[] entity){
            foreach (var e in entity)
            {
                _entityHashset.Add(e.GUID, e);
                
                WhenLoadEntity?.Invoke(e);
                StartCoroutine(StartEntity(e.gameObject));
            }
            yield return true;
        }
        
        /// <summary>
        /// Instantiate entity on scene.
        /// </summary>
        private IEnumerator StartEntity(GameObject entity){
            Instantiate(entity, entity.transform.position, entity.transform.rotation);
            yield return true;
        }
    }
}
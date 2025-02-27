using UnityEngine;

namespace HezekEntitySystem{
    [CreateAssetMenu(menuName = "Hezek's Entity System/New Game Entity Data")]
    public class GameEntityData : ScriptableObject{
        [SerializeField] [Header("Informations :")]
        private int _id = 000;

        public int ID
        {
            get => _id;
        }

        [SerializeField] private string _entityName = "";

        public string EntityName
        {
            get => _entityName;
        }
        
        [SerializeField] private string _entityDescription = "";

        public string EntityDescription
        {
            get => _entityDescription;
        }
        
        [SerializeField] private EEntityType _entityType;

        public EEntityType EntityType
        {
            get => _entityType;
        }
    }
}
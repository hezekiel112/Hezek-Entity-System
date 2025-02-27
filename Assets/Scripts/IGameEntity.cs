namespace HezekEntitySystem{
    public interface IGameEntity{
        void OnEntityStart();
        void OnEntityUpdated();

        string GUID
        {
            get;
        }
    }
}
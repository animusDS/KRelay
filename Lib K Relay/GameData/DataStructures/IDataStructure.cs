namespace Lib_K_Relay.GameData.DataStructures
{
    public interface IDataStructure<TIdType>
    {
        string Name { get; }
        TIdType Id { get; }
    }
}
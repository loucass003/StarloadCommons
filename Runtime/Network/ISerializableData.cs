namespace StarloadCommons.Network
{
    public interface ISerializableData<in T>
    {
        void Read(T buf);
        void Write(T buf);

        int Size();
    }
}
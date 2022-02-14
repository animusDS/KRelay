namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class Status : IDataObject
    {
        public StatData[] Data;
        public int ObjectId;
        public Location Position = new Location();

        public IDataObject Read(PacketReader r)
        {
            ObjectId = CompressedInt.Read(r);
            Position.Read(r);
            Data = new StatData[CompressedInt.Read(r)];

            for (var i = 0; i < Data.Length; i++)
            {
                var statData = new StatData();
                statData.Read(r);
                Data[i] = statData;
            }

            return this;
        }

        public void Write(PacketWriter w)
        {
            CompressedInt.Write(w, ObjectId);
            Position.Write(w);
            CompressedInt.Write(w, Data.Length);

            foreach (var statData in Data)
                statData.Write(w);
        }

        public object Clone()
        {
            return new Status
            {
                Data = (StatData[])Data.Clone(),
                ObjectId = ObjectId,
                Position = (Location)Position.Clone()
            };
        }
    }
}
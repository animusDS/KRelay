namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class Tile : IDataObject
    {
        public ushort Type;
        public short X;
        public short Y;

        public IDataObject Read(PacketReader r)
        {
            X = r.ReadInt16();
            Y = r.ReadInt16();
            Type = r.ReadUInt16();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Type);
        }

        public object Clone()
        {
            return new Tile
            {
                X = X,
                Y = Y,
                Type = Type
            };
        }

        public override string ToString()
        {
            return "{ X=" + X + ", Y=" + Y + ", Type=" + Type + " }";
        }
    }
}
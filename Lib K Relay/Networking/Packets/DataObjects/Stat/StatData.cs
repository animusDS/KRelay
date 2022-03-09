using System;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.DataObjects.Stat
{
    public class StatData : IDataObject {
        public StatsType Id;
        public int IntValue;
        public int SecondaryValue;
        public string StringValue;

        public IDataObject Read(PacketReader r) {
            Id = r.ReadByte();
            if (IsStringData())
                StringValue = r.ReadString();
            else
                IntValue = CompressedInt.Read(r);

            SecondaryValue = CompressedInt.Read(r);

            return this;
        }

        public void Write(PacketWriter w) {
            w.Write(Id);
            if (IsStringData())
                w.Write(StringValue);
            else
                CompressedInt.Write(w, IntValue);

            CompressedInt.Write(w, SecondaryValue);
        }

        public object Clone() {
            return new StatData {
                Id = Id,
                IntValue = IntValue,
                StringValue = StringValue,
                SecondaryValue = SecondaryValue
            };
        }

        public bool IsStringData() {
            return Id.IsUtf();
        }

        public override string ToString() {
            return "{ Id=" + Id + " Value=" + (IsStringData() ? StringValue : IntValue.ToString()) +
                   " SecondaryValue=" + SecondaryValue + " }";
        }
    }

    public partial class StatsType
    {
        private readonly byte _mType;
        
        private StatsType(byte type)
        {
            _mType = type;
        }

        public bool IsUtf()
        {
            return (int)this == (int)Stats.Name
                   || (int)this == (int)Stats.Experience
                   || (int)this == (int)Stats.AccountId
                   || (int)this == (int)Stats.OwnerAccountId
                   || (int)this == (int)Stats.GuildName
                   || (int)this == (int)Stats.Skin
                   || (int)this == (int)Stats.PetName
                   || (int)this == (int)Stats.GraveAccountId
                   || (int)this == (int)Stats.DungeonModifiers;
        }

        public static implicit operator StatsType(int type)
        {
            if (type > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return new StatsType((byte)type);
        }

        public static implicit operator StatsType(byte type)
        {
            return new StatsType(type);
        }

        public static bool operator ==(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return !(type is null) && type._mType == (byte)id;
        }

        public static bool operator ==(StatsType type, byte id)
        {
            return !(type is null) && type._mType == id;
        }

        public static bool operator !=(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return !(type is null) &&
                   type._mType != (byte)id;
        }

        public static bool operator !=(StatsType type, byte id)
        {
            return !(type is null) && 
                   type._mType != id;
        } 

        public static bool operator ==(StatsType type, StatsType id)
        {
            return !(id is null) && !(type is null) &&
                   type._mType == id._mType;
        }

        public static bool operator !=(StatsType type, StatsType id)
        {
            return !(id is null) && !(type is null) &&
                   type._mType != id._mType;
        }

        public static implicit operator int(StatsType type)
        {
            return type._mType;
        }

        public static implicit operator byte(StatsType type)
        {
            return type._mType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StatsType type)) return false;

            return this == type;
        }
        
        public override int GetHashCode()
        {
            return _mType.GetHashCode();
        }

        public override string ToString()
        {
            return _mType.ToString();
        }
    }
}
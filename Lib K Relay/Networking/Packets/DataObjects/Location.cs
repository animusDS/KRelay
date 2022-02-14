using System;

namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class Location : IDataObject
    {
        public float X;
        public float Y;

        public Location()
        {
        }

        public Location(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Location Empty =>
            new Location
            {
                X = 0,
                Y = 0
            };

        public virtual IDataObject Read(PacketReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();

            return this;
        }

        public virtual void Write(PacketWriter w)
        {
            w.Write(X);
            w.Write(Y);
        }

        public virtual object Clone()
        {
            return new Location
            {
                X = X,
                Y = Y
            };
        }

        public float DistanceSquaredTo(Location location)
        {
            var dx = location.X - X;
            var dy = location.Y - Y;
            return dx * dx + dy * dy;
        }

        public float DistanceTo(Location location)
        {
            return (float)Math.Sqrt(DistanceSquaredTo(location));
        }

        private float GetAngle(Location l1, Location l2)
        {
            var dX = l2.X - l1.X;
            var dY = l2.Y - l1.Y;
            return (float)Math.Atan2(dY, dX);
        }

        private float GetAngle(float x1, float y1, float x2, float y2)
        {
            var dX = x2 - x1;
            var dY = y2 - y1;
            return (float)Math.Atan2(dY, dX);
        }

        public override string ToString()
        {
            return "{ X=" + X + ", Y=" + Y + " }";
        }
    }
}
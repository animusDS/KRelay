//The MIT License (MIT)
//
//Copyright (c) 2015 Fabian Fischer
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.GameData;
using Newtonsoft.Json;

namespace MapRipper
{
    public class JsonMap
    {
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[][] Tiles { get; private set; }
        public Entity[][][] Entities { get; private set; }

        private int currentTiles;

        public delegate void TilesChangedDelegate(int currentTiles);

        public event TilesChangedDelegate TilesAdded;

        public void Init(int w, int h, string name)
        {
            Width = w;
            Height = h;
            Tiles = new int[w][];
            Name = name;
            for (int i = 0; i < w; i++) Tiles[i] = new int[h];

            for (int w_ = 0; w_ < w; w_++)
            for (int h_ = 0; h_ < h; h_++)
                Tiles[w_][h_] = -1;

            Entities = new Entity[w][][];
            for (int i = 0; i < w; i++)
            {
                Entities[i] = new Entity[h][];
                for (int j = 0; j < h; j++)
                {
                    Entities[i][j] = new Entity[0];
                }
            }
        }

        private struct obj
        {
            public string name;
            public string id;
        }

        private struct loc
        {
            public string ground;
            public obj[] objs;
            public obj[] regions;
        }

        private struct json_dat
        {
            public byte[] data;
            public int width;
            public int height;
            public loc[] dict;
        }

        public void Update(UpdatePacket packet)
        {
            foreach (var t in (packet as UpdatePacket).Tiles)
            {
                this.Tiles[t.X][t.Y] = t.Type;
                currentTiles++;
                if (TilesAdded != null) TilesAdded(this.currentTiles);
            }

            foreach (var tileDef in (packet as UpdatePacket).NewObjs)
            {
                var def = (Entity)tileDef.Clone();

                if (isMapObject(def.ObjectType))
                {
                    def.Status.Position.X -= 0.5F;
                    def.Status.Position.Y -= 0.5F;

                    int _x = (int)def.Status.Position.X;
                    int _y = (int)def.Status.Position.Y;
                    Array.Resize(ref this.Entities[_x][_y], this.Entities[_x][_y].Length + 1);
                    Entity[] arr = this.Entities[_x][_y];

                    arr[arr.Length - 1] = def;
                }
            }
        }

        private bool isMapObject(ushort objType)
        {
            return true; // Todo: check if player or pet or all that stuff you dont place on the map with the editor
        }

        public string ToJson()
        {
            var obj = new json_dat();
            obj.width = Width;
            obj.height = Height;
            List<loc> locs = new List<loc>();
            MemoryStream ms = new MemoryStream();
            using (PacketWriter wtr = new PacketWriter(ms))
                for (int y = 0; y < obj.height; y++)
                for (int x = 0; x < obj.width; x++)
                {
                    var loc = new loc();
                    loc.ground = Tiles[x][y] != -1 ? GetTileId((ushort)Tiles[x][y]) : null;
                    loc.objs = new obj[Entities[x][y].Length];
                    for (int i = 0; i < loc.objs.Length; i++)
                    {
                        var en = Entities[x][y][i];
                        obj o = new obj() { id = GetEntityId(en.ObjectType) };
                        string s = "";
                        Dictionary<StatsType, object> vals = new Dictionary<StatsType, object>();
                        foreach (var z in en.Status.Data)
                            vals.Add(z.Id, z.IsStringData() ? (object)z.StringValue : (object)z.IntValue);
                        if (vals.ContainsKey((int)Stats.Name)) s += ";name:" + vals[(int)Stats.Name];
                        if (vals.ContainsKey((int)Stats.Size)) s += ";size:" + vals[(int)Stats.Size];
                        if (vals.ContainsKey((int)Stats.ObjectConnection))
                            s += ";conn:0x" + ((int)vals[(int)Stats.ObjectConnection]).ToString("X8");
                        if (vals.ContainsKey((int)Stats.MerchandiseType))
                            s += ";mtype:" + vals[(int)Stats.MerchandiseType];
                        if (vals.ContainsKey((int)Stats.MerchandiseRemainingCount))
                            s += ";mcount:" + vals[(int)Stats.MerchandiseRemainingCount];
                        if (vals.ContainsKey((int)Stats.MerchandiseRemainingMinutes))
                            s += ";mtime:" + vals[(int)Stats.MerchandiseRemainingMinutes];
                        if (vals.ContainsKey((int)Stats.RankRequired)) s += ";nstar:" + vals[(int)Stats.RankRequired];
                        o.name = s.Trim(';');
                        loc.objs[i] = o;
                    }

                    int ix = -1;
                    for (int i = 0; i < locs.Count; i++)
                    {
                        if (locs[i].ground != loc.ground) continue;
                        if (!((locs[i].objs != null && loc.objs != null) || (locs[i].objs == null && loc.objs == null)))
                            continue;
                        if (locs[i].objs != null)
                        {
                            if (locs[i].objs.Length != loc.objs.Length) continue;
                            bool b = false;
                            for (int j = 0; j < loc.objs.Length; j++)
                                if (locs[i].objs[j].id != loc.objs[j].id || locs[i].objs[j].name != loc.objs[j].name)
                                {
                                    b = true;
                                    break;
                                }

                            if (b) continue;
                        }

                        ix = i;
                        break;
                    }

                    if (ix == -1)
                    {
                        ix = locs.Count;
                        locs.Add(loc);
                    }

                    wtr.Write((short)ix);
                }

            obj.data = ZlibStream.CompressBuffer(ms.ToArray());
            obj.dict = locs.ToArray();
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj, settings);
        }

        private string GetEntityId(ushort type)
        {
            // return entity id of type tiles or entities
            return GameData.Tiles.Map.ContainsKey((ushort)type) ? GameData.Tiles.ByID((ushort)type).Name : GameData.Objects.ByID((ushort)type).Name;
        }

        private string GetTileId(ushort type)
        {
            return GameData.Tiles.ByID(type).Name;
        }
    }
}
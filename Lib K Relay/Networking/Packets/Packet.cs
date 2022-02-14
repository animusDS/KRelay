using System;
using System.IO;
using System.Reflection;
using System.Text;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets
{
    public class Packet
    {
        private byte[] _data;
        public byte Id;
        public bool Send = true;
        public byte[] UnreadData = new byte[0];

        public virtual PacketType Type => PacketType.UNKNOWN;

        public virtual void Read(PacketReader r)
        {
            _data = r.ReadBytes((int)r.BaseStream.Length - 5); // All of the packet data
        }

        public virtual void Write(PacketWriter w)
        {
            w.Write(_data); // All of the packet data
        }

        public static Packet Create(PacketType type)
        {
            var st = GameData.GameData.Packets.ByName(type.ToString());
            var packet = (Packet)Activator.CreateInstance(st.Type);
            packet.Id = st.ID;
            return packet;
        }

        public static T Create<T>(PacketType type)
        {
            var packet = (Packet)Activator.CreateInstance(typeof(T));
            packet.Id = GameData.GameData.Packets.ByName(type.ToString()).ID;
            return (T)Convert.ChangeType(packet, typeof(T));
        }

        public T To<T>()
        {
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public static Packet Create(byte[] data)
        {
            using (var r = new PacketReader(new MemoryStream(data)))
            {
                r.ReadInt32(); // Skip over int length
                var id = r.ReadByte();

                // 254 = We don't have the packet defined, log data and send back
                var st = GameData.GameData.Packets.ByID(
                    !GameData.GameData.Packets.Map.ContainsKey(id) ? (byte)254 : id);
                var type = st.Type;

                // Reflect the type to a new instance and read its data from the PacketReader
                var packet = (Packet)Activator.CreateInstance(type);
                packet.Id = id;
                packet.Read(r);

                // Handle all unprocessed bytes in order to ensure packet integrity
                if (r.BaseStream.Position != r.BaseStream.Length)
                {
                    var len = r.BaseStream.Length - r.BaseStream.Position;
                    packet.UnreadData = new byte[len];
                    var msg = "Packet has unread data left over: " +
                              "Id=" + packet.Id + ", Data=[";
                    for (var i = 0; i < len; i++)
                    {
                        packet.UnreadData[i] = r.ReadByte();
                        msg += packet.UnreadData[i] + (i == len - 1 ? "]" : ",");
                    }

                    PluginUtils.Log("Packet", msg);
                }

                return packet;
            }
        }

        public override string ToString()
        {
            // Use reflection to get the packet's fields and values so we don't have
            // to formulate a ToString method for every packet type.
            var fields = GetType().GetFields(BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance);

            var s = new StringBuilder();
            s.Append(Type + "(" + Id + ") Packet Instance");
            foreach (var f in fields) s.Append("\n\t" + f.Name + " => " + f.GetValue(this));

            return s.ToString();
        }

        public string ToStructure()
        {
            // Use reflection to build a list of the packet's fields.
            var fields = GetType().GetFields(BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance);

            var s = new StringBuilder();
            s.Append(Type + " [" + GameData.GameData.Packets.ByName(Type.ToString()).ID + "] \nPacket Structure:\n{");
            foreach (var f in fields) s.Append("\n  " + f.Name + " => " + f.FieldType.Name);

            s.Append("\n}");
            return s.ToString();
        }
    }

    public enum PacketType
    {
        UNKNOWN,
        FAILURE,
        CREATE_SUCCESS,
        CREATE,
        PLAYERSHOOT,
        MOVE,
        PLAYERTEXT,
        TEXT,
        SERVERPLAYERSHOOT,
        DAMAGE,
        UPDATE,
        UPDATEACK,
        NOTIFICATION,
        NEWTICK,
        INVSWAP,
        USEITEM,
        SHOWEFFECT,
        HELLO,
        GOTO,
        INVDROP,
        INVRESULT,
        RECONNECT,
        PING,
        PONG,
        MAPINFO,
        LOAD,
        PIC,
        SETCONDITION,
        TELEPORT,
        USEPORTAL,
        DEATH,
        BUY,
        BUYRESULT,
        AOE,
        GROUNDDAMAGE,
        PLAYERHIT,
        ENEMYHIT,
        AOEACK,
        SHOOTACK,
        OTHERHIT,
        SQUAREHIT,
        GOTOACK,
        EDITACCOUNTLIST,
        ACCOUNTLIST,
        QUESTOBJID,
        CHOOSENAME,
        NAMERESULT,
        CREATEGUILD,
        GUILDRESULT,
        GUILDREMOVE,
        GUILDINVITE,
        ALLYSHOOT,
        ENEMYSHOOT,
        REQUESTTRADE,
        TRADEREQUESTED,
        TRADESTART,
        CHANGETRADE,
        TRADECHANGED,
        ACCEPTTRADE,
        CANCELTRADE,
        TRADEDONE,
        TRADEACCEPTED,
        CLIENTSTAT,
        CHECKCREDITS,
        ESCAPE,
        FILE,
        INVITEDTOGUILD,
        JOINGUILD,
        CHANGEGUILDRANK,
        PLAYSOUND,
        GLOBAL_NOTIFICATION,
        RESKIN,
        PETUPGRADEREQUEST,
        ACTIVE_PET_UPDATE_REQUEST,
        NEW_ABILITY,
        EVOLVE_PET,
        DELETE_PET,
        HATCH_PET,
        ENTER_ARENA,
        IMMINENT_ARENA_WAVE,
        ARENA_DEATH,
        VERIFY_EMAIL,
        RESKIN_UNLOCK,
        PASSWORD_PROMPT,
        QUEST_REDEEM,
        QUEST_REDEEM_RESPONSE,
        KEY_INFO_REQUEST,
        KEY_INFO_RESPONSE,
        QUEST_ROOM_MSG,
        PET_CHANGE_SKIN_MSG,
        REALM_HERO_LEFT_MSG,
        RESET_DAILY_QUESTS,
        NEW_CHARACTER_INFORMATION,
        UNLOCK_INFORMATION,
        QUEUE_INFORMATION,
        QUEUE_CANCEL,
        EXALTATION_BONUS_CHANGED,
        REDEEM_EXALTATION_REWARD,
        VAULT_CONTENT,
        FORGE_REQUEST,
        FORGE_RESULT,
        FORGE_UNLOCKED_BLUEPRINTS,
        SHOOTACK_COUNTER,
        CHANGE_ALLY_SHOOT,
        CREEP_MOVE_MESSAGE,
        REFINEMENTRESULT,
        BUYREFINEMENT,
        KENSEIDASH,
        KENSEIDASHACK,

        // not actually needed, chat server is separate
        CHAT_HELLO_MSG,
        CHAT_TOKEN_MSG,

        UNDEFINED
    }
}
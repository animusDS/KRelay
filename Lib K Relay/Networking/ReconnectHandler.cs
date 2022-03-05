using System.Text;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking
{
    public class ReconnectHandler
    {
        private Proxy _proxy;

        public void Attach(Proxy proxy)
        {
            _proxy = proxy;
            proxy.HookPacket<CreateSuccessPacket>(OnCreateSuccess);
            proxy.HookPacket<ReconnectPacket>(OnReconnect);
            proxy.HookPacket<HelloPacket>(OnHello);

            proxy.HookCommand("con", OnConnectCommand);
            proxy.HookCommand("ip", OnIpCommand);
            proxy.HookCommand("goto", OnGotoCommand);
        }

        private void OnHello(Client client, HelloPacket packet)
        {
            client.State = _proxy.GetState(client, packet.Key);
            if (client.State.ConRealKey.Length != 255) // todo: very scuffed, but needed for /con
            {
                packet.Key = client.State.ConRealKey;
                client.State.ConRealKey = new byte[255];
            }

            client.Connect(packet);
            packet.Send = false;
        }

        private void OnCreateSuccess(Client client, CreateSuccessPacket packet)
        {
            PluginUtils.Delay(1000, () =>
            {
                var message = "Welcome to K Relay!";
                client.SendToClient(PluginUtils.CreateNotification(message));
            });
        }

        private void OnReconnect(Client client, ReconnectPacket packet)
        {
            var recon = (ReconnectPacket)Packet.Create(PacketType.RECONNECT);
            recon.GameId = packet.GameId;
            recon.Host = packet.Host == "" ? client.State.ConTargetAddress : packet.Host;
            recon.Port = packet.Port;
            recon.Key = packet.Key;
            recon.KeyTime = packet.KeyTime;
            recon.Name = packet.Name;

            if (packet.Host != "")
                client.State.ConTargetAddress = packet.Host;

            if (packet.Key.Length != 0)
                client.State.ConRealKey = packet.Key;

            // Tell the client to connect to the proxy
            packet.Key = Encoding.UTF8.GetBytes(client.State.Guid);
            packet.Host = "127.0.0.1";
            packet.Port = 2050;
        }

        private void OnGotoCommand(Client client, string command, string[] args)
        {
            if (args.Length != 1) return;
            var reconnect = (ReconnectPacket)Packet.Create(PacketType.RECONNECT);
            reconnect.Host = args[0];
            reconnect.Port = 2050;
            reconnect.GameId = -2;
            reconnect.Name = "Realm";
            reconnect.Key = new byte[0];
            reconnect.KeyTime = -1;
            SendReconnect(client, reconnect);
        }

        private void OnIpCommand(Client client, string command, string[] args)
        {
            var text = PluginUtils.CreateOryxNotification("Server",
                "Your current server's IP is: " + client.State.ConTargetAddress);
            client.SendToClient(text);
        }

        private void OnConnectCommand(Client client, string command, string[] args)
        {
            if (args.Length == 1)
            {
                if (GameData.GameData.Servers.Map.ContainsKey(args[0].ToUpper()))
                {
                    var reconnect = (ReconnectPacket)Packet.Create(PacketType.RECONNECT);
                    reconnect.Host = GameData.GameData.Servers.ById(args[0].ToUpper()).Address;
                    reconnect.Port = 2050;
                    reconnect.GameId = -2;
                    reconnect.Name = "Nexus";
                    reconnect.Key = new byte[0];
                    reconnect.KeyTime = -1;
                    SendReconnect(client, reconnect);
                }
                else
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("K Relay", "Unknown server!"));
                }
            }
        }

        public static void SendReconnect(Client client, ReconnectPacket reconnect)
        {
            var host = reconnect.Host;
            var port = reconnect.Port;
            var key = reconnect.Key;
            client.State.ConTargetAddress = host;
            client.State.ConTargetPort = port;
            client.State.ConRealKey = key;
            reconnect.Key = Encoding.UTF8.GetBytes(client.State.Guid);
            reconnect.Host = "127.0.0.1";
            reconnect.Port = 2050;

            client.SendToClient(reconnect);

            reconnect.Key = key;
            reconnect.Host = host;
            reconnect.Port = port;
        }
    }
}
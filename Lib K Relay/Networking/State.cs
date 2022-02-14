using System;
using System.Collections.Generic;

namespace Lib_K_Relay.Networking
{
    public class State
    {
        public string AccountId;
        public Client Client;

        public byte[] ConRealKey = new byte[0];
        public string ConTargetAddress = Proxy.DefaultServer;
        public ushort ConTargetPort = 2050;
        public string GUID;

        public Dictionary<string, dynamic> States;

        public State(Client client, string id)
        {
            Client = client;
            GUID = id;
            States = new Dictionary<string, dynamic>();
        }

        public dynamic this[string stateName]
        {
            get
            {
                dynamic value;
                States.TryGetValue(stateName, out value);
                return value;
            }
            set => States[stateName] = value;
        }

        public T Value<T>(string stateName)
        {
            var type = typeof(T);
            object value;

            if (!States.TryGetValue(stateName, out value))
            {
                value = Activator.CreateInstance(type);
                States.Add(stateName, value);
            }

            return (T)value;
        }
    }
}
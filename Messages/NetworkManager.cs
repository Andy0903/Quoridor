﻿using Lidgren.Network;
using System;

namespace QuoridorNetwork
{
    public static class NetworkManager
    {
        public static event EventHandler OnConnect;
        public static event EventHandler OnDisconnect;
        public static event EventHandler<ActionRejectMessage> OnActionRejected;
        public static event EventHandler<NewTurnMessage> OnNewTurn;
        public static event EventHandler<PlayerMoveMessage> OnPlayerMoved;
        public static event EventHandler<PlaceWallMessage> OnNewWallPlaced;
        public static event EventHandler<PlayerWonMessage> OnPlayerWon;
        public static event EventHandler<GameReadyToStartMessage> OnGameReadyToStart;
        public static event EventHandler<PlayerConnectMessage> OnPlayerConnected;

        const string applicationIdentifier = "QuoridorConfig";

        public static NetPeer Peer { get; private set; }

        public static void Send(NetOutgoingMessage outMsg, NetConnection recipient = null)
        {
            if (recipient != null)
            {
                Peer.SendMessage(outMsg, recipient, NetDeliveryMethod.ReliableOrdered);
            }
            else if (Peer is NetClient)
            {
                ((NetClient)Peer).SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
            }
            else if (Peer is NetServer)
            {
                NetServer server = (NetServer)Peer;
                server.SendMessage(outMsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static void InitializeServer(int port)
        {
            Peer = new NetServer(new NetPeerConfiguration(applicationIdentifier) { Port = port });
            Peer.Start();
        }

        public static void InitializeClient(int port, string ip)
        {
            Peer = new NetClient(new NetPeerConfiguration(applicationIdentifier));
            Peer.Start();
            Peer.Connect(ip, port);
        }

        public static void Update()
        {
            if (Peer == null)
                return;
            NetIncomingMessage incMsg;

            while ((incMsg = Peer.ReadMessage()) != null)
            {
                switch (incMsg.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        HandleStatusChangedMessage(incMsg);
                        break;
                    case NetIncomingMessageType.Data:
                        HandleDataMessage(incMsg);
                        break;
                }
            }

            if (incMsg != null)
            {
                Peer.Recycle(incMsg);
            }
        }

        private static void HandleDataMessage(NetIncomingMessage incMsg)
        {
            MessageType messageType = (MessageType)incMsg.ReadInt32();
            switch (messageType)
            {
                case MessageType.GameReadyToStart:
                    OnGameReadyToStart?.Invoke(null, new GameReadyToStartMessage(incMsg));
                    break;
                case MessageType.NewTurn:
                    OnNewTurn?.Invoke(null, new NewTurnMessage(incMsg));
                    break;
                case MessageType.PlaceWall:
                    OnNewWallPlaced?.Invoke(null, new PlaceWallMessage(incMsg));
                    break;
                case MessageType.PlayerConnect:
                    OnPlayerConnected?.Invoke(null, new PlayerConnectMessage(incMsg));
                    break;
                case MessageType.PlayerMove:
                    OnPlayerMoved?.Invoke(null, new PlayerMoveMessage(incMsg));
                    break;
                case MessageType.PlayerWon:
                    OnPlayerWon?.Invoke(null, new PlayerWonMessage(incMsg));
                    break;
                case MessageType.ActionReject:
                    OnActionRejected?.Invoke(null, new ActionRejectMessage(incMsg));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void HandleStatusChangedMessage(NetIncomingMessage incMsg)
        {
            NetConnectionStatus statusMessage = (NetConnectionStatus)incMsg.ReadByte();
            switch (statusMessage)
            {
                case NetConnectionStatus.Connected:
                    OnConnect?.Invoke(null, EventArgs.Empty);
                    break;
                case NetConnectionStatus.Disconnected:
                    OnDisconnect?.Invoke(null, EventArgs.Empty);
                    break;
            }
        }
    }
}

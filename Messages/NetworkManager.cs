using Lidgren.Network;
using System;

namespace Quoridor
{
    public static class NetworkManager
    {
        public static event EventHandler OnConnect;
        public static event EventHandler OnDisconnect;
        public static event EventHandler OnActionRejected;
        public static event EventHandler<NewTurnMessage> OnNewTurn;
        public static event EventHandler<PlayerMoveMessage> OnPlayerMoved;
        public static event EventHandler<PlaceWallMessage> OnNewWallPlaced;
        public static event EventHandler<PlayerWonMessage> OnPlayerWon;
        public static event EventHandler<GameReadyToStartMessage> OnGameReadyToStart;

        public static NetClient Client { get; private set; }

        static NetworkManager()
        {
            Client = new NetClient(new NetPeerConfiguration("QuoridorConfig"));
        }

        public static void Update()
        {
            NetIncomingMessage incMsg;

            while ((incMsg = Client.ReadMessage()) != null)
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
                Client.Recycle(incMsg);
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
                    //Not needed by client.
                    break;
                case MessageType.PlayerMove:
                    OnPlayerMoved?.Invoke(null, new PlayerMoveMessage(incMsg));
                    break;
                case MessageType.PlayerWon:
                    OnPlayerWon?.Invoke(null, new PlayerWonMessage(incMsg));
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

using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace Quoridor
{
    class MainMenu
    {
        string myNameText;

        public MainMenu()
        {
            Initialize();
        }

        public void Initialize()
        {
            Panel titlePanel = new Panel(new Vector2(250, 75), PanelSkin.Default, Anchor.TopCenter, new Vector2(0, 5));
            UserInterface.Active.AddEntity(titlePanel);

            Header title = new Header("Quoridor");
            titlePanel.AddChild(title);
            title.SetOffset(new Vector2(0, -10));

            Panel mainPanel = new Panel(new Vector2(500, 500));
            UserInterface.Active.AddEntity(mainPanel);

            HorizontalLine hline0 = new HorizontalLine();
            mainPanel.AddChild(hline0);
            TextInput nameText = new TextInput(false);
            nameText.PlaceholderText = "Insert name here..";
            mainPanel.AddChild(nameText);
            HorizontalLine hline1 = new HorizontalLine();
            mainPanel.AddChild(hline1);

            TextInput ipText = new TextInput(false);
            ipText.PlaceholderText = "Insert IP here..";
            mainPanel.AddChild(ipText);
            HorizontalLine hline2 = new HorizontalLine();
            mainPanel.AddChild(hline2);

            TextInput portText = new TextInput(false);
            portText.PlaceholderText = "Insert port here..";
            mainPanel.AddChild(portText);
            HorizontalLine hline3 = new HorizontalLine();
            mainPanel.AddChild(hline3);

            Button connectButton = new Button("Connect");
            mainPanel.AddChild(connectButton);
            connectButton.SetOffset(new Vector2(0, 80));

            NetworkManager.OnConnect += OnConnect;

            connectButton.OnClick = (Entity button) =>
            {
                NetworkManager.Client.Start();
                if (ipText.Value == "" && portText.Value == "" && nameText.Value == "") //TODO remove?
                {
                    nameText.Value = "Default";
                    ipText.Value = "127.0.0.1";
                    portText.Value = "14242";
                }

                myNameText = nameText.Value;
                int port;
                int.TryParse(portText.Value, out port);
                NetworkManager.Client.Connect(ipText.Value, port);

            };
        }

        private void OnConnect(object sender, System.EventArgs e)
        {
            PlayerConnectMessage msg = new PlayerConnectMessage(myNameText);
        }
    }
}

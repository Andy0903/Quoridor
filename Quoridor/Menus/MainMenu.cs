﻿using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using QuoridorNetwork;

namespace Quoridor
{
    class MainMenu
    {
        TextInput myNameText;

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
            myNameText = new TextInput(false);
            myNameText.PlaceholderText = "Name: Default";
            mainPanel.AddChild(myNameText);
            HorizontalLine hline1 = new HorizontalLine();
            mainPanel.AddChild(hline1);

            TextInput ipText = new TextInput(false);
            ipText.PlaceholderText = "IP: 127.0.0.1";
            mainPanel.AddChild(ipText);
            HorizontalLine hline2 = new HorizontalLine();
            mainPanel.AddChild(hline2);

            TextInput portText = new TextInput(false);
            portText.PlaceholderText = "Port: 14242";
            mainPanel.AddChild(portText);
            HorizontalLine hline3 = new HorizontalLine();
            mainPanel.AddChild(hline3);

            Button connectButton = new Button("Connect");
            mainPanel.AddChild(connectButton);
            connectButton.SetOffset(new Vector2(0, 80));

            NetworkManager.OnConnect += OnConnect;

            connectButton.OnClick = (Entity button) =>
            {
                connectButton.Disabled = true;
                if (myNameText.Value == "")
                {
                    myNameText.Value = "Default";
                }
                if (ipText.Value == "")
                {
                    ipText.Value = "127.0.0.1";
                }
                if (portText.Value == "")
                {
                    portText.Value = "14242";
                }
                NetworkManager.InitializeClient(int.Parse(portText.Value), ipText.Value);
            };
        }

        private void OnConnect(object sender, System.EventArgs e)
        {
            NetworkManager.Send(new PlayerConnectMessage(myNameText.Value));
        }
    }
}

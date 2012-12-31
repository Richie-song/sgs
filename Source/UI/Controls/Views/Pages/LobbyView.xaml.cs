﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sanguosha.Lobby.Core;
using System.Diagnostics;
using Sanguosha.Core.Network;
using System.ComponentModel;
using System.Threading;

namespace Sanguosha.UI.Controls
{
    /// <summary>
    /// Interaction logic for LobbyView.xaml
    /// </summary>
    public partial class LobbyView : Page
    {
        public LobbyView()
        {
            InitializeComponent();
            DataContext = LobbyViewModel.Instance;
            LobbyModel.OnGameInitiated += (o, e) => _StartGame();
        }

        public LobbyViewModel LobbyModel
        {
            get
            {
                return LobbyViewModel.Instance;
            }            
        }

        private void _StartGame()
        {
            Application.Current.Dispatcher.Invoke((ThreadStart)delegate()
            {
                Client client;
                int mainSeat = 0;

                client = new Client();
                //string addr = LobbyModel.GameServerConnectionString;
                string addr = "127.0.0.1:12345";
                string[] args = addr.Split(':');
                client.IpString = args[0];
                if (args.Length >= 2)
                {
                    client.PortNumber = int.Parse(args[1]);
                }
                else
                {
                    client.PortNumber = 12345;
                }

                busyIndicator.BusyContent = Resources["Busy.JoinGame"];
                busyIndicator.IsBusy = true;

                //client.Start(isReplay, FileStream = file.open(...))
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (o, ea) =>
                {
                    try
                    {
                        ea.Result = false;
                        client.Start();
                        mainSeat = (int)client.Receive();
                        client.SelfId = mainSeat;
                        ea.Result = true;
                    }
                    catch (Exception)
                    {
                    }
                };

                worker.RunWorkerCompleted += (o, ea) =>
                {
                    busyIndicator.IsBusy = false;
                    if ((bool)ea.Result)
                    {
                        MainGame game = new MainGame();
                        game.MainSeat = mainSeat;
                        game.NetworkClient = client;
                        this.NavigationService.Navigate(game);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Failed to create connection");
                    }
                };

                worker.RunWorkerAsync();
            });
        }

        private void muteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            muteButton.Visibility = Visibility.Collapsed;
            soundButton.Visibility = Visibility.Visible;
            GameSoundPlayer.IsMute = false;
        }

        private void soundButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            soundButton.Visibility = Visibility.Collapsed;
            muteButton.Visibility = Visibility.Visible;
            GameSoundPlayer.IsMute = true;
        }

        private void viewRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Trace.Assert(sender is Button);
            var model = (sender as Button).DataContext as RoomViewModel;
            if (model != null)
            {
                LobbyModel.CurrentRoom = model;
            }
        }

        private void enterRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Trace.Assert(sender is Button);
            var model = (sender as Button).DataContext as RoomViewModel;
            if (model != null)
            {
                LobbyModel.CurrentRoom = model;
                LobbyModel.EnterRoom();
            }
        }
    }
}

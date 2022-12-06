using Microsoft.Win32;
using Pushing2.Eventargs;
using Pushing2.Model;
using Pushing2.Persistance;
using Pushing2.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pushing2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _mainwindow;
        private PushingModel _model;
        private PushingViewModel _viewmodel;
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _mainwindow = new MainWindow();
            _model = new PushingModel(new DataAccess());
            _viewmodel = new PushingViewModel(_model);
            _mainwindow.DataContext = _viewmodel;
            _mainwindow.Show();
            _model.Finish += OnGameFinished;
            _viewmodel.LoadGame += new EventHandler(LoadGame);
            _viewmodel.SaveGame += new EventHandler(SaveGame);
        }

        private void OnGameFinished(object? sender, GameFinishedEvenetArgs e)
        {
            int w = e.W;
            int b = e.B;
            if (w > b)
            {
                MessageBox.Show
                (
                    "White wins the game",
                    "Game over"
                ) ;
            }
            else if (w < b)
            {
                MessageBox.Show
                (
                    "Black wins the game",
                    "Game over"
                );
            }
            else
            {
                MessageBox.Show
                (
                    "The game ended in a tie",
                    "Game over"
                );
            }
        }

        private void SaveGame(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                _model.SaveGame(path);
            }
        }

        private void LoadGame(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                _model.LoadGame(path);

            }
        }
        

    }
}


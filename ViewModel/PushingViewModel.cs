using Pushing2.Eventargs;
using Pushing2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Pushing2.ViewModel
{
    public class PushingViewModel : ViewModelBase
    {
        private PushingModel _model;
        private int _size;
        private int vrow;
        private int vcol;
        public int Size { get => _size; set{_size = value; OnPropertyChanged(); } }
        public ObservableCollection<FieldViewModel> Fields { get; set; }
        public DelegateCommand UpCommand { get; set; }
        public DelegateCommand DownCommand { get; set; }
        public DelegateCommand RightCommand { get; set; }
        public DelegateCommand LeftCommand { get; set; }
        public DelegateCommand StartNewGameCommand { get; set; }
        public DelegateCommand SaveGameCommand { get; set; }
        public DelegateCommand LoadGameCommand { get; set; }
        public FieldViewModel selectedButton { get; set; }
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        public PushingViewModel(PushingModel model)
        {
            _model = model;
            _model.GameStarted += OnGameStarted;
            Fields = new ObservableCollection<FieldViewModel>();
            _model.StartNewGame(3);
            _model.Refresh += _model_Refresh;
            StartNewGameCommand = new DelegateCommand(param => _model.StartNewGame(Convert.ToInt32(param)));
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            UpCommand = new DelegateCommand(param => { _model.MoveUp(vrow,vcol); });
            DownCommand = new DelegateCommand(param => { _model.MoveDown(vrow,vcol); });
            RightCommand = new DelegateCommand(param => { _model.MoveRight(vrow, vcol); });
            LeftCommand = new DelegateCommand(param => { _model.MoveLeft(vrow, vcol); });
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        private void _model_Refresh(object sender, GameStartedEventArgs e)
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                var f = Fields[i];
                f.Color = SetColor(e.Board[f.RowIndex, f.ColIndex]);
            }
        }

        private void OnGameStarted(object? sender, GameStartedEventArgs e)
        {
            Size = e.BoardSize;
            Fields.Clear();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var f = new FieldViewModel(SetColor(e.Board[i, j]), i, j, (i * Size + j));
                    f.FieldClicked += OnFieldClicked;
                    Fields.Add(f);

                }
            }
        }

        private void OnFieldClicked(object sender, EventArgs e)
        {
            var f = sender as FieldViewModel;
            if (f is not null)
            {
                vrow = f.RowIndex;
                vcol = f.ColIndex;
            }
        }

        private string SetColor(FieldState state)
        {
            switch(state)
            {
                case FieldState.White:return "white";
                case FieldState.Black: return "black";
                case FieldState.Gray: return "gray";
                default:return "white";
            }
        }

        private FieldViewModel GetSelectedField(int index)
        {
            return Fields[index];
        }
    }
}

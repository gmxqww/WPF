using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pushing2.ViewModel
{
    public class FieldViewModel : ViewModelBase
    {
        private string _color;
        public string Color { get => _color; set { _color = value; OnPropertyChanged(); } }
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
        public int number { get; set; }
        public DelegateCommand SelectCommand { get; set; }
        public event EventHandler<EventArgs> FieldClicked;
        public FieldViewModel(string color,  int rowIndex, int colIndex,int N)
        {
            Color = color;
            RowIndex = rowIndex;
            ColIndex = colIndex;
            number = N;
            SelectCommand = new DelegateCommand(_ => FieldClicked?.Invoke(this, EventArgs.Empty));
           
        }
    }
}

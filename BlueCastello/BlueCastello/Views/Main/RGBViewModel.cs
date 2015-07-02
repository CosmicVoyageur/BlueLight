using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace BlueCastello.Views.Main
{
    public class RGBViewModel : XViewModel
    {
        private Color _resultingColor;
        private int _blueValue;
        private int _greenValue;
        private int _redValue;

        public RGBViewModel()
        {
            Title = "RGB";
        }

        public int RedValue
        {
            get { return _redValue; }
            set
            {
                _redValue = value;
                OnPropertyChanged();
                _refreshBox();
            }
        }

        public int GreenValue
        {
            get { return _greenValue; }
            set
            {
                _greenValue = value;
                OnPropertyChanged();
                _refreshBox();
            }
        }

        public int BlueValue
        {
            get { return _blueValue; }
            set
            {
                _blueValue = value; 
                OnPropertyChanged();
                _refreshBox();
            }
        }

        public Color ResultingColor
        {
            get { return _resultingColor; }
            set
            {
                _resultingColor = value;
                OnPropertyChanged();
            }
        }


        public override void OnInitialise()
        {
            base.OnInitialise();
            _refreshBox();
            RedValue = 50;
            GreenValue = 50;
            BlueValue = 50;
        }

        private void _refreshBox()
        {
            ResultingColor = Color.FromRgb(RedValue, GreenValue, BlueValue);
        }
    }
}

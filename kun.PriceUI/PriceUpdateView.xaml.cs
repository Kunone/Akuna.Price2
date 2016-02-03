using Akuna.PriceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kun.PriceUI
{
    /// <summary>
    /// Interaction logic for PriceUpdateView.xaml
    /// </summary>
    public partial class PriceUpdateView : Window
    {
        private static PriceUpdateViewModel _viewModel;

        public PriceUpdateView()
        {
            InitializeComponent();
            _viewModel = new PriceUpdateViewModel(new RandomWalkPriceService());
            DataContext = _viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _viewModel.Dispose();
        }
    }
}

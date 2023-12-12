using System.ComponentModel;
using TiendaMovil.ViewModels;
using Xamarin.Forms;

namespace TiendaMovil.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
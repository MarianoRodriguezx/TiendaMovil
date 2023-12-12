using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaMovil.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TiendaMovil.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCart : ContentPage
    {
        private ObservableCollection<shoppingCart> _cart;
        public ShoppingCart()
        {
            InitializeComponent();
            var shoppingCart = Preferences.Get("shopping_cart", string.Empty);

            if(!string.IsNullOrEmpty(shoppingCart))
            {
                sbt.IsEnabled = true;
                var shoppings = JsonConvert.DeserializeObject<List<shoppingCart>>(shoppingCart);

                _cart = new ObservableCollection<shoppingCart>(shoppings);
                cartListView.ItemsSource = _cart;

                float totalPagar = 0;

                foreach (var element in shoppings)
                {
                    totalPagar += element.total;
                }

                total.Text = $"Total a Pagar: {totalPagar}";
            }
            else
            {
                sbt.IsEnabled = false;
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            shoppingCart productToRemove = (shoppingCart)button.CommandParameter;

            if (_cart.Contains(productToRemove))
            {
                _cart.Remove(productToRemove);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderPage());
        }
    }
}
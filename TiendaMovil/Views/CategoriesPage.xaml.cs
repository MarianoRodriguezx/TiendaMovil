using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TiendaMovil.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TiendaMovil.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriesPage : ContentPage
    {
        private string url = "http://192.168.116.140:8000/api/categorias";
        HttpClient client = new HttpClient();
        private ObservableCollection<Categories> _categories;
        public CategoriesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            string contenido = await client.GetStringAsync(url);
            List<Categories> categories = JsonConvert.DeserializeObject<List<Categories>>(contenido);

            _categories = new ObservableCollection<Categories>(categories);
            CargarTarjetas(_categories);

            base.OnAppearing();
        }


        private void CargarTarjetas(ObservableCollection<Categories> categories)
        {
            // Crear las tarjetas basadas en la información de los productos
            foreach (var category in categories)
            {
                var card = CreateCard(category);
                stackLayout.Children.Add(card);
            }

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        private CardFrame CreateCard(Categories category)
        {
            // Crear un Frame que actuará como la tarjeta
            var cardFrame = new CardFrame
            {
                BorderColor = Color.Gray,
                CornerRadius = 10,
                HasShadow = true,
                Padding = new Thickness(7),
                Margin = new Thickness(17)
            };

            // Agregar contenido a la tarjeta
            var titleLabel = new Label
            {
                Text = category.name,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.DarkBlue,
                Margin = new Thickness(10, 10, 10, 10),
                HorizontalTextAlignment = TextAlignment.Center
            };

            // Agregar elementos al contenido de la tarjeta
            cardFrame.Content = new StackLayout
            {
                Children =
                {
                    titleLabel
                }
            };

            // Agregar un controlador de eventos de tap a la tarjeta
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new ProductsPage(category.products));
                //Aqui va la direccion a la lista de productos
            };

            cardFrame.GestureRecognizers.Add(tapGestureRecognizer);

            return cardFrame;
        }

        public class CardFrame : Frame
        {
            // Puedes agregar propiedades específicas de la tarjeta si es necesario
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShoppingCart());
        }
    }
}
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
    public partial class BrandsPage : ContentPage
    {
        private string url = "http://192.168.116.140:8000/api/marcas";
        HttpClient client = new HttpClient();
        private ObservableCollection<Brands> _brands;

        public BrandsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            string contenido = await client.GetStringAsync(url);
            List<Brands> categories = JsonConvert.DeserializeObject<List<Brands>>(contenido);

            _brands = new ObservableCollection<Brands>(categories);
            CargarTarjetas(_brands);

            base.OnAppearing();
        }


        private void CargarTarjetas(ObservableCollection<Brands> brands)
        {
            // Crear las tarjetas basadas en la información de los productos
            foreach (var brand in brands)
            {
                var card = CreateCard(brand);
                stackLayout.Children.Add(card);
            }

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        private CardFrame CreateCard(Brands brand)
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
                Text = brand.name,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.DarkBlue,
                Margin = new Thickness(10, 10, 10, 10),
                HorizontalTextAlignment = TextAlignment.Center
            };

            var image = new Image
            {
                Source = brand.logo_path,
                Aspect = Aspect.AspectFill,
                HeightRequest = 250 // Ajusta según tus necesidades
            };

            // Agregar elementos al contenido de la tarjeta
            cardFrame.Content = new StackLayout
            {
                Children =
                {
                    image,
                    titleLabel
                }
            };

            // Agregar un controlador de eventos de tap a la tarjeta
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new ProductsPage(brand.products));
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TiendaMovil.Models;
using TiendaMovil.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TiendaMovil.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        //private ProductsViewModel _viewModel;

        private string url = "http://192.168.116.140:8000/api/productos";
        HttpClient client = new HttpClient();
        private ObservableCollection<Product> _products;

        public HomePage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            string contenido = await client.GetStringAsync(url);
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(contenido);

            _products = new ObservableCollection<Product>(products);
            CargarTarjetas(_products);

            base.OnAppearing();
        }


        private void CargarTarjetas(ObservableCollection<Product> products)
        {
            // Crear las tarjetas basadas en la información de los productos
            foreach (var producto in products)
            {
                var card = CreateCard(producto);
                stackLayout.Children.Add(card);
            }

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        private CardFrame CreateCard(Product product)
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
                Text = product.name,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.DarkBlue,
                Margin = new Thickness(10, 10, 10, 10),
                HorizontalTextAlignment = TextAlignment.Center
            };

            var contentLabel = new Label
            {
                Text = product.description,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var contentLabelPrice = new Label
            {
                Text = "Precio: $" + product.price.ToString(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var image = new Image
            {
                Source = product.image_path,
                Aspect = Aspect.AspectFit,
                HeightRequest = 250 // Ajusta según tus necesidades
            };

            var buttonShop = new Button
            {
                Text = "Agregar al Carrito",
                BackgroundColor = Color.Green,
                TextColor = Color.White,
                CornerRadius = 10
            };

            buttonShop.Clicked += ButtonShop_Clicked;
            buttonShop.CommandParameter = product;

            var buttonDetail = new Button
            {
                Text = "Ver Detalles",
                BackgroundColor = Color.Yellow,
                TextColor = Color.White,
                CornerRadius = 10
            };

            // Agregar elementos al contenido de la tarjeta
            cardFrame.Content = new StackLayout
            {
                Children =
                {
                    image,
                    titleLabel,
                    contentLabel,
                    contentLabelPrice,
                    buttonShop
                
                }
            };

            // Agregar un controlador de eventos de tap a la tarjeta
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new DetailProductPage(product));
                // Realizar la acción que deseas cuando se selecciona la tarjeta
                //DisplayAlert("Tarjeta seleccionada", $"Se seleccionó la tarjeta {product.name}", "OK");
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

        private void ButtonShop_Clicked(object sender, EventArgs e)
        {
            Button botonPresionado = sender as Button;
            var producto = botonPresionado.CommandParameter as Product;

            if (producto != null)
            {
                var carritoJson = Preferences.Get("shopping_cart", string.Empty);

                List<shoppingCart> listaCarrito;

                if (!string.IsNullOrEmpty(carritoJson))
                {
                    try
                    {
                        listaCarrito = JsonConvert.DeserializeObject<List<shoppingCart>>(carritoJson);
                    }
                    catch (JsonException)
                    {
                        // Manejar errores de deserialización JSON
                        return;
                    }
                }
                else
                {
                    listaCarrito = new List<shoppingCart>();
                }

                ActualizarCarrito(producto, listaCarrito);

                // Lógica adicional que se ejecutará después de actualizar el carrito
                MostrarSnackbar();
            }
        }

        private void ActualizarCarrito(Product producto, List<shoppingCart> carrito)
        {
            var itemExistente = carrito.FirstOrDefault(item => item.id == producto.id);

            if (itemExistente != null)
            {
                // El producto ya está en el carrito, actualiza la cantidad
                var total = itemExistente.quantity += 1;
                itemExistente.total = total * itemExistente.product.price;
            }
            else
            {
                // Agrega un nuevo registro al carrito
                var nuevoRegistro = new shoppingCart { id = producto.id, quantity = 1, product = producto, total = producto.price };
                carrito.Add(nuevoRegistro);
            }

            // Guarda la lista actualizada en las preferencias
            var carritoActualizado = JsonConvert.SerializeObject(carrito);
            Preferences.Set("shopping_cart", carritoActualizado);
        }

        private void MostrarSnackbar()
        {
            DisplayAlert("Articulo Agregado", "Se agrego correctamente el articulo a tu carrito", "Aceptar");
        }
    }
}
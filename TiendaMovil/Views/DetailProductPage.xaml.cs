using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class DetailProductPage : ContentPage
    {
        public DetailProductPage(Product product)
        {
            BindingContext = product;
           // lblPriceStock.Text = $"Precio: {product.price} | Stock: {product.stock}";
            InitializeComponent();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShoppingCart());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button botonPresionado = sender as Button;
            var producto = botonPresionado.BindingContext as Product;

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
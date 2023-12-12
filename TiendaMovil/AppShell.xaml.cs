using System;
using System.Collections.Generic;
using TiendaMovil.ViewModels;
using TiendaMovil.Views;
using Xamarin.Forms;

namespace TiendaMovil
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}

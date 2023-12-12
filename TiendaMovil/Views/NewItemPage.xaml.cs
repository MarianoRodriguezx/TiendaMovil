﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using TiendaMovil.Models;
using TiendaMovil.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TiendaMovil.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
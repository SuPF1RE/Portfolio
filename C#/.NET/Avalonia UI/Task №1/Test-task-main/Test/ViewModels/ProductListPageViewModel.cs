using ReactiveUI;
using System.Collections.ObjectModel;
using Test.Models;

namespace Test.ViewModels
{
    public partial class ProductListPageViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _main;
        public static ObservableCollection<Product> Products { get; } = new();

        private Product? _selectedProduct;
        public Product? SelectedProduct
        {   
            get => _selectedProduct;
            set => this.RaiseAndSetIfChanged(ref _selectedProduct, value);
        }

        public ProductListPageViewModel(MainWindowViewModel main)
        {
            _main = main;
        }

        public void AddProduct()
        {
            var owner = Avalonia.Application.Current?.ApplicationLifetime switch
            {
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow,
                _ => null
            };

            if (owner != null)
                _main.Navigate(new ProductDetailViewModel(owner, _main, new Product(), Products));
        }

        public void EditProduct()
        {
            if (SelectedProduct == null) return;

            var owner = Avalonia.Application.Current?.ApplicationLifetime switch
            {
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow,
                _ => null
            };

            if (owner != null)
                _main.Navigate(new ProductDetailViewModel(owner, _main, SelectedProduct, Products));
        }

        public void DeleteProduct()
        {
            if (SelectedProduct != null)
                Products.Remove(SelectedProduct);
        }
    }
}

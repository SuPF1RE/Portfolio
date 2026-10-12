using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Test.Models;

namespace Test.ViewModels
{
    public partial class ProductDetailViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly MainWindowViewModel _main;
        private readonly ObservableCollection<Product> _products;
        private readonly Product _originalProduct;

        private readonly Dictionary<string, List<string>> _errors = new();

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public Product EditableProduct { get; }
        public ProductDetailViewModel(Window owner, MainWindowViewModel main, Product product, ObservableCollection<Product> products)
        {
            var ProductVariants = new List<Variant>
            {
                new Variant("Color", "Blue", false),
                new Variant("Color", "Red", false),
                new Variant("Color", "Green", false),
                new Variant("Size", "S", false),
                new Variant("Size", "M", false)
            };
            _main = main;
            _originalProduct = product;
            _products = products;
            _owner = owner;

            EditableProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Variants = new ObservableCollection<Models.Variant>(
            product.Variants != null && product.Variants.Count > 0
                ? product.Variants
                : ProductVariants
        )
            };
            Variants = EditableProduct.Variants;
            var canSave = this.WhenAnyValue(
                vm => vm.HasErrors,
                hasErrors => !hasErrors
            );
            SaveCommand = ReactiveCommand.Create(Save, canSave);
        }
        public string Name
        {
            get => EditableProduct.Name;
            set
            {
                if (EditableProduct.Name != value)
                {
                    EditableProduct.Name = value;
                    this.RaisePropertyChanged();
                    ValidateName();
                }
            }
        }
        public decimal Price
        {
            get => EditableProduct.Price;
            set
            {
                if (EditableProduct.Price != value)
                {
                    EditableProduct.Price = value;
                    this.RaisePropertyChanged();
                    ValidatePrice();
                }
            }
        }
        public string Description
        {
            get => EditableProduct.Description;
            set
            {
                if (EditableProduct.Description != value)
                {
                    EditableProduct.Description = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<Variant> Variants { get; }
        private readonly Window _owner;
             
        private Variant? _selectedVariant;
        public Variant? SelectedVariant
        {
            get => _selectedVariant;
            set => this.RaiseAndSetIfChanged(ref _selectedVariant, value);
        }
        public void AddVariant()
        {
            var newVariant = new Variant("", "", false);
            Variants.Add(newVariant);
            SelectedVariant = newVariant;
        }

        public async Task DeleteVariant()
        {
            var dialog = new DialogWindow();
            var result = await dialog.ShowDialog<bool>(_owner);

            if (result && SelectedVariant != null)
            {
                Variants.Remove(SelectedVariant);
            }
            
        }
        
        public void Save()
        {
            if (HasErrors)
                return;
            _originalProduct.Name = EditableProduct.Name;
            _originalProduct.Price = EditableProduct.Price;
            _originalProduct.Description = EditableProduct.Description;

            _originalProduct.Variants.Clear();
            foreach (var v in Variants)
                _originalProduct.Variants.Add(v);

            if (!_products.Contains(_originalProduct))
                _products.Add(_originalProduct);

            _main.Navigate(new ProductListPageViewModel(_main));
        }

        public void Cancel() => _main.Navigate(new ProductListPageViewModel(_main));

        #region Validation

        private void ValidateName()
        {
            ClearErrors(nameof(Name));
            if (string.IsNullOrWhiteSpace(Name))
                AddError(nameof(Name), "Name is required");
            else if (Name.Length < 3)
                AddError(nameof(Name), "Name has to be longer than 3");
        }

        private void ValidatePrice()
        {
            ClearErrors(nameof(Price));
            if (Price <= 0)
                AddError(nameof(Price), "Price must to be more than 0");
        }

        public bool HasErrors => _errors.Any();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _errors.TryGetValue(propertyName, out var list))
                return list;
            return Enumerable.Empty<string>();
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();
            _errors[propertyName].Add(error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            this.RaisePropertyChanged(nameof(HasErrors));
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName)) 
            { 
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

                this.RaisePropertyChanged(nameof(HasErrors));
            }
        }

        #endregion
    }
}

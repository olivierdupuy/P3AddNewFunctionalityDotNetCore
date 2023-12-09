using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "MissingName", ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService))]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessageResourceType = typeof(Services.ProductService), ErrorMessageResourceName = "MissingQuantity")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Services.ProductService), ErrorMessageResourceName = "StockNotAnInteger")]
        public string Stock { get; set; }

        [Required(ErrorMessageResourceType = typeof(Services.ProductService), ErrorMessageResourceName = "MissingPrice")]
        [DataType(DataType.Currency, ErrorMessageResourceType = typeof(Services.ProductService), ErrorMessageResourceName = "PriceNotANumber")]
        [Range(0, double.MaxValue, ErrorMessageResourceType = typeof(Services.ProductService), ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}

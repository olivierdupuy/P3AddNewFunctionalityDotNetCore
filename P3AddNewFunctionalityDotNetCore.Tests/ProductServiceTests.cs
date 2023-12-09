using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {

        //static List<string> validityResult(ProductViewModel product)
        //{
        //    var mockCart = Mock.Of<ICart>();
        //    var mockProductRepository = Mock.Of<IProductRepository>();
        //    var mockOrderRepository = Mock.Of<IOrderRepository>();
        //    var mockLocalizer = Mock.Of<IStringLocalizer<ProductService>>();
        //    var productService = new ProductService(
        //        mockCart,
        //        mockProductRepository,
        //        mockOrderRepository,
        //        mockLocalizer
        //    );
        //    var modelState = productService.CheckProductModelErrors(product);
        //    return modelState;
        //}

        public static ModelStateDictionary validityResult(ProductViewModel product)
        {
            var modelState = new ModelStateDictionary();

            // Création du contexte de validation
            var validationContext = new ValidationContext(product, null, null);

            // Liste pour collecter les résultats de la validation
            var validationResults = new List<ValidationResult>();

            // Vérification des annotations de DataAnnotation sur le modèle ProductViewModel
            bool isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

            // Si le modèle n'est pas valide, ajoutez les erreurs de validation au ModelStateDictionary
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    // Pour chaque erreur, ajoutez-la au ModelState avec un clé appropriée
                    // Ici, MemberNames est une collection de noms de propriétés liées à cette erreur de validation
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        modelState.AddModelError(memberName, validationResult.ErrorMessage);
                    }
                }
            }

            return modelState;
        }

        [Fact]
        public void CheckProductModelErrors_ValidProduct()
        {// Arrange
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "100",
                Stock = "5",
                Description = "Valid description",
                Details = "Valid details"
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Empty(result);
        }
        [Fact]
        public void CheckProductModelErrors_ValidProduct_BlankSpaces()
        {// Arrange
            var product = new ProductViewModel
            {
                Name = "  Name  ",
                Price = "  100  ",
                Stock = "  5  ",
                Description = "Valid description",
                Details = "Valid details"
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Empty(result);
        }
        [Fact]
        public void CheckProductModelErrors_CommaSeparator()
        {// Arrange
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "100,5",
                Stock = "5,5",
                Description = "Valid description",
                Details = "Valid details"
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Single(result);
            Assert.Contains("The value entered for the stock must be a integer", result);
        }
        [Fact]
        public void CheckProductModelErrors_DotSeparator()
        {// Arrange
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "100.5",
                Stock = "5.5",
                Description = "Valid description",
                Details = "Valid details"
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Single(result);
            Assert.Contains("The stock must be a positive integer", result);
        }
        [Fact]
        public void CheckProductModelErrors_MissingData()
        {
            var product = new ProductViewModel
            {
                Name = "",
                Price = "",
                Stock = "",
                Description = "Valid description",
                Details = "Valid details"
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("Please enter a name", result);
            Assert.Contains("Please enter a price", result);
            Assert.Contains("Please enter a stock value", result);

        }

        [Fact]
        public void CheckProductModelErrors_NegativePriceAndStock()
        {
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "-100",
                Stock = "-5",
                Description = "Valid description",
                Details = "Valid details"
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("The stock must be a positive integer", result);
            Assert.Contains("The price must be a positive number", result);
        }
        [Fact]
        public void CheckProductModelErrors_InvalidPriceAndStock()
        {
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "e5",
                Stock = "abc",
                Description = "Valid description",
                Details = "Valid details"
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("The stock must be a positive integer", result);
            Assert.Contains("The price must be a positive number", result);
        }

        [Fact]
        public void CheckProductModelErrors_MissingDataInFrench()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "",
                Price = "",
                Stock = "",
                Description = "",
                Details = ""
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("Veuillez saisir un nom", result);
            Assert.Contains("Veuillez saisir un prix", result);
            Assert.Contains("Veuillez saisir une quantité", result);
        }
        [Fact]
        public void CheckProductModelErrors_InvalidDataInFrench()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var product = new ProductViewModel
            {
                Name = "Name",
                Price = "price",
                Stock = "-10.5",
                Description = "",
                Details = ""
            };
            //Act
            var result = validityResult(product);
            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("Le stock doit être un entier positif", result);
            Assert.Contains("Le prix doit être un nombre positif", result);

        }

    }
}
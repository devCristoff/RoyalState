﻿using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Property
{
    public class SavePropertyViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a Property Type.")]
        [Required(ErrorMessage = "Please select a Property Type.")]
        public int PropertyTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a Sale Type.")]
        [Required(ErrorMessage = "Please select a Sale Type.")]
        public int SaleTypeId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The meters field must be a positive value.")]
        [Required(ErrorMessage = "Please provide a price for the property.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please provide a value for the meters field.")]
        [Range(0, double.MaxValue, ErrorMessage = "The meters field must be a positive value.")]
        public double Meters { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid number of bedrooms.")]
        [Required(ErrorMessage = "Please provide a number of bedrooms.")]
        public int Bedrooms { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid number of bathrooms.")]
        [Required(ErrorMessage = "Please provide a number of bathrooms.")]
        public int Bathrooms { get; set; }

        [DataType(DataType.Text)]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "The description must have at least 10 characters.")]
        [Required(ErrorMessage = "You can't leave the description empty.")]

        public string Description { get; set; }


        public int? AgentId { get; set; }
        public List<string>? PropertyImages { get; set; }
        public List<int>? Improvements { get; set; }
    }

}

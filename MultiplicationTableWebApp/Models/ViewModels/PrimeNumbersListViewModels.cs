using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MultiplicationTableWebApp.Models.ViewModels
{
    public class PrimeNumbersListViewModels
    {
        [Required(ErrorMessage = "Please select a number of prime numbers.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int N { get; set; }
        public int[] PrimeNumbers { get; set; }
        public Dictionary<int, List<int>> MultiplicationTable { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiplicationTableCalculator;
using MultiplicationTableWebApp.Models;
using MultiplicationTableWebApp.Models.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;

namespace MultiplicationTableWebApp.Controllers
{
    public class HomeController : Controller
    {
        private int _pageSize = 25;

        public HomeController()
        {
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            ViewBag.ShowTable = false;

            if (!page.HasValue)
                return View();

            var N = HttpContext?.Session?.GetInt32("N");

            if (N.HasValue)
            {
                PrimeNumbersListViewModels model = GetPrimeNumbers((int)N, page);
                return View(model);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PrimeNumbersListViewModels primeNumberListModel)
        {
            ViewBag.ShowTable = false;

            if (ModelState.IsValid)
            {
                var N = primeNumberListModel.N;
                HttpContext.Session.SetInt32("N", N);
                PrimeNumbersListViewModels model = GetPrimeNumbers(N, 1);

                return View(model);
            }
            return View(primeNumberListModel);
        }

        /// <summary>
        /// Method for getting prime numbers, multiplication table and set it to view
        /// </summary>
        /// <param name="N"></param>
        private PrimeNumbersListViewModels GetPrimeNumbers(int N, int? page)
        {
            int[] primeNumbers = PrimeNumberGenerator.getPrimeNumbers(N);
            var pageNumber = page ?? 1;
            var primesMultiplyTable = GetMultiplicationTable(primeNumbers, pageNumber, _pageSize);
            ViewBag.ShowTable = true;

            return new PrimeNumbersListViewModels()
            {
                N = N,
                PrimeNumbers = primeNumbers,
                MultiplicationTable = primesMultiplyTable,
                PagingInfo = new PagingInfo()
                {
                    TotalItems = N,
                    CurrentPage = pageNumber,
                    ItemsPerPage = _pageSize
                }
            };
        }

        /// <summary>
        /// Dynamically compute multiplication table on pagging change
        /// </summary>
        /// <param name="primeNumbers"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private Dictionary<int, List<int>> GetMultiplicationTable(int[] primeNumbers, int page, int pageSize)
        {
            Dictionary<int, List<int>> resultTable = new Dictionary<int, List<int>>();
            int length = primeNumbers.Length;
            int indexFrom = pageSize * page - pageSize;
            int indexTo = page * pageSize;

            if(indexTo > length)
            {
                indexTo = length;
            }

            for (int i = indexFrom; i < indexTo; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < length; j++)
                {
                    int multiply = primeNumbers[i] * primeNumbers[j];
                    row.Add(multiply);
                }
                resultTable.Add(primeNumbers[i], row);
            }

            return resultTable;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
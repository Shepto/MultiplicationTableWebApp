using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MultiplicationTableWebApp.Controllers;
using MultiplicationTableWebApp.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MultiplicationTableWebApp.Tests
{
    public class HomeControllerTests
    {
        private const int N = 5;
        private int[] _primeNumbers = new int[N] { 2, 3, 5, 7, 11 };
        private Dictionary<int, List<int>> _multiplicationTable = new Dictionary<int, List<int>>()
        {
            { 2, new List<int>() { 4, 6, 10, 14, 22 } },
            { 3, new List<int>() { 6, 9, 15, 21, 33 } },
            { 5, new List<int>() { 10, 15, 25, 35, 55 } },
            { 7, new List<int>() { 14, 21, 35, 49, 77 } },
            { 11, new List<int>() { 22, 33, 55, 77, 121 } }
        };

        #region Helper methods
        /// <summary>
        /// Insert Int32 value to mock session object
        /// </summary>
        /// <param name="sessionMock"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        private void SetSessionInt32ToMock(Mock<ISession> sessionMock, string key, int val)
        {
            var value = new byte[]
            {
                (byte)(val >> 24),
                (byte)(0xFF & (val >> 16)),
                (byte)(0xFF & (val >> 8)),
                (byte)(0xFF & val)
            };
            sessionMock.Setup(_ => _.TryGetValue(key, out value)).Returns(true);
        }
        
        /// <summary>
        /// Method for checking return model values (prime numbers and multiplication table)
        /// </summary>
        /// <param name="model"></param>
        private void CheckPrimeNumbersResulModel(PrimeNumbersListViewModels model)
        {
            Assert.Equal(N, model.N);
            Assert.Equal(N, model.MultiplicationTable.Count);

            //Assert prime numbers
            Assert.Equal(_primeNumbers.Length, model.PrimeNumbers.Length);
            for (int i = 0; i < N; i++)
            {
                Assert.Equal(_primeNumbers[i], model.PrimeNumbers[i]);
            }
            //Assert multiplication table
            Assert.Collection(model.MultiplicationTable,
                item => Assert.Equal(2, item.Key),
                item => Assert.Equal(3, item.Key),
                item => Assert.Equal(5, item.Key),
                item => Assert.Equal(7, item.Key),
                item => Assert.Equal(11, item.Key)
            );

            for (int i = 0; i < model.MultiplicationTable.Count; i++)
            {
                var list = _multiplicationTable.ElementAt(i);
                var modelList = model.MultiplicationTable.ElementAt(i);
                Assert.Equal(list.Value.Count, list.Value.Count);

                for (int j = 0; j < list.Value.Count; j++)
                {
                    Assert.Equal(list.Value[j], modelList.Value[j]);
                }
            }
        }
        #endregion

        /// <summary>
        /// Test Index get method with null page parameter and null session
        /// </summary>
        /// <param name="page"></param>
        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public void IndexActionPageGetNullValues(int? page)
        {
            var controller = new HomeController();

            var result = controller.Index(page) as ViewResult;
            bool showTable = (bool)result?.ViewData?.Values?.FirstOrDefault();
            var model = result?.Model as PrimeNumbersListViewModels;

            Assert.NotNull(result);
            Assert.False(showTable);
            Assert.Null(model);
        }

        /// <summary>
        /// Test Index get method
        /// </summary>
        [Fact]
        public void IndexActionPageGet()
        {
            var controller = new HomeController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Mock<ISession> sessionMock = new Mock<ISession>();
            SetSessionInt32ToMock(sessionMock, "N", N);
            controller.ControllerContext.HttpContext.Session = sessionMock.Object;

            var result = controller.Index(1) as ViewResult;
            bool showTableResult = (bool)result?.ViewData?.Values?.FirstOrDefault();
            var model = result.Model as PrimeNumbersListViewModels;

            Assert.NotNull(result);
            Assert.True(showTableResult);
            CheckPrimeNumbersResulModel(model);
        }

        /// <summary>
        /// Test Index post method
        /// </summary>
        [Fact]
        public void IndexActionPagePost()
        {
            var controller = new HomeController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Mock<ISession> sessionMock = new Mock<ISession>();
            controller.ControllerContext.HttpContext.Session = sessionMock.Object;

            var primeNumbers = new PrimeNumbersListViewModels()
            {
                N = N
            };

            var result = controller.Index(primeNumbers) as ViewResult;
            bool showTableResult = (bool)result?.ViewData?.Values?.FirstOrDefault();
            var model = result.Model as PrimeNumbersListViewModels;

            Assert.NotNull(result);
            Assert.True(showTableResult);
            CheckPrimeNumbersResulModel(model);
        }
    }
}
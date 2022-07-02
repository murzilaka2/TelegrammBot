using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FactorTest
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Передаем значение в диапазоне от 1 до 1000
        /// </summary>
        [TestMethod]
        public void PrimeFactorsTest()
        {
            //размещение
            int n = 40;
            string expected = "1 * 2 * 2 * 2 * 5";
            Factors factors = new Factors();
            //действие
            string actual = factors.PrimeFactors(n);
            //утверждение
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// Передаем значение больше 1000
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Введенное число больше 1000.")]
        public void PrimeFactorsTestOversize()
        {
            //размещение
            int n = 1001;
            Factors factors = new Factors();
            //действие
            string actual = factors.PrimeFactors(n);
        }
        /// <summary>
        /// Передаем значение меньше нуля
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Введенное число меньше, либо равно 0.")]
        public void PrimeFactorsTestNegativeNumber()
        {
            //размещение
            int n = -9;
            Factors factors = new Factors();
            //действие
            string actual = factors.PrimeFactors(n);
        }
        [TestMethod]
        public void SumTestResult()
        {
            int a = 10;
            int b = 20;

            int expected = a + b;
            Factors factors = new Factors();
            int actual = factors.Sum(a, b);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(ArithmeticException), "При значениии 0,0 вернем ошибку ArithmeticException")]
        public void SumTestZeroNumbers()
        {
            int a = 0, b = 0;
            Factors factors = new Factors();
            int actual = factors.Sum(a, b);
        }
    }
}
using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
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
    }

}
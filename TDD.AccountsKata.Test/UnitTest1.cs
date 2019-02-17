using System;
using System.Collections.Generic;
using NUnit.Framework;
using TDD.AccountingKata.Core;

namespace TDD.AccountsKata.Test
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidateDate()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201901"},
            });

            AmountShouldBe(0, 
                GetTotalAmountReturnFromInstance(accountingInstance,
                new DateTime(2019, 01, 05),
                new DateTime(2019, 01, 04)));
        }

        [Test]
        public void QueryTotalWithNoBudgetRecord()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 0, YearMonth = "201904"}
            });

            AmountShouldBe(0,
                GetTotalAmountReturnFromInstance(accountingInstance, 
                    new DateTime(2019, 04, 01),
                    new DateTime(2019, 04, 02)));
        }

        [Test]
        public void QueryOneDateBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201901"}
            });

            AmountShouldBe(10,
                GetTotalAmountReturnFromInstance(accountingInstance, 
                    new DateTime(2019, 01, 01),
                    new DateTime(2019, 01, 01)));
        }

        [Test]
        public void QueryOneMonthBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201901"}
            });

            AmountShouldBe(310,
                GetTotalAmountReturnFromInstance(accountingInstance, 
                    new DateTime(2019, 01, 01),
                    new DateTime(2019, 01, 31)));
        }

        [Test]
        public void QueryTwoMonthBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 280, YearMonth = "201902"},
                new Budget {Amount = 310, YearMonth = "201903"},
            });

            AmountShouldBe(20,
                GetTotalAmountReturnFromInstance(accountingInstance, 
                    new DateTime(2019, 02, 28),
                    new DateTime(2019, 03, 01)));
        }

        [Test]
        public void QueryTwoMonthBudgetWithNoData()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201903"},
                new Budget {Amount = 0, YearMonth = "201904"}
            });

            AmountShouldBe(20,
                GetTotalAmountReturnFromInstance(accountingInstance, 
                    new DateTime(2019, 03, 30),
                    new DateTime(2019, 04, 04)));
        }

        [Test]
        public void QueryLunarYearBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 290, YearMonth = "202002"}
            });

            AmountShouldBe(280,
                GetTotalAmountReturnFromInstance(accountingInstance, 
                    new DateTime(2020, 02, 01),
                    new DateTime(2020, 02, 28)));
        }

        [Test]
        public void QueryThreeMonthBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201901"},
                new Budget {Amount = 280, YearMonth = "201902"},
                new Budget {Amount = 310, YearMonth = "201903"},
            });

            AmountShouldBe(400, 
                GetTotalAmountReturnFromInstance(accountingInstance,
                new DateTime(2019, 01, 25),
                new DateTime(2019, 03, 05)));
        }

        #region Private

        private void AmountShouldBe(double expected, double totalAmount)
        {
            Assert.AreEqual(expected, totalAmount);
        }
        
        private Accounting GetAccountingInstance(List<Budget> data)
        {
            return new Accounting(new FakeBudgetRepo(data));
        }
        
        private double GetTotalAmountReturnFromInstance(Accounting accountingInstance, DateTime starDate, DateTime endDate)
        {
            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            return totalAmount;
        }

        #endregion
    }
    
    public class FakeBudgetRepo : IBudgetRepo
    {
        private readonly IList<Budget> _data;

        public FakeBudgetRepo(IList<Budget> initData)
        {
            _data = initData;
        }
        public IList<Budget> GetAll()
        {
            return _data;
        }
    }
}
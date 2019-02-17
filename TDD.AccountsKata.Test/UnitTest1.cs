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
            
            var starDate = new DateTime(2019, 01, 05);
            var endDate = new DateTime(2019, 01, 04);

            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            
            AmountShouldBe(0, totalAmount);
        }

        [Test]
        public void QueryTotalWithNoBudgetRecord()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 0, YearMonth = "201904"}
            });
            
            var starDate = new DateTime(2019, 04, 01);
            var endDate = new DateTime(2019, 04, 02);
            
            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            
            AmountShouldBe(0, totalAmount);
        }
        
        [Test]
        public void QueryOneDateBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201901"}
            });
            
            var starDate = new DateTime(2019, 01, 01);
            var endDate = new DateTime(2019, 01, 01);
            
            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            
            AmountShouldBe(10, totalAmount);
        }
        
        [Test]
        public void QueryOneMonthBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201901"}
            });
            
            var starDate = new DateTime(2019, 01, 01);
            var endDate = new DateTime(2019, 01, 31);
            
            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            
            AmountShouldBe(310, totalAmount);
        }
        
        [Test]
        public void QueryTwoMonthBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 280, YearMonth = "201902"},
                new Budget {Amount = 310, YearMonth = "201903"},
            });
            
            var starDate = new DateTime(2019, 02, 28);
            var endDate = new DateTime(2019, 03, 01);
            
            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            
            AmountShouldBe(20, totalAmount);
        }
        [Test]
        public void QueryTwoMonthBudgetWithNoData()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 310, YearMonth = "201903"},
                new Budget {Amount = 0, YearMonth = "201904"}
            });
            
            var starDate = new DateTime(2019, 03, 30);
            var endDate = new DateTime(2019, 04, 04);
            
            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            
            AmountShouldBe(20, totalAmount);
        }

        [Test]
        public void QueryLunarYearBudget()
        {
            var accountingInstance = GetAccountingInstance(new List<Budget>
            {
                new Budget {Amount = 290, YearMonth = "202002"}
            });
            
            var starDate = new DateTime(2020, 02, 01);
            var endDate = new DateTime(2020, 02, 28);

            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);

            AmountShouldBe(280, totalAmount);
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
            
            var starDate = new DateTime(2019, 01, 25);
            var endDate = new DateTime(2019, 03, 05);
            
            var totalAmount = accountingInstance.TotalAmount(starDate, endDate);
            
            AmountShouldBe(400, totalAmount);
        }

        private void AmountShouldBe(double expected, double totalAmount)
        {
            Assert.AreEqual(expected, totalAmount);
        }
        
        private Accounting GetAccountingInstance(List<Budget> data)
        {
            return new Accounting(new FakeBudgetRepo(data));
        }
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
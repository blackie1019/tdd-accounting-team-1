using System;
using System.Collections.Generic;
using System.Linq;

namespace TDD.AccountingKata.Core
{
    public class Accounting
    {
        private readonly IBudgetRepo _budgetRepo;

        public Accounting(IBudgetRepo repo)
        {
            _budgetRepo = repo;
        }
        
        public double TotalAmount(DateTime start, DateTime end)
        {
            var totalAmount = 0d;
            if (IsInValidDateRange(start, end)) return totalAmount;
            
            if (IsSingleMonth(start, end))
            {
                var budgetList = _budgetRepo.GetAll();
                
                return GetMonthTotalAmount(start, end, budgetList, null);
            }
            else
            {
                var budgetList = _budgetRepo.GetAll();
                
                totalAmount += GetFirstAndLastTotalAmounts(start, end, budgetList);
                totalAmount += GetMiddleTotalAmounts(start, end, budgetList);
                
                return totalAmount;
            }
        }

        private double GetMiddleTotalAmounts(DateTime start, DateTime end, IList<Budget> budgetList)
        {
            var totalAmount=0d;
            var monthsInTargetRange = GetMiddleMonthCount(start, end);
            if (monthsInTargetRange <= 1) return totalAmount;
            for (var i = 1; i < monthsInTargetRange; i++)
            {
                var targetDateTime = start.AddMonths(i);
                totalAmount += CalculateMonthTotalAmount(budgetList, 1d, targetDateTime);
            }

            return totalAmount;
        }

        private double GetFirstAndLastTotalAmounts(DateTime start, DateTime end, IList<Budget> budgetList)
        {
            var totalAmount = 0d;
            var filterYearMonths = new List<DateTime> {start, end};

            foreach (var targetDateTime in filterYearMonths)
            {
                totalAmount += GetMonthTotalAmount(start, end, budgetList, targetDateTime);
            }

            return totalAmount;
        }

        private double GetMonthTotalAmount(DateTime start, DateTime end, IList<Budget> budgetList,
            DateTime? targetDateTime)
        {
            var ratioOfMonth = ConvertToRatioOfMonth(start, end, targetDateTime);
            return CalculateMonthTotalAmount(budgetList, ratioOfMonth, targetDateTime ?? start);
        }

        private double CalculateMonthTotalAmount(IList<Budget> budgetList, double ratioOfMonth, DateTime targetDateTime)
        {
            var targetMonthBudget = budgetList.FirstOrDefault(x => x.YearMonth == targetDateTime.ToString("yyyyMM"));

            return targetMonthBudget?.Amount * ratioOfMonth ?? 0d;
        }

        private double ConvertToRatioOfMonth(DateTime start, DateTime end, DateTime? targetDateTime)
        {
            int monthOfDays;
            int daysOfMonth;
            if (targetDateTime.HasValue)
            {
                monthOfDays = GetDaysFromMonth(targetDateTime.Value);
                daysOfMonth = targetDateTime == start
                    ? (monthOfDays - targetDateTime.Value.Day + 1)
                    : targetDateTime.Value.Day;
            }
            else
            {
                monthOfDays = GetDaysFromMonth(start);
                daysOfMonth = (end - start).Days + 1;
            }

            return daysOfMonth / (monthOfDays * 1d);;
        }

        private int GetMiddleMonthCount(DateTime start, DateTime end)
        {
            var diffMonths = 12 * (end.Year - start.Year) + (end.Month - start.Month);
            return diffMonths;

        }

        private bool IsSingleMonth(DateTime start, DateTime end)
        {
            return start.ToString("yyyyMM") == end.ToString("yyyyMM");
        }
        
        private bool IsInValidDateRange(DateTime start, DateTime end)
        {
            return start > end;
        }

        private int GetDaysFromMonth(DateTime targetDateTime)
        {
            return DateTime.DaysInMonth(targetDateTime.Year,targetDateTime.Month);
        }
    }
}
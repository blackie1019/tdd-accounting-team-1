using System;
using System.Collections.Generic;
using System.Linq;
using Tdd.AccountingPractice;

namespace TDD.AccountingKata.Core
{
    public class Accounting
    {
        private IBudgetRepo budgetRepo;

        public Accounting(IBudgetRepo repo)
        {
            budgetRepo = repo;
        }
        
        public double TotalAmount(DateTime start, DateTime end)
        {
            var budgetList = budgetRepo.GetAll();

            if (!IsValidateDateRange(start, end)) return 0d;
            if (IsSameMonth(start, end))
            {
                var targetBudgets = GetBudget(start, budgetList);

                if (targetBudgets == null)
                {
                    return 0d;
                }

                var unitOfDay = targetBudgets.Amount / GetDayInTargetMonth(start);
                var daysOfTargetMonth = GetDifferentDays(start, end);

                return CalculateAmount(unitOfDay, daysOfTargetMonth);
            }

            var totalAmount = 0d;

            totalAmount += GeFirstAndLastTotalAmounts(start, end, budgetList);

            totalAmount += GetMiddleTotalAmounts(start, end, budgetList);

            return totalAmount;

        }

        private double GeFirstAndLastTotalAmounts(DateTime start, DateTime end, IEnumerable<Budget> budgetList)
        {
            var totalAmount = 0d;
            var filterYearMonths = new List<DateTime>() {start, end};

            foreach (var targetDateTime in filterYearMonths)
            {
                var targetMonthBudget = GetTargetMonthBudget(budgetList, targetDateTime);
                if (targetMonthBudget != null)
                {
                    var monthOfDays = GetDayInTargetMonth(targetDateTime);
                    var unitOfDay = Convert.ToDouble(targetMonthBudget.Amount / monthOfDays);
                    var targetAmount = 0d;
                    targetAmount = GetTargetAmount(start, end, targetDateTime, targetAmount, unitOfDay,
                        monthOfDays);
                    totalAmount += targetAmount;
                }
            }

            return totalAmount;
        }
        

        private double GetMiddleTotalAmounts(DateTime start, DateTime end, IEnumerable<Budget> budgetList)
        {
            var monthsInTargetRange = GetMonthsInTargetRange(start, end);
            var totalAmount = 0d;
            if (monthsInTargetRange > 1)
            {
                for (int i = 1; i < monthsInTargetRange; i++)
                {
                    var searchMonth = start.AddMonths(i);
                    var targetMonthBudget = GetTargetMonthBudget(budgetList, searchMonth);
                    if (targetMonthBudget != null)
                    {
                        totalAmount += targetMonthBudget.Amount;
                    }
                }
            }

            return totalAmount;
        }

        private Budget GetTargetMonthBudget(IEnumerable<Budget> budgetList, DateTime targetDateTime)
        {
            return budgetList.FirstOrDefault(x => x.YearMonth == targetDateTime.ToString("yyyyMM"));
        }

        private double GetMonthsInTargetRange(DateTime start, DateTime end)
        {
            var diffMonths = 12 * (end.Year - start.Year) + (end.Month - start.Month);
            return diffMonths;

        }

        private double GetTargetAmount(DateTime start, DateTime end, DateTime targetDateTime, double targetAmount,
            double unitOfDay, int monthOfDays)
        {
            if (targetDateTime == start)
            {
                targetAmount = unitOfDay * (monthOfDays - targetDateTime.Day + 1);
            }
            else if (targetDateTime == end)
            {
                targetAmount = unitOfDay * targetDateTime.Day;
            }

            return targetAmount;
        }

        private bool IsSameMonth(DateTime start, DateTime end)
        {
            
            return start.ToString("yyyyMM") == end.ToString("yyyyMM");
        }

        private double CalculateAmount(double unitOfDay, double daysOfTargetMonth)
        {
            return unitOfDay*daysOfTargetMonth;
        }

        private bool IsValidateDateRange(DateTime start, DateTime end)
        {
            return start <= end;
        }

        private Budget GetBudget(DateTime start, IEnumerable<Budget> budegtList)
        {
            return budegtList.FirstOrDefault(x => x.YearMonth == start.ToString("yyyyMM"));
        }

        private int GetDifferentDays(DateTime start,DateTime end)
        {
            return (end-start).Days+1;
        }

        private int GetDayInTargetMonth(DateTime targetDateTime)
        {
            return DateTime.DaysInMonth(targetDateTime.Year,targetDateTime.Month);
        }
    }
}
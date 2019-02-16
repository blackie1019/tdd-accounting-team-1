using System.Collections.Generic;
using TDD.AccountingKata.Core;

namespace Tdd.AccountingPractice
{
    public interface IBudgetRepo
    {
        IEnumerable<Budget> GetAll();
    }
}
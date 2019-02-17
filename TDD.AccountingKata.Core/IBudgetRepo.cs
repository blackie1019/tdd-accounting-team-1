using System.Collections.Generic;

namespace TDD.AccountingKata.Core
{
    public interface IBudgetRepo
    {
        IList<Budget> GetAll();
    }
}
using FinanceManagement.Core.Entities;

namespace FinanceManagement.API.Helpers
{
    public interface IFileImporter
    {
        public IEnumerable<FinancialTransaction> GetFinancialTransactionsFromFile(StreamReader streamReader);
    }
}

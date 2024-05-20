using System.Xml.Serialization;

namespace FinanceManagement.API.DTOs.FinancialTransactions
{
    [XmlRoot("transactions")]
    public class FinancialTransactionsCreateDto
    {
        [XmlElement("transaction")]
        public List<FinancialTransactionCreateDto> FinancialTransactions { get; set; }
    }
}

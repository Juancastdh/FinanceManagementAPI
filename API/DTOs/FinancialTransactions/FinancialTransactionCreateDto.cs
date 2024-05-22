using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FinanceManagement.API.DTOs.FinancialTransactions
{

    [XmlRoot("transaction")]
    public class FinancialTransactionCreateDto
    {
        [XmlElement("date")]
        public DateTime Date { get; set; }
        [XmlElement("value")]
        public decimal Value { get; set; }
        [XmlElement("description")]
        public string? Description { get; set; }
        [XmlElement("categoryId")]
        public int CategoryId { get; set; }
        [XmlElement("isExpense")]
        public bool IsExpense { get; set; }
        [XmlElement("periodId")]
        public int PeriodId { get; set; }

    }
}

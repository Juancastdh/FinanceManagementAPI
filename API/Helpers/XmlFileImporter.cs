using AutoMapper;
using FinanceManagement.API.DTOs.FinancialTransactions;
using FinanceManagement.Core.Entities;
using System.Xml.Serialization;

namespace FinanceManagement.API.Helpers
{
    public class XMLFileImporter: IFileImporter
    {

        private readonly IMapper Mapper;

        public XMLFileImporter(IMapper mapper)
        {
            Mapper = mapper;
        }

        public IEnumerable<FinancialTransaction> GetFinancialTransactionsFromFile(StreamReader streamReader)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FinancialTransactionsCreateDto));

            FinancialTransactionsCreateDto financialTransactionsCreateDto = (FinancialTransactionsCreateDto?)deserializer.Deserialize(streamReader) ?? new FinancialTransactionsCreateDto();

            IEnumerable<FinancialTransaction> financialTransactions = Mapper.Map<IEnumerable<FinancialTransaction>>(financialTransactionsCreateDto.FinancialTransactions);

            return financialTransactions;
        }


    }
}

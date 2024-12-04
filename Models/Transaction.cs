using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;
using Expensify.Csv.TypeConversion;


namespace Expensify.Models;

[Table("Transactions")]
public class Transaction
{
    [Ignore]
    public int Id { get; set; }

    [Name("Transaction Date"), Format("dd/MM/yyyy")]
    public DateOnly TransactionDate { get; set; }

    [Name("Transaction Number")]
    public string TransactionNumber { get; set; } = string.Empty;

    [Name("Description")]
    public string Description { get; set; } = string.Empty;

    [Name("Currency")]
    public string Currency { get; set; } = string.Empty;

    [Name("Debit")]
    [TypeConverter(typeof(DecimalTypeConverter))]
    public decimal Debit { get; set; }

    [Name("Credit")]
    [TypeConverter(typeof(DecimalTypeConverter))]
    public decimal Credit { get; set; }

    [Name("Running Balance")]
    [TypeConverter(typeof(DecimalTypeConverter))]
    public decimal RunningBalance { get; set; }
}

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Expensify.Csv.TypeConversion;


public class DecimalTypeConverter : DecimalConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        decimal result;
        decimal.TryParse(text, out result);
        return result;
    }
}
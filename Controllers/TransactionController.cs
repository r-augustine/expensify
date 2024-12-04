using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Expensify.Context;
using Expensify.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Expensify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public string BasePath { get; }

        public TransactionController(AppDbContext context)
        {
            _context = context;
            BasePath = Path.GetFullPath("Imports");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Transaction>>> Index()
        {
            return await _context.Transactions.ToListAsync();
        }

        [HttpPost("upload")]
        public async Task<ActionResult> ImportTransactions(IFormFile file)
        {
            string fileName = Path.Combine(BasePath, file.FileName);

            try
            {
                if (!System.IO.Directory.Exists(BasePath))
                {
                    System.IO.Directory.CreateDirectory(BasePath);
                }

                if (!System.IO.File.Exists(fileName))
                {
                    // do not save the file as the same name
                    // what happens if the csv file is ill-formatted?
                    using (var stream = System.IO.File.Create(fileName))
                    {
                        await file.CopyToAsync(stream);
                    }
                }


                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                };

                using (StreamReader rs = new StreamReader(fileName))
                using (var csv = new CsvReader(rs, config))
                {
                    var records = csv.GetRecordsAsync<Transaction>();

                    await _context.Database.BeginTransactionAsync();

                    await foreach (var r in records)
                    {
                        // add a logger
                        Console.WriteLine(r.TransactionDate);
                        Console.WriteLine(r.TransactionNumber);
                        Console.WriteLine(r.Description);
                        Console.WriteLine(r.Debit);
                        Console.WriteLine(r.Credit);
                        Console.WriteLine(r.RunningBalance);
                        Console.WriteLine("---");

                        if ((await _context.Transactions.SingleOrDefaultAsync(t => t.TransactionNumber == r.TransactionNumber)) == null)
                        {
                            await _context.AddAsync(r);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }

                return NoContent();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}

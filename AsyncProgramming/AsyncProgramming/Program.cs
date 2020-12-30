using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgramming.Database;
using Microsoft.EntityFrameworkCore;

namespace AsyncProgramming
{
    internal class Program
    {
        private static void Main(string[] args) => Console.WriteLine("Hello World!");

        public async Task<string> FetchStringFromServer()
        {
            // Simulate delay
            await Task.Delay(3000);

            return "Fetched string";
        }
    }

    class MyService
    {
        private readonly MyAppContext _dbContext;
        private readonly AddressService _addressService;

        public MyService(MyAppContext dbContext, AddressService addressService)
        {
            _dbContext = dbContext;
            _addressService = addressService;
        }

        public async Task SyncUserAddressAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Start both I/O operations concurrently
            var userTask = _dbContext.Users.SingleAsync(u => u.Id == userId, cancellationToken);
            var addressTask = _addressService.GetAddressAsync(userId, cancellationToken);

            // Wait for both tasks to complete. This line is optional, but makes code more readable. Also, it will wait for both tasks even the first one would fail.
            await Task.WhenAll(userTask, addressTask);

            // With Task.WhenALl above, awaits are actually redundant in this scenario, because those tasks has been completed. It COULD be replaced with the following code:
            // userTask.Result.Address = addressTask.Result;
            // However, it can lead to problems in other scenarios
            var user = await userTask;
            var address = await addressTask;
            user.Address = address;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    class AddressService
    {
        public async Task<string> GetAddressAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Simulate delay
            await Task.Delay(3000);

            return "Fetched address";
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgramming.Database;
using Microsoft.EntityFrameworkCore;

namespace AsyncProgramming
{
    internal class MyService
    {
        private readonly AddressService _addressService;
        private readonly MyAppContext _dbContext;

        public MyService(MyAppContext dbContext, AddressService addressService)
        {
            _dbContext = dbContext;
            _addressService = addressService;
        }

        public async Task SyncUserAddressAsync(Guid userId)
        {
            // Start both I/O operations concurrently
            var user = await _dbContext.Users.SingleAsync(u => u.Id == userId);
            var address = await _addressService.GetAddressAsync(userId);

            user.Address = address;

            await _dbContext.SaveChangesAsync();
        }

        public async Task SyncUserAddressInParallelAsync(Guid userId)
        {
            // Start both I/O operations concurrently
            var userTask = _dbContext.Users.SingleAsync(u => u.Id == userId);
            var addressTask = _addressService.GetAddressAsync(userId);

            // Wait for both tasks to complete.
            // This line is optional, but makes code more readable.
            // Also, it will wait for both tasks even the first one would fail.
            await Task.WhenAll(userTask, addressTask);

            // With Task.WhenALl above, awaits are actually redundant in this scenario, because those tasks has been completed.
            // It COULD be replaced with the following code:
            // userTask.Result.Address = addressTask.Result;
            // However, it can lead to problems in other scenarios
            var user = await userTask;
            var address = await addressTask;
            user.Address = address;

            await _dbContext.SaveChangesAsync();
        }

        public async Task SyncUserAddressInParallelAsync(Guid userId, CancellationToken cancellationToken)
        {
            // Start both I/O operations concurrently
            var userTask = _dbContext.Users.SingleAsync(u => u.Id == userId, cancellationToken);
            var addressTask = _addressService.GetAddressAsync(userId, cancellationToken);

            // Wait for both tasks to complete.
            // This line is optional, but makes code more readable.
            // Also, it will wait for both tasks even the first one would fail.
            await Task.WhenAll(userTask, addressTask).ConfigureAwait(false);

            // With Task.WhenALl above, awaits are actually redundant in this scenario, because those tasks has been completed.
            // It COULD be replaced with the following code:
            // userTask.Result.Address = addressTask.Result;
            // However, it can lead to problems in other scenarios
            var user = await userTask.ConfigureAwait(false);
            var address = await addressTask.ConfigureAwait(false);
            user.Address = address;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}

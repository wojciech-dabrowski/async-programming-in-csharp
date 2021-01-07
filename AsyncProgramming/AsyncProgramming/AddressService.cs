using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgramming
{
    public class AddressService
    {
        public async Task<string> GetAddressAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Simulate delay
            await Task.Delay(3000, cancellationToken);

            return "Fetched address";
        }
    }
}

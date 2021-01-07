using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncProgramming
{
    public class SpammingAsync
    {
        private readonly AddressService _addressService;

        public SpammingAsync(AddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<IReadOnlyList<string>> GetAddressesAsync(IEnumerable<Guid> ids)
        {
            var results = new List<string>();

            foreach (var id in ids)
            {
                var result = await _addressService.GetAddressAsync(id);
                results.Add(result);
            }

            return results;
        }

        public async Task<IReadOnlyList<string>> GetAddressesInParallelAsync(IEnumerable<Guid> ids)
        {
            // Start all calls immediately.
            // Be careful using this method.
            // We can flood our traffic, thread pool, or external address service.
            var tasks = ids.Select(id => _addressService.GetAddressAsync(id));

            // Wait for all calls to complete.
            var result = await Task.WhenAll(tasks);
            return result;
        }
    }
}

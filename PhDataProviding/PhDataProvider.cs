using Core.Interfaces;
using Core.Models;
using System.Reflection.Metadata.Ecma335;

namespace PhDataProviding
{
    public class PhDataProvider : IPhDataProvider
    {
        public async IAsyncEnumerable<float> GetData()
        {
            IEnumerable<float> data = new List<float>() { 1.1f ,1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f };
            Random rand = new Random(); // DATA FAKING

            foreach (var item in data)
            {
               await Task.Delay(TimeSpan.FromSeconds(1));
               yield return await Task.FromResult(item);
            }
        }
    }
}
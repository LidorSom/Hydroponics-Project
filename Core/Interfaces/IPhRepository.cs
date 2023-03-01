using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPhRepository
    {
        public Task WritePh(PhModel phModel);
        public Task<PhModel> ReadPh(Guid guid);

    }
}

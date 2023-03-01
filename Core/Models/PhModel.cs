using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PhModel
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public int SystemId { get; set; }
    }
}

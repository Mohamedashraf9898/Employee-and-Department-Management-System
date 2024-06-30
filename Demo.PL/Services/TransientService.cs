using System;

namespace Demo.PL.Services
{
    public class TransientService : ITransientService
    {
        public Guid Guid { get; set; }
        public TransientService()
        {
            Guid = Guid.NewGuid();
        }
        public Guid GetGuid()
        {
            return Guid;
        }
        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}

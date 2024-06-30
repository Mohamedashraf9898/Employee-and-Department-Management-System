using System;

namespace Demo.PL.Services
{
    public class ScopedService : IScopedService
    {
        public Guid Guid { get; set; }
        public ScopedService()
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

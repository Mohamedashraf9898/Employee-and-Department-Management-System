using System;

namespace Demo.PL.Services
{
    public class SingletoneService : ISingletoneService
    {
        public Guid Guid { get; set; }
        public SingletoneService()
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

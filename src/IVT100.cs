using System;
using System.Text;

namespace libVT100
{
    public interface IVT100 : IDisposable
    {
        Encoding Encoding { get; set; }
        
        void Input ( byte[] _data );
        
        void Subscribe ( IVT100Client _client );
        void UnSubscribe ( IVT100Client _client );
    }
}

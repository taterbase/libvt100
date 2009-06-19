using System;
using System.Text;

namespace libVT100
{
    public interface IVT100Decoder : IDisposable
    {
        Encoding Encoding { get; set; }
        
        void Input ( byte[] _data );
        
        void Subscribe ( IVT100DecoderClient _client );
        void UnSubscribe ( IVT100DecoderClient _client );
    }
}

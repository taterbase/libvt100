using System;
using System.Text;

namespace libVT100
{
    public delegate void DecoderOutputDelegate ( IDecoder _decoder, byte[] _output );

    public interface IDecoder : IDisposable    
    {
        Encoding Encoding { get; set; }
        
        void Input ( byte[] _data );
        
        event DecoderOutputDelegate Output;
    }
}

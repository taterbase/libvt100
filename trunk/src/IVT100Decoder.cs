using System;
using System.Text;

namespace libVT100
{
    public interface IVT100Decoder : IDecoder
    {
        void Subscribe ( IVT100DecoderClient _client );
        void UnSubscribe ( IVT100DecoderClient _client );
    }
}

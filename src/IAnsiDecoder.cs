using System;
using System.Text;

namespace libVT100
{
    public interface IAnsiDecoder : IDecoder
    {
        void Subscribe ( IAnsiDecoderClient _client );
        void UnSubscribe ( IAnsiDecoderClient _client );
    }
}

namespace libVT100
{
    public interface IVT100Decoder : IAnsiDecoder
    {
        void Subscribe ( IVT100DecoderClient _client );
        void UnSubscribe ( IVT100DecoderClient _client );
    }
}

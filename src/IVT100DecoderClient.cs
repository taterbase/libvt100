using System;
using System.Drawing;

namespace libVT100
{
   public enum DeviceStatus
   {
      Unknown = -1,
      Ok = 0,
      Failure = 3,
   }
   
   public interface IVT100DecoderClient : IAnsiDecoderClient
   {
      string GetDeviceCode ( IVT100Decoder _decoder );
      DeviceStatus GetDeviceStatus ( IVT100Decoder _decoder );
   }
}

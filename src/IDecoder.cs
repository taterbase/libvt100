using System;
using System.Text;
using System.Windows.Forms;

namespace libVT100
{
    public delegate void DecoderOutputDelegate ( IDecoder _decoder, byte[] _output );

    public interface IDecoder : IDisposable    
    {
        Encoding Encoding { get; set; }
        
        void Input ( byte[] _data );
        
        event DecoderOutputDelegate Output;

       void KeyPressed( Keys _modifiers, Keys _key );
       void CharacterTyped( char _character );
    }
}

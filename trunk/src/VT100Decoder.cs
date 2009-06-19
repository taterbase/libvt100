using System;
using System.Text;
using System.Collections.Generic;

namespace libVT100
{
    public class VT100Decoder : EscapeCharacterDecoder, IVT100Decoder
    {
        protected List<IVT100DecoderClient> m_listeners;
        
        Encoding IVT100Decoder.Encoding
        {
            get
            {
                return m_encoding;
            }
            set
            {
                if ( m_encoding != value )
                {
                    m_encoding = value;
                    m_decoder = m_encoding.GetDecoder();
                    m_encoder = m_encoding.GetEncoder();
                }
            }
        }
        
        public VT100Decoder ()
            : base()
        {
            m_listeners = new List<IVT100DecoderClient> ();
        }
        
        protected override void ProcessCommand ( byte _command, String _parameter )
        {
            System.Console.WriteLine ( "ProcessCommand: {0} {1}", (char) _command, _parameter );
            
        }
        
        protected override void OnCharacters ( char[] _characters )
        {
            foreach ( IVT100DecoderClient client in m_listeners )
            {
                client.Characters ( this, _characters );
            }
        }
        
        void IVT100Decoder.Input ( byte[] _data )
        {
            base.Input ( _data );
        }
        
        void IVT100Decoder.Subscribe ( IVT100DecoderClient _client )
        {
            m_listeners.Add ( _client );
        }
        
        void IVT100Decoder.UnSubscribe ( IVT100DecoderClient _client )
        {
            m_listeners.Remove ( _client );
        }

        void IDisposable.Dispose ()
        {
            m_listeners.Clear();
            m_listeners = null;
        }
    }
}

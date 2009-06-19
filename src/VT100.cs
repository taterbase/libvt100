using System;
using System.Text;
using System.Collections.Generic;

namespace libVT100
{
    public class VT100 : IVT100
    {
        public const byte EscapeCharacter = 0x1B;
        public const byte LeftBracketCharacter = 0x5B;
        
        protected enum State
        {
            Normal,
            Command,
        }
        protected State m_state;
        protected List<IVT100Client> m_listeners;
        protected Encoding m_encoding;
        protected Decoder m_decoder;
        protected Encoder m_encoder;
        protected List<byte> m_commandBuffer;
        
        Encoding IVT100.Encoding
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
        
        public VT100 ()
        {
            m_listeners = new List<IVT100Client> ();
            m_state = State.Normal;
            (this as IVT100).Encoding = Encoding.ASCII;
            m_commandBuffer = new List<byte>();
        }
        
        virtual protected bool IsValidParameterCharacter ( char _c )
        {
            return (Char.IsNumber(_c) || _c == '(' || _c == ')' || _c == ';' || _c == '"');
        }
        
        protected void ProcessCommandBuffer ()
        {
            System.Console.Write ( "ProcessCommandBuffer: " );
            foreach ( byte b in m_commandBuffer )
            {
                System.Console.Write ( "{0:X2} ", b );
            }
            System.Console.WriteLine ( "" );
            
            m_state = State.Command;
            
            if ( m_commandBuffer.Count > 1 )
            {
                if ( m_commandBuffer[0] != EscapeCharacter )
                {
                    throw new Exception ( "Internal error, first command character _MUST_ be the escape character, please report this bug to the author." );
                }
                
                int start = 1;
                // Is this a one or two byte escape code?
                if ( m_commandBuffer[start] == LeftBracketCharacter )
                {
                    start ++;
                    
                    // It is a two byte escape code, but we still need more data
                    if ( m_commandBuffer.Count < 3 )
                    {
                        return;
                    }
                }
                
                bool insideQuotes = false;
                int end = start;
                while ( end < m_commandBuffer.Count && (IsValidParameterCharacter((char) m_commandBuffer[end]) || insideQuotes) )
                {
                    if ( m_commandBuffer[end] == '"' )
                    {
                        insideQuotes = !insideQuotes;
                    }
                    end++;
                }
                
                if ( end == m_commandBuffer.Count )
                {
                    // More data needed
                    return;
                }
                
                Decoder decoder = (this as IVT100).Encoding.GetDecoder();
                byte[] parameterData = new byte[end - start];
                for ( int i = 0; i < parameterData.Length; i++ )
                {
                    parameterData[i] = m_commandBuffer[start + i];
                }
                int parameterLength = decoder.GetCharCount ( parameterData, 0, parameterData.Length );
                char[] parameterChars = new char[parameterLength];
                decoder.GetChars ( parameterData, 0, parameterData.Length, parameterChars, 0 );
                String parameter = new String ( parameterChars );
                
                byte command = m_commandBuffer[end];
                
                ProcessCommand ( command, parameter );
                
                System.Console.WriteLine ( "Remove the processed commands" );
                
                // Remove the processed commands
                if ( m_commandBuffer.Count == end - 1 )
                {
                    // All command bytes processed, we can go back to normal handling
                    m_commandBuffer.Clear();
                    m_state = State.Normal;
                }
                else
                {
                    for ( int i = end + 1; i < m_commandBuffer.Count; i++ )
                    {
                        if ( m_commandBuffer[i] == EscapeCharacter )
                        {
                            m_commandBuffer.RemoveRange ( 0, i );
                            ProcessCommandBuffer ();
                            return;
                        }
                        else
                        {
                            ProcessNormalInput ( m_commandBuffer[i] );
                        }
                    }
                    m_commandBuffer.Clear ();
                    
                    m_state = State.Normal;
                }
            }
        }
        
        protected void ProcessNormalInput ( byte _data )
        {
            System.Console.WriteLine ( "ProcessNormalInput: {0:X2}", _data );
            if ( _data == EscapeCharacter )
            {
                throw new Exception ( "Internal error, ProcessNormalInput was passed an escape character, please report this bug to the author." );
            }
                            
            byte[] data = new byte[] { _data };
            int charCount = m_decoder.GetCharCount ( data, 0, 1 );
            if ( charCount > 0 )
            {
                char[] characters = new char[charCount];
                m_decoder.GetChars ( data, 0, 1, characters, 0 );
                
                OnCharacters ( characters );
            }
        }
        
        virtual protected void ProcessCommand ( byte _command, String _parameter )
        {
            System.Console.WriteLine ( "ProcessCommand: {0} {1}", (char) _command, _parameter );
            
        }
        
        void IVT100.Input ( byte[] _data )
        {
            System.Console.Write ( "Input[{0}]: ", m_state );
            foreach ( byte b in _data )
            {
                System.Console.Write ( "{0:X2} ", b );
            }
            System.Console.WriteLine ( "" );
            
            if ( _data.Length == 0 )
            {
                throw new ArgumentException ( "Input can not process an empty array." );
            }
            
            switch ( m_state )
            {
            case State.Normal:
                if ( _data[0] == EscapeCharacter )
                {
                    m_commandBuffer.AddRange ( _data );
                    ProcessCommandBuffer ();
                }
                else
                {
                    int i = 0;
                    while ( i < _data.Length && _data[i] != EscapeCharacter )
                    {
                        ProcessNormalInput ( _data[i] );
                        i++;
                    }
                    if ( i != _data.Length )
                    {
                        while ( i < _data.Length )
                        {
                            m_commandBuffer.Add ( _data[i] );
                            i++;
                        }
                        ProcessCommandBuffer ();
                    }
                }
                break;
                
            case State.Command:
                m_commandBuffer.AddRange ( _data );
                ProcessCommandBuffer ();
                break;
            }
        }
        
        void IVT100.Subscribe ( IVT100Client _client )
        {
            m_listeners.Add ( _client );
        }

        void IVT100.UnSubscribe ( IVT100Client _client )
        {
            m_listeners.Remove ( _client );
        }
        
        void IDisposable.Dispose ()
        {
            m_listeners.Clear();
            m_listeners = null;
        }
        
        protected virtual void OnCharacters ( char[] _characters )
        {
            //System.Console.WriteLine ( "OnCharacters: {0} characters", _characters.Length );
            
            foreach ( IVT100Client client in m_listeners )
            {
                client.Characters ( this, _characters );
            }
        }
    }
}

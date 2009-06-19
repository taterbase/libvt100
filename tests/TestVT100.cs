using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using libVT100;

namespace libVT100.Tests
{
    [TestFixture]
    public class TestVT100
    {
        private List<char[]> m_chars;
        private IVT100Decoder m_vt100;
        private VT100DecoderClient m_client;
        
        [SetUp]
        public void SetUp ()
        {
            m_vt100 = new VT100Decoder ();
            m_client = new VT100DecoderClient();
            m_chars = new List<char[]>();
            
            m_vt100.Subscribe ( m_client );
            
            m_client.Characters += new VT100DecoderClient.CharactersDelegate ( Characters );
        }
        
        [TearDown]
        public void TearDown ()
        {
            m_client.Characters -= new VT100DecoderClient.CharactersDelegate ( Characters );
            
            m_vt100.UnSubscribe ( m_client );
            
            (m_client as IDisposable).Dispose();
            m_vt100.Dispose();
            m_chars = null;
            m_client = null;
            m_vt100 = null;
            m_chars = null;
        }
        
        private void Input ( String _input )
        {
            byte[] data = new byte[_input.Length];
            int i = 0;
            foreach ( char c in _input )
            {
                data[i] = (byte) c;
                i++;
            }
            m_vt100.Input ( data );
        }
        
        private void Characters ( VT100DecoderClient _client, char[] _chars )
        {
            m_chars.Add ( _chars );
        }
    }
}

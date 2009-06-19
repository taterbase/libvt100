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
        private IVT100 m_vt100;
        private VT100Client m_client;
        
        [SetUp]
        public void SetUp ()
        {
            m_vt100 = new VT100 ();
            m_client = new VT100Client();
            m_chars = new List<char[]>();
            
            m_vt100.Subscribe ( m_client );
            
            m_client.Characters += new VT100Client.CharactersDelegate ( Characters );
        }
        
        [TearDown]
        public void TearDown ()
        {
            m_client.Characters -= new VT100Client.CharactersDelegate ( Characters );
            
            m_vt100.UnSubscribe ( m_client );
            
            (m_client as IDisposable).Dispose();
            m_vt100.Dispose();
            m_chars = null;
            m_client = null;
            m_vt100 = null;
            m_chars = null;
        }
        
        [Test]
        public void TestNormalCharactersAreJustPassedThrough ()
        {
            m_vt100.Input ( new byte[] { (byte) 'A', (byte) 'B', (byte) 'C', (byte) 'D', (byte) 'E' } );
            
            Assert.AreEqual ( "ABCDE", ReceivedCharacters );
        }
        
        [Test]
        public void TestCommandsAreNotInterpretedAsNormalCharacters ()
        {
            //m_vt100.Input ( new byte[] { (byte) 'A', (byte) 'B', 0x1B, (byte) '1', (byte) '2', (byte) '3', (byte) 'm', (byte) 'C', (byte) 'D', (byte) 'E' } );
            //Assert.AreEqual ( "ABCDE", ReceivedCharacters );
            
            Input ( "AB\x001B123mCDE" );
            Assert.AreEqual ( "ABCDE", ReceivedCharacters );
            
            Input ( "AB\x001B123mCDE" );
            Assert.AreEqual ( "ABCDE", ReceivedCharacters );
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
        
        private String ReceivedCharacters
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach ( char[] chars in m_chars )
                {
                    builder.Append ( chars );
                }
                m_chars.Clear();
                return builder.ToString();
            }
        }
        
        private void Characters ( VT100Client _client, char[] _chars )
        {
            m_chars.Add ( _chars );
        }
    }
}

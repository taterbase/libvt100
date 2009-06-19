using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using libVT100;

namespace libVT100.Tests
{
    [TestFixture]
    public class TestEscapeCharacterDecoder : EscapeCharacterDecoder
    {
        private List<char[]> m_chars;
        
        [SetUp]
        public void SetUp ()
        {
            m_chars = new List<char[]>();
        }
        
        [TearDown]
        public void TearDown ()
        {
            m_chars = null;
        }
        
        [Test]
        public void TestNormalCharactersAreJustPassedThrough ()
        {
            Input ( new byte[] { (byte) 'A', (byte) 'B', (byte) 'C', (byte) 'D', (byte) 'E' } );
            
            Assert.AreEqual ( "ABCDE", ReceivedCharacters );
        }
        
        [Test]
        public void TestCommandsAreNotInterpretedAsNormalCharacters ()
        {
            Input ( new byte[] { (byte) 'A', (byte) 'B', 0x1B, (byte) '1', (byte) '2', (byte) '3', (byte) 'm', (byte) 'C', (byte) 'D', (byte) 'E' } );
            Assert.AreEqual ( "ABCDE", ReceivedCharacters );
            
            Input ( "\x001B123mA" );
            Assert.AreEqual ( "A", ReceivedCharacters );
            
            Input ( "\x001B123m\x001B123mA" );
            Input ( "A" );
            Assert.AreEqual ( "AA", ReceivedCharacters );
            
            Input ( "AB\x001B123mCDE" );
            Assert.AreEqual ( "ABCDE", ReceivedCharacters );
            
            Input ( "AB\x001B123m" );
            Assert.AreEqual ( "AB", ReceivedCharacters );
            
            Input ( "A" );
            Input ( "AB\x001B123mCDE\x001B123m\x001B123mCDE" );
            Assert.AreEqual ( "AABCDECDE", ReceivedCharacters );

            Input ( "A\x001B[123m\x001B[123mA" );
            Input ( "A" );
            Assert.AreEqual ( "AAA", ReceivedCharacters );
            
            Input ( "A\x001B123m\x001B[123mA" );
            Assert.AreEqual ( "AA", ReceivedCharacters );

            Input ( "A\x001B[123;321;456a\x001B[\"This string is part of the command\"123bA" );
            Assert.AreEqual ( "AA", ReceivedCharacters );
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
            Input ( data );
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
        
        override protected void ProcessCommand ( byte _command, String _parameter )
        {
        }
        
        override protected void OnCharacters ( char[] _chars )
        {
            m_chars.Add ( _chars );
        }
    }
}

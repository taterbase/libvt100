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
        private struct Command
        {
            public byte m_command;
            public string m_parameter;
            
            public Command ( byte _command, string _parameter )
            {
                m_command = _command;
                m_parameter = _parameter;
            }
        }
        
        private List<char[]> m_chars;
        private List<Command> m_commands;
        
        [SetUp]
        public void SetUp ()
        {
            m_chars = new List<char[]>();
            m_commands = new List<Command>();
        }
        
        [TearDown]
        public void TearDown ()
        {
            m_chars = null;
            m_commands = null;
        }
        
        [Test]
        public void TestNormalCharactersAreJustPassedThrough ()
        {
            (this as IDecoder).Input ( new byte[] { (byte) 'A', (byte) 'B', (byte) 'C', (byte) 'D', (byte) 'E' } );
            
            Assert.AreEqual ( "ABCDE", ReceivedCharacters );
        }
        
        [Test]
        public void TestCommandsAreNotInterpretedAsNormalCharacters ()
        {
            (this as IDecoder).Input ( new byte[] { (byte) 'A', (byte) 'B', 0x1B, (byte) '1', (byte) '2', (byte) '3', (byte) 'm', (byte) 'C', (byte) 'D', (byte) 'E' } );
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
        
        [Test]
        public void TestCommands ()
        {
            Input ( "A\x001B123m\x001B[123mA" );
            AssertCommand ( 'm', "123" );
            AssertCommand ( 'm', "123" );
            
            Input ( "A\x001B[123;321;456a\x001B[\"This string is part of the command\"123bA" );
            AssertCommand ( 'a', "123;321;456" );
            AssertCommand ( 'b', "\"This string is part of the command\"123" );
        }
        
        private void AssertCommand ( char _command, string _parameter )
        {
            Assert.IsNotEmpty ( m_commands );
            Assert.AreEqual ( (byte) _command, m_commands[0].m_command );
            Assert.AreEqual ( _parameter, m_commands[0].m_parameter );
            m_commands.RemoveAt ( 0 );
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
            (this as IDecoder).Input ( data );
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
            m_commands.Add ( new Command(_command, _parameter) );
        }
        
        override protected void OnCharacters ( char[] _chars )
        {
            m_chars.Add ( _chars );
        }
        
        override public event DecoderOutputDelegate Output;
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using libVT100;

namespace libVT100.Tests
{
    [TestFixture]
    public class TestSystemTextDecoder
    {
        byte[] data = new byte[] { 0xc3, 0x86, 0x73, 0x6c, 0x65, 0x74 };
        
        [Test]
        public void TestCallWithAllData ()
        {
            Encoding encoding = Encoding.UTF8;
            Decoder decoder = encoding.GetDecoder();
            int chars = decoder.GetCharCount( data, 0, data.Length );
            char[] characters = new char[chars];
            decoder.GetChars ( data, 0, data.Length, characters, 0 );
            Assert.AreEqual ( "Æslet", new String(characters) );
            
        }

        [Test]
        public void TestCallOneByteAtATime ()
        {
            Encoding encoding = Encoding.UTF8;
            Decoder decoder = encoding.GetDecoder();
            String output = "";
            foreach ( byte b in data )
            {
                int chars = decoder.GetCharCount( new byte[] { b }, 0, 1 );
                char[] characters = new char[chars];
                decoder.GetChars ( new byte[] { b }, 0, 1, characters, 0 );
                if ( chars > 0 )
                {
                    output += characters[0];
                }
            }
            Assert.AreEqual ( "Æslet", output );
        }
    }
}

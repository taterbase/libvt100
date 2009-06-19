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
    public class TestAnsiArt
    {
        [Test]
        public void TestWendy ()
        {
           ReadAndRenderFile ( "../tests/70-twilight.ans", Encoding.GetEncoding ( "ibm437"), new Size(80, 80) );
           ReadAndRenderFile ( "../tests/n4-wendy.ans", Encoding.GetEncoding ( "ibm437"), new Size(80, 80)  );
           ReadAndRenderFile( "../tests/zv-v01d.ans", Encoding.GetEncoding( "ibm437" ), new Size( 80, 180 ) );
        }
        
        [Test]
        public void TestSimpleTxt ()
        {
            System.Console.Write ( ReadAndRenderFile ( "../tests/simple.txt", Encoding.UTF8, new Size(50, 6) ).ToString() );
        }
        
        [Test]
        public void TestUnixProgramOutput ()
        {
            ReadAndRenderFile ( "../tests/mc.output", Encoding.UTF8, new Size(65, 180) );
            ReadAndRenderFile ( "../tests/ls.output", Encoding.UTF8, new Size(65, 10) );
        }
        
        public void ReadAndRenderFileAll ( string _filename, Size _size )
        {
            foreach ( EncodingInfo encodingInfo in Encoding.GetEncodings() )
            {
                ReadAndRenderFile ( _filename, encodingInfo.GetEncoding(), _size );
            }
        }
        
        public Screen ReadAndRenderFile ( string _filename, Encoding _encoding, Size _size )
        {
            IVT100Decoder vt100 = new VT100Decoder();
            //vt100.Encoding = Encoding.GetEncoding ( encodingInfo.Name, new EncoderExceptionFallback(), new DecoderReplacementFallback ("U") );
            vt100.Encoding = _encoding;
            Screen screen = new Screen ( _size.Width, _size.Height );
            vt100.Subscribe ( screen );
           
            using ( Stream stream = File.Open( _filename, FileMode.Open ) )
            {
                try
                {
                    int read = 0;
                    while ( ( read = stream.ReadByte()) != -1 )
                    {
                        vt100.Input ( new byte[] { (byte) read } );
                    }
                }
                catch ( EndOfStreamException )
                {
                }
            }
            //System.Console.Write ( screen.ToString() );
            Bitmap bitmap = screen.ToBitmap ( new Font("Courier New", 6) );
            bitmap.Save ( _filename + "_" + _encoding.EncodingName + ".png", System.Drawing.Imaging.ImageFormat.Png );

            /*
              foreach ( Screen.Character ch in screen )
              {
              if ( ch.Char != 0x20 )
              {
              System.Console.WriteLine ( "Non-space character: {0} 0x{1:X4}", ch.Char, (int) ch.Char );
              }
              }
            */
            return screen;
        }
    }
}

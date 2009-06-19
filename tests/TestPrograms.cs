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
    public class TestPrograms
    {
        [Test]
        public void TestProgram ()
        {
            
        }
        
        public void ReadAndRenderFile ( string _filename )
        {
            IVT100Decoder vt100 = new VT100Decoder();
            //vt100.Encoding = encodingInfo.GetEncoding (); // encodingInfo.Name, new EncoderExceptionFallback(), new DecoderReplacementFallback ("U") );
            Screen screen = new Screen ( 80, 160 );
            vt100.Subscribe ( screen );
            
            using ( BinaryReader reader = new BinaryReader(File.Open(_filename, FileMode.Open)) )
            {
                try
                {
                    int read = 0;
                    while ( (read = reader.Read()) != -1 )
                    {
                        vt100.Input ( new byte[] { (byte) read } );
                    }
                }
                catch ( EndOfStreamException )
                {
                }
            }
            System.Console.Write ( screen.ToString() );
            Bitmap bitmap = screen.ToBitmap ( new Font("Courier New", 10) );
            bitmap.Save ( _filename + ".png", System.Drawing.Imaging.ImageFormat.Png );
        }
    }
}

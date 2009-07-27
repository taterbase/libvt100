using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using libVT100;

namespace vt100render
{
   class Program
   {
      static int Main( string[] args )
      {
         string inputFilename = null;
         string outputFilename = null;
         int width = 80;
         int height = 25;
         string fontName = "Courier New";
         int fontSize = 6;
         string encoding = "ibm437";
         for ( int i = 0 ; i < args.Length ; i++ )
         {
            switch ( args[i] )
            {
               case "-w":
                  i++;
                  width = Int32.Parse( args[i] );
                  break;

               case "-h":
                  i++;
                  height = Int32.Parse( args[i] );
                  break;

               case "-f":
                  i++;
                  fontName = args[i];
                  break;

               case "-s":
                  i++;
                  fontSize = Int32.Parse( args[i] );
                  break;
                  
               case "-e":
                  i++;
                  encoding = args[i];
                  break;
                  
               default:
                  if ( inputFilename == null )
                  {
                     inputFilename = args[i];
                  }
                  else if ( outputFilename == null )
                  {
                     outputFilename = args[i];
                  }
                  else
                  {
                     System.Console.WriteLine( "Unrecognized command line parameter ({0}): {1}", i, args[i] );
                  }
                  break;
            }
         }

         if ( inputFilename == null || outputFilename == null )
         {
            System.Console.WriteLine( "Syntax:" );
            System.Console.WriteLine( "    {0} [-w width] [-h height] [-f fontName] [-s fontSize] [-e encoding] <input.txt> <output.png>", System.Reflection.Assembly.GetCallingAssembly().GetName().Name );
            return -1;
         }
         
         IAnsiDecoder vt100 = new AnsiDecoder();
         Screen screen = new Screen(width, height );
         vt100.Encoding = Encoding.GetEncoding( encoding );
         vt100.Subscribe( screen );

         using ( Stream stream = File.Open( inputFilename, FileMode.Open ) )
         {
            int read = 0;
            while ( (read = stream.ReadByte()) != -1 )
            {
               vt100.Input( new byte[] { (byte) read } );
            }
         }

         Bitmap bitmap = screen.ToBitmap( new Font( fontName, fontSize ) );
         bitmap.Save( outputFilename, System.Drawing.Imaging.ImageFormat.Png );
         return 0;
      }
   }
}

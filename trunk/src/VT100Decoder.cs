using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace libVT100
{
   public class VT100Decoder : AnsiDecoder, IVT100Decoder
   {
      protected List<IVT100DecoderClient> m_vt100Listeners;
      
      public VT100Decoder()
         : base()
      {
         m_vt100Listeners = new List<IVT100DecoderClient>();
      }

      protected override bool IsValidParameterCharacter( char _c )
      {
         return _c == '=' || _c == ' ' || base.IsValidParameterCharacter( _c );
      }

      protected override void ProcessCommand( byte _command, string _parameter )
      {
         switch ( (char) _command )
         {
         case 'c':
            string deviceCode = OnGetDeviceCode ();
            break;
            
         case 'n':
            if ( _parameter == "5" )
            {
               DeviceStatus status = OnGetDeviceStatus ();
               string stringStatus = ((int) status).ToString();
               byte[] output = new byte[2 + stringStatus.Length + 1];
               int i = 0;
               output[i++] = EscapeCharacter;
               output[i++] = LeftBracketCharacter;
               foreach ( char c in stringStatus )
               {
                  output[i++] = (byte) c;
               }
               output[i++] = (byte) 'n';
               OnOutput( output );
            }
            else
            {
               base.ProcessCommand( _command, _parameter );
            }
            break;

         case '(':
            // Set normal font
            break;

         case ')':
            // Set alternative font

         case 'r':
            if ( _parameter == "" )
            {
               // Set scroll region to entire screen
            }
            else
            {
               // Set scroll region, separated by ;
            }
            break;
               
         default:
            base.ProcessCommand( _command, _parameter );
            break;
         }
      }
      
      virtual protected string OnGetDeviceCode ()
      {
         foreach ( IVT100DecoderClient client in m_vt100Listeners )
         {
            string deviceCode = client.GetDeviceCode( this );
            if ( deviceCode != null )
            {
               return deviceCode;
            }
         }
         return "UNKNOWN";
      }
      
      virtual protected DeviceStatus OnGetDeviceStatus ()
      {
         foreach ( IVT100DecoderClient client in m_vt100Listeners )
         {
            DeviceStatus status = client.GetDeviceStatus( this );
            if ( status != DeviceStatus.Unknown )
            {
               return status;
            }
         }
         return DeviceStatus.Failure;
      }
      
      void IVT100Decoder.Subscribe( IVT100DecoderClient _client )
      {
         m_listeners.Add ( _client );
         m_vt100Listeners.Add( _client );
      }

      void IVT100Decoder.UnSubscribe( IVT100DecoderClient _client )
      {
         m_vt100Listeners.Remove( _client );
         m_listeners.Remove ( _client );
      }
   }
}

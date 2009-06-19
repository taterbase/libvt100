using System;
using System.Drawing;

namespace libVT100
{
    public class VT100DecoderClient : IVT100DecoderClient
    {
        public delegate Point GetCursorPositionDelegate ( VT100DecoderClient _client );
        public event GetCursorPositionDelegate GetCursorPosition;
        
        public delegate Size GetSizeDelegate ( VT100DecoderClient _client );
        public event GetSizeDelegate GetSize;
        
        public delegate void CharactersDelegate ( VT100DecoderClient _client, char[] _chars );
        public event CharactersDelegate Characters;
        
        void IVT100DecoderClient.Characters ( IVT100Decoder _sender, char[] _chars )
        {
            if ( Characters != null )
            {
                Characters ( this, _chars );
            }
        }
        
        void IVT100DecoderClient.SaveCursor ( IVT100Decoder _sernder )
        {
        }
        
        void IVT100DecoderClient.RestoreCursor ( IVT100Decoder _sender )
        {
        }
        
        Size IVT100DecoderClient.GetSize ( IVT100Decoder _sender )
        {
            if ( GetSize != null )
            {
                return GetSize(this);
            }
            return Size.Empty;
        }
        
        void IVT100DecoderClient.MoveCursor ( IVT100Decoder _sender, Direction _direction, int _amount )
        {
        }
        
        void IVT100DecoderClient.MoveCursorToBeginningOfLineBelow ( IVT100Decoder _sender, int _lineNumberRelativeToCurrentLine )
        {
        }
        
        void IVT100DecoderClient.MoveCursorToBeginningOfLineAbove ( IVT100Decoder _sender, int _lineNumberRelativeToCurrentLine )
        {
        }
        
        void IVT100DecoderClient.MoveCursorToColumn ( IVT100Decoder _sender, int _columnNumber )
        {
        }
        
        void IVT100DecoderClient.MoveCursorTo ( IVT100Decoder _sender, int _row, int _column )
        {
        }
        
        void IVT100DecoderClient.ClearScreen ( IVT100Decoder _sender, ClearDirection _direction )
        {
        }
        
        void IVT100DecoderClient.ClearLine ( IVT100Decoder _sender, ClearDirection _direction )
        {
        }
        
        void IVT100DecoderClient.ScrollPageUpwards ( IVT100Decoder _sender, int _linesToScroll )
        {
        }
        
        void IVT100DecoderClient.ScrollPageDownwards ( IVT100Decoder _sender, int _linesToScroll )
        {
        }
        
        void IVT100DecoderClient.HideCursor ( IVT100Decoder _sender )
        {
        }
        
        void IVT100DecoderClient.ShowCursor ( IVT100Decoder _sender )
        {
        }
        
        Point IVT100DecoderClient.GetCursorPosition ( IVT100Decoder _sender )
        {
            if ( GetCursorPosition != null )
            {
                return GetCursorPosition(this);
            }
            return Point.Empty;
        }

        void IDisposable.Dispose ()
        {
            Characters = null;
            GetCursorPosition = null;
            GetSize = null;
        }
    }
}

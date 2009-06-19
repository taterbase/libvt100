using System;
using System.Drawing;

namespace libVT100
{
    public class VT100Client : IVT100Client
    {
        public delegate Point GetCursorPositionDelegate ( VT100Client _client );
        public event GetCursorPositionDelegate GetCursorPosition;
        
        public delegate Size GetSizeDelegate ( VT100Client _client );
        public event GetSizeDelegate GetSize;
        
        public delegate void CharactersDelegate ( VT100Client _client, char[] _chars );
        public event CharactersDelegate Characters;
        
        void IVT100Client.Characters ( IVT100 _sender, char[] _chars )
        {
            if ( Characters != null )
            {
                Characters ( this, _chars );
            }
        }
        
        void IVT100Client.SaveCursor ( IVT100 _sernder )
        {
        }
        
        void IVT100Client.RestoreCursor ( IVT100 _sender )
        {
        }
        
        Size IVT100Client.GetSize ( IVT100 _sender )
        {
            if ( GetSize != null )
            {
                return GetSize(this);
            }
            return Size.Empty;
        }
        
        void IVT100Client.MoveCursor ( IVT100 _sender, Direction _direction, int _amount )
        {
        }
        
        void IVT100Client.MoveCursorToBeginningOfLineBelow ( IVT100 _sender, int _lineNumberRelativeToCurrentLine )
        {
        }
        
        void IVT100Client.MoveCursorToBeginningOfLineAbove ( IVT100 _sender, int _lineNumberRelativeToCurrentLine )
        {
        }
        
        void IVT100Client.MoveCursorToColumn ( IVT100 _sender, int _columnNumber )
        {
        }
        
        void IVT100Client.MoveCursorTo ( IVT100 _sender, int _row, int _column )
        {
        }
        
        void IVT100Client.ClearScreen ( IVT100 _sender, ClearDirection _direction )
        {
        }
        
        void IVT100Client.ClearLine ( IVT100 _sender, ClearDirection _direction )
        {
        }
        
        void IVT100Client.ScrollPageUpwards ( IVT100 _sender, int _linesToScroll )
        {
        }
        
        void IVT100Client.ScrollPageDownwards ( IVT100 _sender, int _linesToScroll )
        {
        }
        
        void IVT100Client.HideCursor ( IVT100 _sender )
        {
        }
        
        void IVT100Client.ShowCursor ( IVT100 _sender )
        {
        }
        
        Point IVT100Client.GetCursorPosition ( IVT100 _sender )
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

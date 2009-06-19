using System;
using System.Drawing;

namespace libVT100
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public enum ClearDirection
    {
        Backwards,
        Forwards,
        Both
    }
    
    public interface IVT100DecoderClient : IDisposable
    {
        void Characters ( IVT100Decoder _sender, char[] _chars );
        void SaveCursor ( IVT100Decoder _sernder );
        void RestoreCursor ( IVT100Decoder _sender );
        Size GetSize ( IVT100Decoder _sender );
        void MoveCursor ( IVT100Decoder _sender, Direction _direction, int _amount );
        void MoveCursorToBeginningOfLineBelow ( IVT100Decoder _sender, int _lineNumberRelativeToCurrentLine );
        void MoveCursorToBeginningOfLineAbove ( IVT100Decoder _sender, int _lineNumberRelativeToCurrentLine );
        void MoveCursorToColumn ( IVT100Decoder _sender, int _columnNumber );
        void MoveCursorTo ( IVT100Decoder _sender, int _row, int _column );
        void ClearScreen ( IVT100Decoder _sender, ClearDirection _direction );
        void ClearLine ( IVT100Decoder _sender, ClearDirection _direction );
        void ScrollPageUpwards ( IVT100Decoder _sender, int _linesToScroll );
        void ScrollPageDownwards ( IVT100Decoder _sender, int _linesToScroll );
        void HideCursor ( IVT100Decoder _sender );
        void ShowCursor ( IVT100Decoder _sender );
        Point GetCursorPosition ( IVT100Decoder _sender );
    }
}

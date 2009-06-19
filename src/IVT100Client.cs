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
    
    public interface IVT100Client : IDisposable
    {
        void Characters ( IVT100 _sender, char[] _chars );
        void SaveCursor ( IVT100 _sernder );
        void RestoreCursor ( IVT100 _sender );
        Size GetSize ( IVT100 _sender );
        void MoveCursor ( IVT100 _sender, Direction _direction, int _amount );
        void MoveCursorToBeginningOfLineBelow ( IVT100 _sender, int _lineNumberRelativeToCurrentLine );
        void MoveCursorToBeginningOfLineAbove ( IVT100 _sender, int _lineNumberRelativeToCurrentLine );
        void MoveCursorToColumn ( IVT100 _sender, int _columnNumber );
        void MoveCursorTo ( IVT100 _sender, int _row, int _column );
        void ClearScreen ( IVT100 _sender, ClearDirection _direction );
        void ClearLine ( IVT100 _sender, ClearDirection _direction );
        void ScrollPageUpwards ( IVT100 _sender, int _linesToScroll );
        void ScrollPageDownwards ( IVT100 _sender, int _linesToScroll );
        void HideCursor ( IVT100 _sender );
        void ShowCursor ( IVT100 _sender );
        Point GetCursorPosition ( IVT100 _sender );
    }
}

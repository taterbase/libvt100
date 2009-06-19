using System;
using System.Drawing;

namespace libVT100
{
    public enum Direction
    {
        Up,
        Down,
        Forward,
        Backward
    }
    
    public enum ClearDirection
    {
        Forward = 0,
        Backward = 1,
        Both = 2
    }
    
    public enum GraphicRendition
    {
        /// all attributes off
        Reset = 0,
        /// Intensity: Bold
        Bold = 1,
        /// Intensity: Faint     not widely supported
        Faint = 2,
        /// Italic: on     not widely supported. Sometimes treated as inverse.
        Italic = 3,
        /// Underline: Single     not widely supported
        Underline = 4,
        /// Blink: Slow     less than 150 per minute
        BlinkSlow = 5,
        /// Blink: Rapid     MS-DOS ANSI.SYS; 150 per minute or more
        BlinkRapid = 6,
        /// Image: Negative     inverse or reverse; swap foreground and background
        Inverse = 7,
        /// Conceal     not widely supported
        Conceal = 8,
        /// Font selection (not sure which)
        Font1 = 10,
        /// Underline: Double
        UnderlineDouble = 21,
        /// Intensity: Normal     not bold and not faint
        NormalIntensity = 22,
        /// Underline: None     
        NoUnderline = 24,
        /// Blink: off     
        NoBlink = 25,
        /// Image: Positive
        ///
        /// Not sure what this is supposed to be, the opposite of inverse???
        Positive = 27,
        /// Reveal,     conceal off
        Reveal = 28,
        /// Set foreground color, normal intensity
        ForegroundNormalBlack = 30,
        ForegroundNormalRed = 31,
        ForegroundNormalGreen = 32,
        ForegroundNormalYellow = 33,
        ForegroundNormalBlue = 34,
        ForegroundNormalMagenta = 35,
        ForegroundNormalCyan = 36,
        ForegroundNormalWhite = 37,
        ForegroundNormalReset = 39,
        /// Set background color, normal intensity
        BackgroundNormalBlack = 40,
        BackgroundNormalRed = 41,
        BackgroundNormalGreen = 42,
        BackgroundNormalYellow = 43,
        BackgroundNormalBlue = 44,
        BackgroundNormalMagenta = 45,
        BackgroundNormalCyan = 46,
        BackgroundNormalWhite = 47,
        BackgroundNormalReset = 49,
        /// Set foreground color, high intensity (aixtem)
        ForegroundBrightBlack = 90,
        ForegroundBrightRed = 91,
        ForegroundBrightGreen = 92,
        ForegroundBrightYellow = 93,
        ForegroundBrightBlue = 94,
        ForegroundBrightMagenta = 95,
        ForegroundBrightCyan = 96,
        ForegroundBrightWhite = 97,
        ForegroundBrightReset = 99,
        /// Set background color, high intensity (aixterm)
        BackgroundBrightBlack = 100,
        BackgroundBrightRed = 101,
        BackgroundBrightGreen = 102,
        BackgroundBrightYellow = 103,
        BackgroundBrightBlue = 104,
        BackgroundBrightMagenta = 105,
        BackgroundBrightCyan = 106,
        BackgroundBrightWhite = 107,
        BackgroundBrightReset = 109,
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
        void MoveCursorTo ( IVT100Decoder _sender, Point _position );
        void ClearScreen ( IVT100Decoder _sender, ClearDirection _direction );
        void ClearLine ( IVT100Decoder _sender, ClearDirection _direction );
        void ScrollPageUpwards ( IVT100Decoder _sender, int _linesToScroll );
        void ScrollPageDownwards ( IVT100Decoder _sender, int _linesToScroll );
        void HideCursor ( IVT100Decoder _sender );
        void ShowCursor ( IVT100Decoder _sender );
        Point GetCursorPosition ( IVT100Decoder _sender );
        void SetGraphicRendition ( IVT100Decoder _sender, GraphicRendition[] _commands );
    }
}

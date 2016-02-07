When writing anything that needs to communicate with a terminal in some way it is almost always speaking some dialect of VT100 or ANSI.

This library aims solely at parsing a stream of VT100/ANSI data and then letting the host application do the rendering. Many other project also parse VT100/ANSI data but their parser is always tangled up with the actual rendering of the data, making reuse in other projects problematic.

Hopefully other projects will start using libvt100 and we can stop reinventing the wheel in each project.

[Screenshots](Screenshots.md)
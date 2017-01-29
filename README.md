# RomItemDataEditor

Get data from the itemdata struct in various gen 3 Pokemon game.

How to use it:

romitemdataeditor 0.0.2
Copyright (C) 2017 Aukie's Homebrew
Usage: romitemdataeditor --rom-file <*.gba file> [--data-file <*.xml file>]
[--index-number <index number>] [--data-name <datamember name>] [--print-hex]

  -f, --rom-file        Required. Path of the GBA ROM.

  -x, --xml-file        (Default: data.xml) Path of the XML file.

  -i, --index-number    Required. (Default: 0) Index of the item.

  -d, --data-name       Struct name to get value from.

  --print-hex           Print hexidecimal value.

  -n, --get-name        Get item name.

  --help                Display this help screen.

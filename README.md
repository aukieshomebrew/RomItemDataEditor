# RomItemDataEditor

Get data from the itemdata struct in various gen 3 Pokemon game.

How to use it:

romitemdataeditor 0.2
Copyright (C) 2017 Aukie's Homebrew
Usage: romitemdataeditor --rom-file <*.gba file> [--data-file <*.xml file>]
[--index-number <index number>] [--get-value <datamember name>] [--get-name]
[--set-name <new name>] [--set-value <value-name> <integer>] [--print-hex]

  -f, --rom-file        Required. Path of the GBA ROM.

  -x, --xml-file        (Default: data.xml) Path of the XML file.

  -i, --index-number    Required. Index of the item.

  --print-hex           Print hexidecimal value.

  -n, --get-name        Get item name.

  -v, --get-value       Struct name to get value from.

  --set-name            Set item name.

  --set-value           Struct name to set value to.

  --set-value-int       New value for the --set-value option

  --help                Display this help screen.
  
  How to compile:
  
    1.  Clone this project in a directory.
    2.  Use: 'nuget restore' to download the dependencies.
    3.  Use: 'msbuild RomItemDataEditor.sln' to build the project.

Note:
In the project directory is a sample 'data.xml' file.

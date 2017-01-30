# RomItemDataEditor

Get data from the itemdata struct in various 3rd generation Pokemon games.

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
  
  
  Structure names and description:
  name          : Name of the item. 
  index         : Index number of the item.
  price         : Price of the item.
  hold_effect   : Hold effect.
  param         : Parameter.
  desc_pointer  : Description pointer.
  mystery_value : Mystery value.
  pocket        : Pocket
  type          : Type
  pointer_fuc   : Pointer to field usage code.
  battle_usage  : Battle Usage
  pointer_buc   : Pointer to battle usage code.
  extra_param   : Extra parameter.
  
 More information: http://bulbapedia.bulbagarden.net/wiki/Item_data_structure_in_Generation_III 
  
  How to compile:
  
    1.  Clone this project in a directory.
    2.  Use: 'nuget restore' to download the dependencies.
    3.  Use: 'msbuild RomItemDataEditor.sln' to build the project.

Note:
In the project directory is a sample 'data.xml' file.

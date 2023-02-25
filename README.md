# Linxplorer

This is CLI app for cmd. There may be errors in other terminals.

Finds all links in a text file(s) and outputs a tree to work with. 

Can output(in file or terminal) selected branches, sort, find and all that

Makes it beautiful. Works fine.
________________________________________________________________________________________________________________________________
```
Usage: Linxplorer.exe [OPTION]... [FILE]...

Options:
-a,   --all           Expand all nodes of tree.

-h,   --headers-off   Does not print additional information, such as tooltips.

-l,   --links         Print only found links.

-f,   --files         Print only found links on files. 
                      For example, with the ending .html, .png, .doc...

-s,   --sort          Sorts alphabetically.

-m,   --marge         Leaves only unique links merging into one file.

-b,   --sort-count    Sort by count.

-c,   --count-off     Do not print count in tree.

      --ends-with=    Print links with specified endings
                      In double quotes separated by a space
                      If this is used, then "-f/--files" is ignored.
                      
      --find=         Looks for regex matches in each link and displays only
                      those in which it was found.
                      You can use "^" "&" for full match.
 
      
      --help       Display help and exit
      --version    Output version information and exit
```
![](https://media.discordapp.net/attachments/969942906628608070/1079166211063033916/cmd.png?width=682&height=676)

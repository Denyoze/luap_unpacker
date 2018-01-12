# luap_unpacker
.luap file format (The Saboteur) unpacker.  
By Denyoze  
[Decompiler](https://sourceforge.net/projects/unluac/) by tehtmi  

----------

This is one night project developed for The Saboteur game file format **.luap** and help to unpack it.  

## **How to use:**

luap_unpacker.exe -f <file_name> [-info] [-d]
> -f \<file name\>- *file to unpack*  
> -info - *show more info in console*  
> -d - *decompile files when unpack*  
> -pleaseno - *skip decompiled file, if it exist*

For use decompiling feature, put unluac.jar to "./jar" folder

## **Folders**

 - ./compiled - for .luac files   
 -  ./decompiled - result of unluac -  working   
 -  ./jar - "special" folder for unluac.jar

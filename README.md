## Jollypop Injector - A WPF DLL Injector supporting 32/64 bit unmanaged DLL injection, and 64 bit managed(Unity) DLL injection.
This originally started as a GUI frontend for [MonoJabber](https://github.com/AWilliams17/MemTools) but I decided to go ahead and include
unmanaged 32 and 64 bit injection as well.  
  
The way I support both 32 bit and 64 bit DLL injection is incredibly primitive. I just distribute 32 and 64 bit binaries of  
a hastily whipped together CLI DLL Injector, and then I have the WPF app call them and grab all the output.  
  
The same goes for MonoJabber.  
  
This tool also uses [my Windows registry manager library](https://github.com/AWilliams17/Registrar), so all settings are  
preserved through each application session.  
  
It also uses [my memory hacking library](https://github.com/AWilliams17/MemTools), [as well as my C# utility library](https://github.com/AWilliams17/SharpUtils).
  
I didn't really intend for this to be publicly released, so there's some code quality issues, and there's some end user problems...  
Mainly being that I didn't write an updater for this (since again, personal usage), so unless you manually check and see if any of those libraries  
have updated, you'll probably end up using the outdated ones.  
When I make a new release of this project, I'll include the currently released versions though. That's **if** I make another release,  
since I kind of was extremely bored making this, and it already does its job.  
  
## Preview
![alt text](https://i.imgur.com/2xhn3aD.png)
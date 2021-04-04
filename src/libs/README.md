# Required libs:
These are the external libraries that are required for this project.  
Make sure these files are copied to the output directory of the project, you can use a command like this `xcopy /Y "$(ProjectDir)libs\*.dll" "$(SolutionDir)\$(OutDir)"` in the build events to copy the files.  
For the NDI specific files, if you have the SDK installed they should be found here: `C:\Program Files\NewTek\NDI 4 SDK`.
- NDILibDotNet2.dll
- Processing.NDI.Lib.x86.dll
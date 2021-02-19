var MyPlugin = {
      IsMobile() : Function()
     {
         Return UnityLoader.SystemInfo.mobile;
     }
 };
 
 mergeInto(LibraryManager.library, MyPlugin);
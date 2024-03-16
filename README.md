# autojump for bash inspired
cli tool to jump to different locations

## idea
Jump directly to paths / directories you visit often without typing the full path.

ps > one 
ps > cwd -> C:\OneDrive\

ps > books csharp
ps > cwd -> C:\\books\programming\csharp

### how it works
TODO

## Commands
the following are all currently available commands:
```csharp
public static class Names
{
    /// <summary>
    /// change to the directory (or the segment that is matched)
    /// </summary>
    public const string CD = "cd";
    /// <summary>
    /// cupgrade current working directory (or insert into the store in the first place)
    /// </summary>
    public const string Bump = "bump";

    /// <summary>
    /// create the initial store
    /// </summary>
    public const string Init = "init";
    /// <summary>
    /// list all values with counter and last access
    /// </summary>
    public const string List = "list";
    /// <summary>
    /// run a query on the data store:O
    /// </summary>
    public const string Exec = "exec";
}
```

## Architecture
I tried to go for the most simple design I could come up with that still does the job and can be tested sufficiently.


## Notes
Most software nowadays is either totally underengineered or totally overengineered (at least in my personal experience)
So with this tool I tried to find a perfect sweet spot in between.

I use Documentation and comments where I see them helping to clarify WHY the code exists and WHAT it is supposed to do. Where the Code should explain the HOW.

Tests and a "local" build pipeline exist in v1.
In form of a __.\build.ps1__, __publish.ps1__ and the __create-db.ps1__.

There is also an __Install.ps1__ which will add to your powershell profile the following functions:
```Powershell
function cwd
{ get-location | select -expandProp Path | clip.exe }

# improved cd command with the tunnel
function j([string] $path) {
    $result = autojumpsharp cd $path
    if ([string]::isnullorwhitespace($result)) {
        write-host "$path not found in the store"
    } else {
        cd $result
    }
}

function jbmp() {
   $path = cwd
   autojumpsharp bump $path
}
```

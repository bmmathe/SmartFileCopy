// See https://aka.ms/new-console-template for more information
using SmartFileCopy;

Console.WriteLine("Loading config...");

var config = SmartFileCopyConfig.Load();
Console.WriteLine($"Source folder: {config.SourceFolder}");
Console.WriteLine($"Destination folder: {config.DestinationFolder}");
Console.WriteLine($"Interval in seconds: {config.IntervalInSeconds}");


var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(config.IntervalInSeconds));
while (await periodicTimer.WaitForNextTickAsync())
{
    Console.WriteLine("Sync started.");
    var lastRun = LastRunManager.GetLastRun();
    Console.WriteLine($"Last run: {lastRun}");

    FileInfo[] newFiles = new DirectoryInfo(config.SourceFolder)
                         .EnumerateFiles()
                         .Select(x => {
                             x.Refresh();
                             return x;
                         })
                         .Where(x => x.LastWriteTime >= lastRun)
                         .ToArray()
                         ;
    if (newFiles.Length > 0)
    {
        Console.WriteLine("Syning the following files...");
        foreach (var file in newFiles)
        {
            Console.WriteLine($"{file.Name}");
        }
    }

    foreach (var file in newFiles)
    {
        try
        {
            Console.WriteLine($"Copying {file.Name}...");
            var destination = Path.Combine(config.DestinationFolder, file.Name);
            File.Copy(file.FullName, destination, false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    LastRunManager.Update();

    // post copy
    if(!string.IsNullOrEmpty(config.AfterCopyPostUrl) && !string.IsNullOrEmpty(config.AfterCopyPostBody))
    {
        Console.WriteLine("Running post copy step.");
        using (var http = new HttpClient())
        {
            var res = await http.PostAsync(config.AfterCopyPostUrl, new StringContent(config.AfterCopyPostBody, System.Text.Encoding.UTF8, "application/json"));            
            if(res.IsSuccessStatusCode)
            {
                Console.WriteLine("Post copy step success.");
            } else
            {
                Console.WriteLine($"Post copy step failed: {res.StatusCode}");
            }
        }
    }

    Console.WriteLine("Sync finished.");
}



Console.ReadLine();



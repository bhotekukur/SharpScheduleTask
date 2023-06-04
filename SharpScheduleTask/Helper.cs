using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SharpScheduleTask
{
    public class Helper
    {
        public static Dictionary<string, string> ParseArgs(string[] args) {
            try
            {
                Dictionary<string, string> ret = new Dictionary<string, string>();
                for (int i = 0; i < args.Length; i++)
                    ret.Add(args[i].Split(':')[0].Remove(0, 1).ToLower(), args[i].Split(new[] { ':' }, 2)[1]);
                return ret;
            }
            catch (Exception)
            {
                Debug.WriteLine("[X] Your command is wrong. Please check help page.");
                return null;
            }
        }

        public static void PrintHelp() {
            Debug.WriteLine(@"Methods (/method):");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"    create        - Create a new scheduled task");
            Debug.WriteLine(@"    delete        - Delete an existing scheduled task");
            Debug.WriteLine(@"    run           - Execute an existing scheduled task");
            Debug.WriteLine(@"    query         - Query details for a scheduled task or all scheduled tasks under a folder");
            Debug.WriteLine(@"    queryfolders  - Query all sub-folders in scheduled task recursively");
            Debug.WriteLine(@"    move          - Perform lateral movement using scheduled task (automatically create, run and delete)");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"[*] are mandatory fields.");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Options for scheduled task creation (/method:create):");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"    [*] /taskname     - Specify the name of the scheduled task");
            Debug.WriteLine(@"    [*] /program      - Specify the program that the task runs");
            Debug.WriteLine(@"    [*] /trigger      - Specify the schedule type. The valid values include: ""minute"", ""hourly"", ""daily"", ""weekly"", ""onstart"", ""onlogon"", and ""onidle""");
            Debug.WriteLine(@"    /modifier         - Specify how often the task runs within its schedule type. Applicable only for schedule type such as ""minute"" (e.g., 1-1439 minutes), ""hourly"" (e.g., 1-23 hours) and ""weekly"" (e.g., mon,sat,sun)");
            Debug.WriteLine(@"    /starttime        - Specify the start time for daily schedule type (e.g., 23:30)");
            Debug.WriteLine(@"    /argument         - Specify the command line argument for the program");
            Debug.WriteLine(@"    /folder           - Specify the folder where the scheduled task stores (default: \)");
            Debug.WriteLine(@"    /author           - Specify the author of the scheduled task");
            Debug.WriteLine(@"    /description      - Specify the description for the scheduled task");
            Debug.WriteLine(@"    /remoteserver     - Specify the hostname or IP address of a remote computer");
            Debug.WriteLine(@"    /user             - Run the task with a specified user account");
            Debug.WriteLine(@"    /technique        - Specify evasion technique:");
            Debug.WriteLine(@"                        ""hide"": A technique used by HAFNIUM malware that will hide the scheduled task from ""/method:query"", ""schtasks /query"", and Task Scheduler");
            Debug.WriteLine(@"                        (This technique does not support remote execution due to privilege of remote registry. It requires ""NT AUTHORITY\SYSTEM"" and the task will continue to run until system reboot even after task deletion)");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Options for scheduled task deletion (/method:delete):");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"    [*] /taskname     - Specify the name of the scheduled task");
            Debug.WriteLine(@"    /folder           - Specify the folder where the scheduled task stores");
            Debug.WriteLine(@"    /remoteserver     - Specify the hostname or IP address of a remote computer");
            Debug.WriteLine(@"    /technique        - Specify when the scheduled task was created using evasion technique:");
            Debug.WriteLine(@"                        ""hide"": Delete scheduled task that used ""hiding scheduled task"" technique");
            Debug.WriteLine(@"                        (The deletion requires ""NT AUTHORITY\SYSTEM"" and the task will continue to run until system reboot even after task deletion)");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Options for scheduled task execution (/method:run):");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"    [*] /taskname     - Specify the name of the scheduled task");
            Debug.WriteLine(@"    /folder           - Specify the folder where the scheduled task stores");
            Debug.WriteLine(@"    /remoteserver     - Specify the hostname or IP address of a remote computer");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Options for scheduled task query (/method:query):");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"    /taskname         - Specify the name of the scheduled task");
            Debug.WriteLine(@"    /folder           - Specify the folder where the scheduled task stores");
            Debug.WriteLine(@"    /remoteserver     - Specify the hostname or IP address of a remote computer");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Options for scheduled task lateral movement (/method:move):");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"    [*] /taskname     - Specify the name of the scheduled task");
            Debug.WriteLine(@"    [*] /program      - Specify the program that the task runs");
            Debug.WriteLine(@"    [*] /remoteserver - Specify the hostname or IP address of a remote computer");
            Debug.WriteLine(@"    /trigger          - Specify the schedule type. The valid values include: ""minute"", ""hourly"", ""daily"", ""weekly"", ""onstart"", ""onlogon"", and ""onidle""");
            Debug.WriteLine(@"    /modifier         - Specify how often the task runs within its schedule type. Applicable only for schedule type such as ""minute"" (e.g., 1-1439 minutes), ""hourly"" (e.g., 1-23 hours) and ""weekly"" (e.g., mon,sat,sun)");
            Debug.WriteLine(@"    /starttime        - Specify the start time for daily schedule type (e.g., 23:30)");
            Debug.WriteLine(@"    /argument         - Specify the command line argument for the program");
            Debug.WriteLine(@"    /folder           - Specify the folder where the scheduled task stores (default: \)");
            Debug.WriteLine(@"    /author           - Specify the author of the scheduled task");
            Debug.WriteLine(@"    /description      - Specify the description for the scheduled task");
            Debug.WriteLine(@"    /user             - Run the task with a specified user account");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Example:");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Create a scheduled task called ""Cleanup"" that will be executed every day at 11:30 p.m.:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:create /taskname:Cleanup /trigger:daily /starttime:23:30 /program:calc.exe /description:""Some description"" /author:netero1010");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Create a scheduled task called ""Cleanup"" that will be executed every 4 hours on a remote server:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:create /taskname:Cleanup /trigger:hourly /modifier:4 /program:rundll32.exe /argument:c:\temp\payload.dll /remoteserver:TARGET-PC01");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Delete a scheduled task called ""Cleanup"":");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:delete /taskname:Cleanup");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Execute a scheduled task called ""Cleanup"":");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:run /taskname:Cleanup");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Query details for a scheduled task called ""Cleanup"" under ""\Microsoft\Windows\CertificateServicesClient"" folder on a remote server:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:query /taskname:Cleanup /folder:\Microsoft\Windows\CertificateServicesClient /remoteserver:TARGET-PC01");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Query all scheduled tasks under a specific folder ""\Microsoft\Windows\CertificateServicesClient"" on a remote server:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:query /folder:\Microsoft\Windows\CertificateServicesClient /remoteserver:TARGET-PC01");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Query all sub-folders in scheduled task:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:queryfolders");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Perform lateral movement using scheduled task to a remote server using a specific user account:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:move /taskname:Demo /remoteserver:TARGET-PC01 /program:rundll32.exe /argument:c:\temp\payload.dll /user:netero1010");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Technique - hide:");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Create a scheduled task called ""Cleanup"" using hiding scheduled task technique:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:create /taskname:Cleanup /trigger:daily /starttime:23:30 /program:calc.exe /description:""Some description"" /author:netero1010 /technique:hide");
            Debug.WriteLine(@"");
            Debug.WriteLine(@"Delete a scheduled task called ""Cleanup"" that used hiding scheduled task technique:");
            Debug.WriteLine(@"    SharpScheduleTask.exe /method:delete /taskname:Cleanup /technique:hide");
        }
    }
}

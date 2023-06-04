# SharpScheduleTask - A C# tool with more flexibility to customize scheduled task for both persistence and lateral movement in red team operation
----
Scheduled task is one of the most popular attack technique in the past decade and now it is still commonly used by red teamers for persistence and lateral movement. 

A number of C# tools were already developed to simulate the attack using scheduled task. I have been playing around with some of them but each of them has its own limitations on customizing the scheduled task. Therefore, this project aims to provide a C# tool (CobaltStrike execute-assembly friendly) to include the features that I need and provide enough flexibility on customizing the scheduled task.

## Methods (/method):
|  Method | Function  |
| ------------ | ------------ |
| create | Create a new scheduled task |
| delete | Delete an existing scheduled task |
| run | Execute an existing scheduled task |
| query | Query details for a scheduled task or all scheduled tasks under a folder |
| queryfolders | Query all sub-folders in scheduled task  |
| move | Perform lateral movement using scheduled task (automatically create, run and delete) |

## Options for scheduled task creation (/method:create):
|  Method | Function  |
| ------------ | ------------ |
| [*] /taskname | Specify the name of the scheduled task |
| [*] /program | Specify the program that the task runs |
| [*] /trigger | Specify the schedule type. The valid values include: "minute", "hourly", "daily", "weekly", "onstart", "onlogon", and "onidle" |
| /modifier | Specify how often the task runs within its schedule type. Applicable only for schedule type such as "minute" (e.g., 1-1439 minutes), "hourly" (e.g., 1-23 hours) and "weekly" (e.g., mon,sat,sun) |
| /starttime | Specify the start time for daily schedule type (e.g., 23:30) |
| /argument | Specify the command line argument for the program |
| /folder | Specify the folder where the scheduled task stores (default: \\) |
| /author | Specify the author of the scheduled task |
| /description | Specify the description for the scheduled task |
| /remoteserver | Specify the hostname or IP address of a remote computer |
| /user | Run the task with a specified user account |
| /technique | Specify evasion technique:<br>- "hide": A technique used by HAFNIUM malware that will hide the scheduled task from task query<br><br>[!] https://www.microsoft.com/security/blog/2022/04/12/tarrask-malware-uses-scheduled-tasks-for-defense-evasion/<br>[!] This technique does not support remote execution due to privilege of remote registry. It requires "NT AUTHORITY\SYSTEM" and the task will continue to run until system reboot even after task deletion |

[*] are mandatory fields.

## Options for scheduled task deletion (/method:delete):
|  Method | Function  |
| ------------ | ------------ |
| [*] /taskname | Specify the name of the scheduled task |
| /folder | Specify the folder where the scheduled task stores (default: \\) |
| /remoteserver | Specify the hostname or IP address of a remote computer |
| /technique | Specify when the scheduled task was created using evasion technique:<br>- "hide": Delete scheduled task that used "hiding scheduled task" technique<br><br>[!] The deletion requires "NT AUTHORITY\SYSTEM" and the task will continue to run until system reboot even after task deletion |

[*] are mandatory fields.

## Options for scheduled task execution (/method:run):
|  Method | Function  |
| ------------ | ------------ |
| [*] /taskname | Specify the name of the scheduled task |
| /folder | Specify the folder where the scheduled task stores (default: \\) |
| /remoteserver | Specify the hostname or IP address of a remote computer |

[*] are mandatory fields.

## Options for scheduled task query (/method:query):
|  Method | Function  |
| ------------ | ------------ |
| /taskname | Specify the name of the scheduled task |
| /folder | Specify the folder where the scheduled task stores (default: \\) |
| /remoteserver | Specify the hostname or IP address of a remote computer |

[*] are mandatory fields.

## Options for scheduled task lateral movement (/method:move):
|  Method | Function  |
| ---------------- | ---------------- |
| [*] /taskname | Specify the name of the scheduled task |
| [*] /program | Specify the program that the task runs |
| [*] /remoteserver | Specify the hostname or IP address of a remote computer |
| /trigger | Specify the schedule type. The valid values include: "minute", "hourly", "daily", "weekly", "onstart", "onlogon", and "onidle" |
| /modifier | Specify how often the task runs within its schedule type. Applicable only for schedule type such as "minute" (e.g., 1-1439 minutes), "hourly" (e.g., 1-23 hours) and "weekly" (e.g., mon,sat,sun) |
| /starttime | Specify the start time for daily schedule type (e.g., 23:30) |
| /argument | Specify the command line argument for the program |
| /folder | Specify the folder where the scheduled task stores (default: \\) |
| /author | Specify the author of the scheduled task |
| /description | Specify the description for the scheduled task |
| /user | Run the task with a specified user account |

[*] are mandatory fields.

## Example
**Create a scheduled task called "Cleanup" that will be executed every day at 11:30 p.m.**

`SharpScheduleTask.exe /method:create /taskname:Cleanup /trigger:daily /starttime:23:30 /program:calc.exe /description:"description" /author:user1`

**Create a scheduled task called "Cleanup" that will be executed every 4 hours on a remote server**

`SharpScheduleTask.exe /method:create /taskname:Cleanup /trigger:hourly /modifier:4 /program:rundll32.exe /argument:c:\temp\payload.dll /remoteserver:TARGET-PC01`

**Delete a scheduled task called "Cleanup"**

`SharpScheduleTask.exe /method:delete /taskname:Cleanup`

**Execute a scheduled task called "Cleanup"**

`SharpScheduleTask.exe /method:run /taskname:Cleanup`

**Query details for a scheduled task called "Cleanup" under "\Microsoft\Windows\CertificateServicesClient" folder on a remote server**

`SharpScheduleTask.exe /method:query /taskname:Cleanup /folder:\Microsoft\Windows\CertificateServicesClient /remoteserver:TARGET-PC01`

**Query all scheduled tasks under a specific folder "\Microsoft\Windows\CertificateServicesClient" on a remote server**

`SharpScheduleTask.exe /method:query /folder:\Microsoft\Windows\CertificateServicesClient /remoteserver:TARGET-PC01`

**Query all sub-folders in scheduled task**

`SharpScheduleTask.exe /method:queryfolders`

**Perform lateral movement using scheduled task to a remote server using a specific user account**

`SharpScheduleTask.exe /method:move /taskname:Demo /remoteserver:TARGET-PC01 /program:rundll32.exe /argument:c:\temp\payload.dll /user:user1`

**Create a scheduled task called "Cleanup" using hiding scheduled task technique:**

`SharpScheduleTask.exe /method:create /taskname:Cleanup /trigger:daily /starttime:23:30 /program:calc.exe /description:"Some description" /author:user1 /technique:hide`

**Delete a scheduled task called "Cleanup" that used hiding scheduled task technique:**

`SharpScheduleTask.exe /method:delete /taskname:Cleanup /technique:hide`

## Hiding Scheduled Task Technique
This technique was used by threat actor - HAFNIUM and discovered by Microsoft recently. It aims to make the scheduled task unqueriable by tools and unseeable by Task Scheduler.

To use this technique, you are required to have "NT AUTHORITY/SYSTEM" and SharpScheduleTask will do the following for you:
1. Delete "SD" value from "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\\[task name]"
2. Delete scheduled task XML file "C:\Windows\System32\Tasks\\[task name]"

To remove scheduled task that is created using this technique require to add "/technique:hide" in the delete method to remove it properly.

### Disadvantage of this technique:
The task will continue to run util next system reboot even if the task is deleted via registry. Therefore, it is better not to use this technique in server for your operation.

## Library and Reference Used:
| Library | Link |
| ------------ | ------------ |
| TaskScheduler | https://github.com/dahall/TaskScheduler |

| Reference | Link |
| ------------ | ------------ |
| SharpPersist | https://github.com/mandiant/SharPersist |
| Hiding scheduled task technique | https://www.microsoft.com/security/blog/2022/04/12/tarrask-malware-uses-scheduled-tasks-for-defense-evasion/ |

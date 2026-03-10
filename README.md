⌨️ KeyMapper & Logger (Educational Project)
This project is a low-level keyboard monitoring (keylogging) application developed using C# and Win32 API. It was created for educational purposes to understand how operating system-level inputs are handled and how raw data can be mapped using JSON formatting.

🚀 Features
Win32 API Integration: Real-time keyboard monitoring using the GetAsyncKeyState function via the user32.dll library.

Dynamic Key Mapping: Converting pressed Virtual-Key Codes into meaningful characters by reading from an external keys.json file.

Local Logging: Saving captured data as a text file in a specific directory (230543014) on the user's desktop.

JSON Support: Efficient data processing using the Newtonsoft.Json library.

🛠️ Technologies Used
Language: C# (.NET)

Libraries: * Newtonsoft.Json (for JSON deserialization)

System.Runtime.InteropServices (for P/Invoke operations)

API: Windows User32 SDK

📁 Project Structure
Program.cs: The main logic of the application containing API calls and loops.

keys.json: Configuration file containing key codes (digit) and their corresponding string values (value).

keyLogger.txt: The output file generated while the application is running.

⚙️ Setup and Execution
Clone this repository:

Bash

git clone https://github.com/yourusername/keymapper-logger.git
Open the .sln file with Visual Studio.

Install the Newtonsoft.Json package via NuGet Package Manager.

Build and run the project.

[!IMPORTANT]
Legal Disclaimer: This tool is intended for educational and analytical purposes only. Unauthorized use or monitoring of data without consent is illegal. The user is solely responsible for any legal consequences arising from the use of this software.

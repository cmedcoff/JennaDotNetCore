# JennaDotNetCore

## A software "scratch pad" for tutoring one of my DotNet students.

### Planning (9/31/2023)

<summary>We put together an outline to list potential areas within DotNet to get started on.
The decision was to focus on .NET 6 and 7, Console Apps and IOC, Configuration and Logging in 
preparation for using those same things in ASP.NET Web MVC and Razor Pages.
The outline is here below:</summary>

<details>

- .NET (Full Framework) Old/Legacy (4.8?)
- .NET (Core - the new stuff)
	- Core 1
	-	Core 2
	-	Core 3
	-	Core 4 (Microsoft skipped to avoid confusion with Full Framework)
	-	5  - Out of Support - Dropped "core" now just ".NET"
	-	6 - LTS (3 year)
	-	7 - STS (1 year)
	-	8 - Preview out soon?
	
- Tooling
	- Visual Studio (Windows only, Community)
	- Visual Studio for Mac
	- VS Code (free, portable)
		- fallback to the command line
			- demo
	- Jetbrains - Rider (Python IDE PyCharm)
	
- Console Apps
- 	Focus here is on Host.GetDefaultBuilder()
	- 	Convers things that are used in all .NET (Core) applications such as
	- 	Configuration, 
	- 	Logging, 
	- 	IOC - Inversion of Control - 
	- 	BackGround Versus Console

- Web Apps
	- 	An overview of the difference between MVC and Razor
	- 	MVC
	- 	Razor Pages
	- 	An overview of SOAP versus Rest - History of API Development (WCF)
	- 	(REST) API
	- 	SPA's  - JavaScript - Angular, Vue, React
	- 	Blazer

- BackgroundServices
	- 	Plain
	- 	Windows Service
	- 	Linux Daemon
	- 	Other deployment models for the cloud

- Database
	- 	Sql Server - stand alone server, "localdb"
	- 	Sqlite

- "Greenfield" Development
- "Brownfield" Development

</details>

### 9/31/2023 

<summary>

- We began our investigation of "IOC" Inversion of Control Containers.
We started with first principles - Robert Cecil Martin's Dependency
Inversion Principle - and how to implement with either "pure DI" or
using and IOC.

- While we will focus on using Visual Studio for tooling, we discussed the need to use the 
"dotnet" command from a command prompt to perform some operations as where Visual
Studio offers no equivlent feature/function.  We walked through creating
a console app using the "dotnet" command and looked the code and the option to use or not 
use "top level statments" versus the traditional "static main" approach.


</summery>
<details>

- See the ConsoleAppDip1 project
- see https://blog.ploeh.dk/2014/06/10/pure-di/
	- I highly recommend this guys's book on DI

</details>


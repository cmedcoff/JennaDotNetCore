# JennaDotNetCore

## A software "scratch pad" for tutoring one of my DotNet students.

### Planning (8/31/2023)

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

### 8/31/2023 

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


</summary>
<details>

- See the ConsoleAppDip1 project
- see https://blog.ploeh.dk/2014/06/10/pure-di/
	- I highly recommend this guys's book on DI

</details>

### 9/7/2023 

<summary>

- We continue our investigation of "IOC" Inversion of Control Containers.

</summary>
<details>
- We reviewed Jenna's investigation/coding of interfaces and concrete classes
- We (accidently) ran into the use case of having more than one implementation of an 
- interface
</details>

### 9/14/2023 

<summary>

- We continue our investigation of "IOC" Inversion of Control Containers.
- We take on the use case of having more than one implementation of an interface
- We take a look at writing our own rudementary ioc container to gain some insight into
how a full fledged ioc container works it's "magic"


</summary>
<details>

- See Session3 consoleFromWorker1 as we continue to modify it from Session2.  We also use
app as a segeway into .NET (Core) configuration which is very different and more complex, but
more flexible than .NET Full Framework's app.config and web.config files.

- we'll debug our way through See Session3\IocExample console app in order to debug our own
IOC Container which uses .NET Reflection as is pretty much how any IOC Container is 
implemented.

	- No support for concrete class registration or resolution
	- No support for different lifetimes

</details>

### 9/21/2023
<summary>
- Session was cancelled
</summary>

### 9/28/2023
<summary>

- Quick review of Session 3 with regards to IOptions of T
- Overview/Examples of ILogger and ILogger of T

</summary>
<details>

- IOptions Pattern see https://learn.microsoft.com/en-us/dotnet/core/extensions/options
- Logging see https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
- debates and trade-offs of .NET (Core) Logging see
https://stackoverflow.com/questions/51345161/should-i-take-ilogger-iloggert-iloggerfactory-or-iloggerprovider-for-a-libra

</details>

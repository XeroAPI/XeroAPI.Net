XeroApi .Net Library 
====================

Unsupported - no longer under active development
------------
This library is unsupported. Please see [https://github.com/XeroAPI/Xero-Net](https://github.com/XeroAPI/Xero-Net) for the updated and supported .Net library.

Introduction
------------

This is a .Net wrapper library, written in C#, used to comnunicate with the Xero Core API (http://api.xero.com).  There is a branch that adds support for the [Payroll API](https://github.com/XeroAPI/XeroAPI.Net/tree/payroll) [(payroll API documentation)](http://developer.xero.com/documentation/payroll-api/overview/)

You can use this library to communicate with the Xero API, without needing any prior knowledge of Xml or OAuth. The library can work in [public](http://developer.xero.com/api-overview/setup-an-application/#public-apps), [private](http://developer.xero.com/api-overview/setup-an-application/#private-apps) or [partner](http://developer.xero.com/api-overview/setup-an-application/#partner-apps) mode.

Note: Unless you're looking to debug the library, you don't need to download the source code and compile it yourself. Each release is published to the NuGet package repository (http://nuget.org/List/Packages/XeroAPI.Net).

If you have any questions or problems using this library, have a look at the Xero API Answers site (https://community.xero.com/developer/).


Features
--------

This library contains a linq provider for writing linq queries:

	var bankAccounts = from account in repository.Accounts
					   where account.Type == "BANK"
					   select account;

Or, using linq extension methods...

	int invoiceCount = repository.Contacts
		.Where(c => c.UpdatedDateUTC >= DateTime.UtcNow.AddMonths(-1))
		.Count();

Entities can be created or updated using the familiar repository pattern:

	Contact contact = new Contact
	{
		Name = "Joe Bloggs",
		FirstName = "Joe",
		LastName = "Bloggs",
		EmailAddress = "joe.bloggs@nowhere.com"
	};
	
	repository.Create(contact);


License
-------
This software is published under the [MIT License](http://en.wikipedia.org/wiki/MIT_License).

Portions of this software were taken from the DevDefined.OAuth library (https://github.com/bittercoder/DevDefined.OAuth).

	Copyright (c) 2011 Xero Limited

	Permission is hereby granted, free of charge, to any person
	obtaining a copy of this software and associated documentation
	files (the "Software"), to deal in the Software without
	restriction, including without limitation the rights to use,
	copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the
	Software is furnished to do so, subject to the following
	conditions:

	The above copyright notice and this permission notice shall be
	included in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
	OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
	NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
	HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
	WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
	OTHER DEALINGS IN THE SOFTWARE.

	
Installation
------------
There are 2 ways to install this library:

1. Download the source code from github and compile yourself:
 **https://github.com/XeroAPI/XeroAPI.Net**

2. Download directly into Visual Studio using the NuGet powershell command:
 **PM&gt; Install-Package XeroAPI.Net**


Usage
-----

There are sample projects in the git repository show how to use the library:

1. **XeroApi.Console** - A console app that can run in public, private or partner mode.
2. **XeroApi.MvcWebApp** - An ASP.Net MVC2 web app that runs in partner mode.

In fact, here's one I wrote earlier:

        static void Main(string[] args)
        {
            IOAuthSession session = new XeroApi.OAuth.XeroApiPrivateSession(
                "XeroAPI Mini App",
                "YOUR-CONSUMER-KEY",
                new X509Certificate2(@"D:\Your-Certificate.pfx", "your-pfx-password"));

            Repository repository = new Repository(session);
            
            Console.WriteLine("You're connected to " + repository.Organisation.Name);
            Console.ReadLine();
        }

.

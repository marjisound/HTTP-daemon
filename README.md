# HTTP-daemon
This project is a simple HTTP Server in C# 

You can run this server through Command Prompt using the exe file or Through Visual Studio using the solution. 

The TCP port number is 5000 by default but if necessary, the port can be changed by passing it as the command prompt variable. 

There are a number of end points that are handled on this server as following:

Time
List
Random
Prime
Cache

The Program.cd contains the Main method that is the starting point of the program. 

The logic of each endpoint that could be considered as a resourse is put in its own class. But the main switch that decides which end point should be called is located in the Program.cs. 

The HttpServer class contains the part of the code dealing with HttpListener. The credit for the content of this class goes to David's Blog on https://codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server



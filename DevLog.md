# Dev Log
I want to chronicle my journey throughout this project. This will be a place to compile my thoughts and explain what I'm doing and why I'm doing it.

## Log 4 - System Management
2/17/24

After I got the Logger System up and running, I started to turn my attention towards implementing the next system. Before getting too far however, I wanted to make sure we had a good basis for System management. In order to create some standardization, I decided that utilizing an ISystem interface for all of the systems would be a good idea. An interface is really only supposed to define the aspects of a class that other classes need to know, and as such is inherently lightweight. I didnt want to put too much detail in to this, because each of the systems will end up being considerably different from each other. This was another factor in deciding to start with an interface, as opposed to an abstract class.

For the interface, I wanted to define two things: Some fields to capture basic info about the System, and some methods used in the control and deployment of all systems. This is what the interface looks like right now:
```
public interface ISystem
{
    string SystemName { get; }
    ESystemType SystemType { get; }
    ESystemStatus SystemStatus { get; }
    event EventHandler<SystemStatusChangedEventArgs> SystemStatusChanged;

    ManualResetEvent SystemShutdownEvent { get; }

    void InitializeSystem();
    void StartSystem();
    void StopSystem();

    void UpdateSystemState();
}
```
I ended up defining a few more things, but each of those extra thigns is in support of the main purpose of the interface. The SystemStatusChanged Event is an event that gets fired when the SystemStatus changes. The SystemShutdownEvent is an event that can be querried to know if a system has finished shutting down or not. And lastly the UpdateSystemState exposed a public method that will make sure the SystemStatus is accurate.

Now that we have a common interface for systems designated, we can more efficently manage the various Systems from the ServerMasterController. First, we want to create a master list of all of the Systems. We add systems to that list from the Initialize method. The order that we add systems to the list matters however. The Systems will get started in the order they are added to the list, and they will be stopped in the reverse order. I'm not confident that this will always be the case, but at least for where I'm at on this project currently, that flow made sense to me. Once the Systems are added to the list, it because trivial to iterate over the list and call Start or Stop.

```
public void Initialize()
{
    if (!Initialized)
    {
        // Always add the logger first
        AddSystem(Logger.Instance);

        // The ClientManager relies on data from the GameManager
        // You need to add the GameManager before the ClientManager
        AddSystem(GameManager);
        AddSystem(ClientManager);

        Initialized = true;
    }
}

// Start all Systems 
public void StartServer()
{
    // Start each system in the order it was added to the MasterSystemList
    foreach (var system in MasterSystemList)
    {
        system.StartSystem();
    }
}

// Stop all Systems 
public void StopServer()
{
    // Stop each system in the reverse order it was added to the MasterSystemList
    foreach (var system in MasterSystemList.Reverse<ISystem>())
    {
        system.StopSystem();
    }
}
```
You may have noticed that in the Initialize method, we use both a singleton, as well as class instances when adding to the list. Because the ISystem interface doesnt care what you are, we are able to do this!

The last thing to do was to create a UI element to report all of these details. I created a simple user control that contains a table layout panel with 3 columns, one for each of the fields associated with each system. Because I dont know how many systems I may end up having, I needed this user control to be able to dynamically add Systems to it. The base user control doesnt actually display anything until someone calls the control's AddSystem() method. When this method is called, it creates a new row in the table layout, creates a label to put in each of the columns, and then registers an eventhandler to the systems SystemStatusChanged event.

Now we can start our server, and see the status of each of our 3 systems so far!

## Log 3 - Custom Logger & Asynchronous Programming
2/6/24

I figured that the first System that I should set up would be the logging system. This system should be the first system that the server starts during Initialization, and the last system that the server shuts down during Shutdown. Additionally, this system will become incredibly useful to assist with logging errors and debugging. I want the Logger to do two things. First and foremost, it should log messages to a file. This is the primary function of the logger. Secondly though, I wanted to expose a way for the Logger to notify other systems when it logs a message, in case those other systems want to do something with those messages. My initial use case for this is that I want to build in a pseudo console window in to the application itself to see some of the debug messages during development. To accomplish this, the Logger would just need to include a MessageLogged event, and to pass the LoggerMessage to any registered event handlers.

Initially, I wanted to try out asynchronous programming, which is something that I have very little experience with. Originally, I figured that I could make a Logger class that just contained some static methods. Then when a LogMessage call was made, I could write to the file and any event handlers asynchronously. This is where my understanding of asynchronous was lacking. In my head, I was using the word asynchronous to mean "fire and forget". I assumed that I could launch a LogMessage call, and that whatever other system or thread made that call would just fire the function and then continue. That is not how asynchronous works. 

Asynchronous programming works like this. Typically, when you make a function call, you cant do anything else until that function call returns. Asynchronous programming allows you to defer receiving the the return value of that method call until later. Lets create an example. Lets say that I have several chores I need to get done before I can leave the house. Those chores are doing laundry, making a sandwich, putting away the dishes, and then folding the clean clothes. In a synchronous programming model, if I started doing laundry with the DoLaundry() method, I would have to wait until DoLaundry() had finished before I could move on to MakeSandwich(). The pseudocode would look something like this:
```
bool laundryFinished = DoLaundry();

bool sandwichMade = MakeSandwich();

if(sandwichMade)
  PutAwayDishes();

if(laundryFinished)
  FoldLaundry();
```
Obviously you wouldn't want to wait for DoLaundry() to finish before you move on to MakeSandwich(). You would want DoLaundry() to work in the background while you perform the MakeSandwich() and PutAwayDishes() methods. This is where asynchronous programming comes in. It allows you to capture an executing method as a Task, and then get the results of the task later. To update the example code, it would now look like this:
```
Task<bool> laundryTask = DoLaundry();

bool sandwichMade = MakeSandwich();

if(sandwichMade)
  PutAwayDishes();

bool laundryFinished = await laundryTask;

if(laundryFinished)
  FoldLaundry();
```
Now in this version of code, you would start the DoLaundry() task, move on to the MakeSandwich() and PutAwayDishes() methods, and then wait for the laundryTask to finish before proceeding. 

Asynchronous can be really powerful and can save a lot of executing time if used correctly. The problem with what I was trying to do was that I wanted to call the LogMessage() method, and then continue with whatever else I was doing, but never wait for the task to finish. That is not how you are supposed to use asynchronous programming. After consulting the oracle, I decided that what I really wanted was to have a queue of messages, and when I wanted a system to log a message, all that that method would do is add the message to the queue, instead of waiting for the message to be written to the file and to all of the event handlers. I would then simply create a background thread that would continually check the queue for messages, and the background thread would write the messages to the file and to the event handlers. Its not so much that I was implementing a "fire and forget" pattern, it was more so that I was implementing a "hand off to someone else" pattern. This seemed to work well and accomplish what I was looking for!


## Log 2 - Initial Design Considerations
2/3/24

In the past, I would have just jumped right in and started implementing features. For this project, I want to be a bit more methodical than that. I'm going to start by laying out some initial desing considerations. For starters, I may end up reusing some of this code in the future for different projects/games, so I will try to make my code modular and reusable. Secondly, I like the idea of games having a built in modding capability, so whenever/whereever it makes sense, I'm going to implement public APIs and allow for the community the bandwidth to mod the game how they see fit. And yes, I know right now that there isnt actually a player community to use the APIs. This is more an exercise in thinking and designing with this aspect in mind. 

The first place I'm going to start is working on getting some of the initial server capabilities up and running. Afterall, a client can't do anything if it doesnt have a server to connect to. 

We first need to figure out some essentials in terms of how we organize and structure the server. To do that, lets think about what the server needs to do. The server needs to do several things right off the bat. When the server starts, it needs to start listening for and establishing connections with clients. After the connection is established, the client will need to either register or log in. After a client is logged in and authenitcated, it needs to process actions/requests. While all of this is going on, a primary game simulation needs to be running in the background. This is the main game loop. It simulates everything that goes on in the game world, and reacts to inputs by the clients. The allow for persistence, all of this data needs to be stored in a database. Lastly, I want to be able to see some metrics about how the server is running. This would be a form that displays information such as the state of the server, a list of client connections, etc. 

Keeping all of this in mind, I will use a mixture of multithreading, as well as asynchronous methods. The server will utilize 3 primary threads: 1. The UI Thread; 2. The Game Loop Thread; 3. The Client Manager Thread. 

**UI Thread:**

Handles the user interface and interactions with server administrators or operators.\
Remains responsive to user input and displays real-time information about the server's status.\
Updates and displays logs, server statistics, and relevant information to the user.

**Game Loop Thread:**

Simulates and maintains the game state, including game logic and world updates.\
Runs at a consistent rate to ensure smooth gameplay for connected clients.\
Receives and processes game-related actions or events from clients and updates the game state accordingly.

**Client Manager Thread:**

Manages client connections, such as accepting incoming connections using a TcpListener.\
Listens for client requests, receives input from clients, and dispatches these requests to the appropriate systems (e.g., game logic, database).\
Coordinates communication between clients and other server systems, making asynchronous calls to handle I/O-bound operations.\
Provides a central point of contact for client-related activities and maintains a list of connected clients.


In addition to the three main threads, there will be several "Systems" utilized by the threads. These systems will encapsulate various functions, such as communicating with a database, file IO, debugging/logging. The threads will communicate with these Systems with asynchronous tasks. 

For my first set of commits, I will focus on getting these threads up and running, and will begin to implement some of these systems. 

## Log 1 - Project Beginnings
2/3/24 

I have long been fascinated with programming and software development. When I went to college, I originally intended to double major in Physics and Computer Engineering. I thought I wanted to go down the hardware route. This was mostly based on my interests with building my own gaming PC. When I got further into college, I took an intro programming course and absolutely loved it. I loved how everything was so linear and logically and organized. I had control over everything that my code did, and I loved it! (Hindsight, this should have been obvious given my control freak tendencies ha!) I quickly changed my major from Physics to Computer Science. I got super in to programming, I loved all my classes. I loved everything about programming. I loved writing code; I loved teaching others about it. It was a common occurrence that the night before large tests, several other students would show up to the computer lab knowing that I would be there ready to help others cram for the tests, or learn things for the first time! I excelled in all my classes, and by the time of graduation was one of the top students!

Naturally when I graduated, I got a job as a programmer. I started off at an aerospace and defense company called Raytheon. (Yes, the huge defense contractor Raytheon.) I was fortunate enough to work on one of the better programs at the time. The purpose of the program was essentially to implement a moving map touchscreen smart display system, similar to the GPS type moving maps you have in smart cars now, except we implemented it on the HH-60G Combat Search and Rescue helicopter, and instead of showing you where the nearest gas station or Chick-Fil-A is, it would show you where other aircraft were, where the person you were supposed to pick up was located, placed and objects to avoid, etc. It was honestly a fantastic program to start out my professional career with. However, I quickly realized that programming large real-world applications was considerably different than programming the relatively small and succinct applications we created in college. Things moved slowly. We spent a lot of time on design, a lot of time on testing, and nowhere near as much time on actual programming as I had hoped for. I also was introduced to the notion that often times in industry, you don't get to make decisions based on what would create the best product. You sometimes have to make decisions based on the budget or timeline that you are given. I found this to be an especially aggravating aspect of professional software development. There are a lot of details to my career path that I could share here, but for the sake of brevity, I will simply say that I was only a professional programmer for a couple of years before I moved away from software development and in to Project Management. This was definitely the right career move for me, but it always left me longing for a way to scratch the programming itch.

Over the years, I have started (and mostly never finished) many different programming endeavors in my free time. At one point I took a stab at creating a game engine from scratch. I spent some time diving in to graphics programming. I got thru basic prototyping on a dozen different small desktop apps to do things like manage a to-do list, send email reminders, create and track budgets. None of them were ever finished, or even used ha! I always had the programming itch, but never had any ideas succinct enough to keep my attention or to see thru to completion. It was always a flare up where I would get all excited for a week or so, write a few hundred or thousand lines of code, and then forget and move on. 

More so recently, I finally found a project to keep my attention for a while. The task was to create a Gameboy emulator. Yes I know a ton already exist, but I was looking for something to work on but struggling to come up with ideas, and at least with programming an emulator, I already knew what the end product was supposed to look like, so I could just focus on the programming aspects. I got quite a ways in to the emulator, getting so far as to get a few things rendering on a screen, but then I started to encounter some bugs, and my interest started to wane again. I was encouraged by my wife to keep at, and so I decided to start over on the emulator project. This time though, I decided that rather than just jumping in head first, I should slow down, take my time, and try to be a bit more methodical in my approach. So I started up a GitHub repository, started a wiki, and started to journey what I was doing and why I was doing it. It definitely slowed the progress in terms of actually coding, but what I found out was that I was actually enjoying the process even more, and because I was taking the time to journey things, I was passively doing design work at the same time. The second attempt at the emulator turned out considerably better than the first attempt, and even though the full emulator is still not finished, it very well might be the first project I have started that I am actually proud of. 

This leads us to this project. I have always been interested in video game development. However, my interests have generally fallen less on the graphical or gameplay elements, and more on the server side, backend aspects of game design. I remember years ago reading articles about how some of these large video game companies sunk millions of dollars in to very particular and detailed server architectures to account for the millions of concurrent users. I always found that aspect to be interesting. I will probably never get to this point, but part of me has always wanted to try my hand at programming the server side of a large MMO like World of Warcraft or Final Fantasy XIV. So, in an effort to create a project that would keep my interest, and would at least start to move me closer to the MMO realm, I have come up with the concept of Grindscape. The idea is to take the game Runescape, and try to recreate it, but in the format of an idle game, instead of an active game. There are several reasons for this design concept. For starters, I play a lot of idle game personally. I also have played Runescape for decades, and maintain an active membership. In creating an idle game, I don’t have to put much time in to the client UI side of things, because everything can put on the screen in terms of simple buttons or selections. This means I don’t have to sink time in to graphics or anything like that. Graphics is an interest of mine, but that’s for much further down the road. Additionally, with this being a "MMO Idler", it allows me to opportunity to develop a game server, and to work on the networking aspects of games. This is perfect because those are the two aspects that I am most interested in at the moment. So to get this game off the ground, I will focus on those two aspects first. As this game evolves, and I complete these other aspects of the game, I may incorporate new aspects, such as 3D graphics, or more active gameplay. Also, with this being a project more interesting in experience the programming aspects, and less about actually creating a shippable game, it frees me from some of the decisions that come when profit is a motivator. So that it, that’s the first post. I will continue to journal my thoughts along the way. To anyone who has made it this far, welcome and I hope you enjoy the ride!
